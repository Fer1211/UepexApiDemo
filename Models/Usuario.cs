namespace UepexApiDemo.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        // Ej: "admin", "g.rodriguez", etc. NombreUsuario va a ser lo que el usuario usa para hacer login.
        public string NombreUsuario { get; set; } = string.Empty;

        // Aquí guardamos la contraseña encriptada (hash). ClaveHash es la contraseña hasheada con bcrypt.
        public string ClaveHash { get; set; } = string.Empty;

        // Para control de permisos más adelante. Rol lo usamos luego en
        // [Authorize(Roles = "Administrador")], por ejemplo.
        public string Rol { get; set; } = "Usuario";
    }
}
