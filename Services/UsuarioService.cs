using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UepexApiDemo.Data;
using UepexApiDemo.Models;

namespace UepexApiDemo.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(ApplicationDbContext context, ILogger<UsuarioService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Usuario?> AutenticarAsync(string nombreUsuario, string clave)
        {
            // Normalizamos lo que recibimos del controller
            var normalizado = nombreUsuario?.Trim().ToLower();

            _logger.LogInformation(
                "[USUARIO SERVICE] Intentando autenticar. Raw='{Raw}', Normalizado='{Norm}'",
                nombreUsuario, normalizado
            );

            // Debug: ver qué ve EF en ESTA base
            var todos = await _context.Usuarios.ToListAsync();
            _logger.LogInformation("[USUARIO SERVICE] Usuarios en BD actual = {Count}", todos.Count);
            foreach (var u in todos)
            {
                _logger.LogInformation(
                    "[USUARIO SERVICE] -> Id={Id}, NombreUsuario={NombreUsuario}, Rol={Rol}",
                    u.Id, u.NombreUsuario, u.Rol
                );
            }

            // Buscar usuario ignorando mayúsculas/minúsculas
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario.ToLower() == normalizado);

            if (usuario == null)
            {
                _logger.LogWarning(
                    "[USUARIO SERVICE] No se encontró usuario '{Norm}' en la BD actual",
                    normalizado
                );
                return null;
            }

            _logger.LogInformation(
                "[USUARIO SERVICE] Usuario encontrado en BD. Id={Id}, Rol={Rol}",
                usuario.Id, usuario.Rol
            );

            // Verificar contraseña con BCrypt
            bool passwordOk = BCrypt.Net.BCrypt.Verify(clave, usuario.ClaveHash);
            _logger.LogInformation(
                "[USUARIO SERVICE] ¿Password válida? {PasswordOk}",
                passwordOk
            );

            if (!passwordOk)
            {
                _logger.LogWarning(
                    "[USUARIO SERVICE] Clave inválida para '{User}'",
                    usuario.NombreUsuario
                );
                return null;
            }

            // Asegurar que el usuario tenga un rol no vacío
            if (string.IsNullOrWhiteSpace(usuario.Rol))
            {
                usuario.Rol = "Usuario";
            }

            return usuario;
        }
    }
}
