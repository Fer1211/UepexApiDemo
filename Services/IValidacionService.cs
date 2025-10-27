using UepexApiDemo.Models;

namespace UepexApiDemo.Services
{
    public interface IValidacionService
    {
        ResultadoValidacion ValidarTipoDocumento(string tipoDocumento);
        ResultadoValidacion ValidarNumeroDocumento(string numeroDocumento, string tipoDocumento);
        ResultadoValidacion ValidarNombre(string nombre);
        ResultadoValidacion ValidarApellidos(string apellidos);
        ResultadoValidacion ValidarMoneda(string moneda);
        ResultadoValidacion ValidarCondicionEstudiante(string condicion);
        ResultadoValidacion ValidarTipoCuenta(string tipoCuenta);
        ResultadoValidacion ValidarCuentaBancaria(string cuenta);
        ResultadoValidacion ValidarFecha(string fecha, string nombreCampo);
        ResultadoValidacion ValidarEstudianteCompleto(Estudiante estudiante);
    }
}
