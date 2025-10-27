using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using UepexApiDemo.Services;

namespace UepexApiDemo.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration config, IUsuarioService usuarioService, ILogger<AuthController> logger)
        {
            _config = config;
            _usuarioService = usuarioService;
            _logger = logger;
        }

        public class LoginRequest
        {
            [JsonPropertyName("usuario")]
            public string Usuario { get; set; } = string.Empty;

            [JsonPropertyName("clave")]
            public string Clave { get; set; } = string.Empty;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // 📜 LOG INICIAL: Mostrar lo que llega desde el cliente
            _logger.LogInformation("---- LOGIN REQUEST ----");
            _logger.LogInformation("Body recibido: Usuario='{Usuario}', Clave='{Clave}' (longitud={Len})",
                request?.Usuario, request?.Clave, request?.Clave?.Length ?? 0);
            _logger.LogInformation("------------------------");

            if ( request == null|| string.IsNullOrWhiteSpace(request.Usuario) || string.IsNullOrWhiteSpace(request.Clave))
            {
                _logger.LogWarning("Body incompleto o nulo. Usuario='{Usuario}', Clave='{Clave}'",
                    request?.Usuario, request?.Clave);
                return BadRequest(new { mensaje = "Debe ingresar usuario y clave" });
            }

            // 1️⃣ Validar usuario/clave contra la BD
            _logger.LogInformation("Llamando a AutenticarAsync con usuario '{Usuario}'", request.Usuario);

            var usuario = await _usuarioService.AutenticarAsync(request.Usuario, request.Clave);

            if (usuario == null)
            {
                _logger.LogWarning("Autenticación fallida para usuario '{Usuario}'", request.Usuario);
                return Unauthorized(new { mensaje = "Usuario o clave incorrectos" });
            }

            _logger.LogInformation("Usuario autenticado correctamente: '{Usuario}' con rol '{Rol}'",
                usuario.NombreUsuario, usuario.Rol);

            // 2️⃣ Construir el token JWT
            var jwtSection = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var rol = string.IsNullOrWhiteSpace(usuario.Rol) ? "Usuario" : usuario.Rol;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.NombreUsuario),
                new Claim("role", rol),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSection["ExpiresInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _logger.LogInformation("Token generado correctamente para usuario '{Usuario}', expira en {Expira}",
                usuario.NombreUsuario, expires);

            return Ok(new
            {
                access_token = tokenString,
                token_type = "Bearer",
                expires_at = expires,
                usuario = usuario.NombreUsuario,
                rol = rol
            });
        }
    }
}
