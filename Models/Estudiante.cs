using System.ComponentModel.DataAnnotations;

namespace UepexApiDemo.Models
{
    public class Estudiante
    {
        [Required(ErrorMessage = "Tipo de documento es requerido")]
        public string TipoDocumento { get; set; } = string.Empty;  // Cedula, RNC, Pasaporte
        
        [Required(ErrorMessage = "Número de documento es requerido")]
        [StringLength(20, ErrorMessage = "Número de documento no puede exceder 20 caracteres")]
        public string NumeroDocumento { get; set; } = string.Empty; // antes CedulaEstudiante
        
        [Required(ErrorMessage = "Nombre del estudiante es requerido")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Nombre debe tener entre 2 y 30 caracteres")]
        public string NombreEstudiante { get; set; } = string.Empty; // nombre del estudiante
        
        [Required(ErrorMessage = "Apellidos del estudiante son requeridos")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Apellidos deben tener entre 2 y 30 caracteres")]
        public string ApellidosEstudiante { get; set; } = string.Empty; // apellidos del estudiante
        
        [Required(ErrorMessage = "Código del curso es requerido")]
        [StringLength(10, ErrorMessage = "Código del curso no puede exceder 10 caracteres")]
        public string CodigoCurso { get; set; } = string.Empty; // codigo del curso, 01, 02, 03, etc.
        
        [Required(ErrorMessage = "Condición del estudiante es requerida")]
        public string CondicionEstudiante { get; set; } = string.Empty; // Activo, Inactivo, Graduado, Nuevo ingreso
        
        [Required(ErrorMessage = "Nombre del curso es requerido")]
        [StringLength(100, ErrorMessage = "Nombre del curso no puede exceder 100 caracteres")]
        public string Curso { get; set; } = string.Empty; // nombre del curso, taller o tecnico que se esta cursando
        
        [Required(ErrorMessage = "Regional es requerida")]
        [StringLength(50, ErrorMessage = "Regional no puede exceder 50 caracteres")]
        public string Regional { get; set; } = string.Empty; // Lugar donde esta cursando el estudiante
        
        [Required(ErrorMessage = "Tipo de cuenta es requerido")]
        public string TipoCuenta { get; set; } = string.Empty; // Ahorro, Corriente, Cheque
        
        [Required(ErrorMessage = "Moneda es requerida")]
        public string Moneda { get; set; } = string.Empty; // DOP, USD, EUR
        
        [Required(ErrorMessage = "Nombre del banco es requerido")]
        [StringLength(50, ErrorMessage = "Nombre del banco no puede exceder 50 caracteres")]
        public string Banco { get; set; } = string.Empty; // nombre del banco
        
        [Required(ErrorMessage = "Cuenta bancaria es requerida")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Cuenta bancaria debe tener entre 6 y 20 caracteres")]
        public string CuentaBancariaEstudiante { get; set; } = string.Empty; // numero de la cuenta bancaria
        
        [Required(ErrorMessage = "Nombre de la cuenta bancaria es requerido")]
        [StringLength(100, ErrorMessage = "Nombre de la cuenta bancaria no puede exceder 100 caracteres")]
        public string NombreCuentaBancaria { get; set; } = string.Empty; // nombre de la cuenta bancaria
        
        [Range(0, int.MaxValue, ErrorMessage = "Asistencia debe ser un número positivo")]
        public int Asistencia { get; set; }  // numero de asistencias
        
        [Range(0, int.MaxValue, ErrorMessage = "Inasistencia debe ser un número positivo")]
        public int Inasistencia { get; set; }   // numero de inasistencias
        
        [Required(ErrorMessage = "Fecha de entrada es requerida")]
        public string AsistenciaFechaHoraEntrada { get; set; } = string.Empty;  // fecha y hora de entrada
        
        [Required(ErrorMessage = "Fecha de salida es requerida")]
        public string AsistenciaFechaHoraSalida { get; set; } = string.Empty;  // fecha y hora de salida
    }
}
