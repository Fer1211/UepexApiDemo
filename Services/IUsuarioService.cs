using UepexApiDemo.Models;

namespace UepexApiDemo.Services
{
    public interface IUsuarioService
    {
        Task<Usuario?> AutenticarAsync(string nombreUsuario, string clave);
    }
}
