namespace UepexApiDemo.Models
{
    public class UepexResponse
    {
        public Estudiante? Estudiante { get; set; }
        public string? Mensaje { get; set; }
        public string? Codigo { get; set; }
        public List<string>? Errores { get; set; }
    }
}