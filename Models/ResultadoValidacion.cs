namespace UepexApiDemo.Models
{
    public class ResultadoValidacion
    {
        public string CodigoRespuesta { get; set; } = string.Empty;
        public string DescripcionRespuesta { get; set; } = string.Empty;
        public List<string>? Errores { get; set; } = null;
        public bool EsValido => CodigoRespuesta == "CV000";

        // Devuelve una validación exitosa
        public static ResultadoValidacion Exitoso()
        {
            return new ResultadoValidacion
            {
                CodigoRespuesta = "CV000",
                DescripcionRespuesta = "Validación exitosa",
                Errores = null
            };
        }

        // Devuelve un solo error
        public static ResultadoValidacion Error(string codigo, string descripcion)
        {
            return new ResultadoValidacion
            {
                CodigoRespuesta = codigo,
                DescripcionRespuesta = descripcion,
                Errores = new List<string> { descripcion }
            };
        }

        // Devuelve múltiples errores
        public static ResultadoValidacion ErrorMultiple(string codigo, List<string> errores)
        {
            return new ResultadoValidacion
            {
                CodigoRespuesta = codigo,
                DescripcionRespuesta = "Se encontraron errores en la validación",
                Errores = errores
            };
        }
    }
}
