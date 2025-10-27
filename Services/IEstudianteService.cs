using UepexApiDemo.Models;

namespace UepexApiDemo.Services
{
    public interface IEstudianteService
    {
        Estudiante GetEstudianteEjemplo();
        ResultadoValidacion ProcesarEstudiante(Estudiante estudiante);
        Task<ResultadoValidacion> GuardarEstudianteAsync(Estudiante estudiante);
        Task<List<Estudiante>> ObtenerTodosLosEstudiantesAsync();
        Task<Estudiante?> ObtenerEstudiantePorDocumentoAsync(string numeroDocumento);
        Task<byte[]> ExportarEstudiantesACsvAsync();
        Task<byte[]> ExportarEstudiantesAExcelAsync();
    }
}
