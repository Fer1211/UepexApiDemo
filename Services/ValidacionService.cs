using UepexApiDemo.Models;
using System.Text.RegularExpressions;

namespace UepexApiDemo.Services
{
    public class ValidacionService : IValidacionService
    {
        private readonly string[] tiposValidos = { "Cedula", "Pasaporte", "RNC" };
        private readonly string[] monedasValidas = { "DOP", "USD", "EUR" };
        private readonly string[] condicionesValidas = { "Activo", "Inactivo",  "Graduado", "Nuevo ingreso" };
        private readonly string[] tiposCuentaValidos = { "Ahorro", "Corriente", "Cheque" };

        public ResultadoValidacion ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return ResultadoValidacion.Error("NOMBRE_REQUERIDO", "Se requiere el nombre del estudiante");

            // Solo letras (incluye acentos), espacios y hasta 30 caracteres
            if (!Regex.IsMatch(nombre, "^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]{2,30}$"))
                return ResultadoValidacion.Error("NOMBRE_FORMATO_INVALIDO", "El nombre debe tener solo letras y espacios, máximo 30 caracteres");

            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarApellidos(string apellidos)
        {
            if (string.IsNullOrWhiteSpace(apellidos))
                return ResultadoValidacion.Error("APELLIDOS_REQUERIDOS", "Se requieren los apellidos del estudiante");

            // Solo letras (incluye acentos), espacios y hasta 30 caracteres
            if (!Regex.IsMatch(apellidos, "^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]{2,30}$"))
                return ResultadoValidacion.Error("APELLIDOS_FORMATO_INVALIDO", "Los apellidos deben tener solo letras y espacios, máximo 30 caracteres");

            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarTipoDocumento(string tipoDocumento)
        {
            if (!tiposValidos.Contains(tipoDocumento))
                return ResultadoValidacion.Error("TIPO_DOCUMENTO_INVALIDO", "Tipo de documento debe ser Cedula, Pasaporte o RNC");
            
            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarNumeroDocumento(string numeroDocumento, string tipoDocumento)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento))
                return ResultadoValidacion.Error("NUMERO_DOCUMENTO_INVALIDO", "Se requiere el número de documento");

            switch (tipoDocumento)
            {
                case "Cedula":
                    if (!Regex.IsMatch(numeroDocumento, @"^\d{11}$"))
                        return ResultadoValidacion.Error("CEDULA_FORMATO_INVALIDO", "La cédula debe tener 11 dígitos numéricos");
                    break;
                case "RNC":
                    if (!Regex.IsMatch(numeroDocumento, @"^\d{9}$"))
                        return ResultadoValidacion.Error("RNC_FORMATO_INVALIDO", "El RNC debe tener 9 dígitos numéricos");
                    break;
                case "Pasaporte":
                    if (numeroDocumento.Length > 20)
                        return ResultadoValidacion.Error("PASAPORTE_LONGITUD_INVALIDA", "El pasaporte no puede tener más de 20 caracteres");
                    break;
            }

            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarMoneda(string moneda)
        {
            if (!monedasValidas.Contains(moneda))
                return ResultadoValidacion.Error("MONEDA_INVALIDA", "Moneda debe ser DOP, USD o EUR");

            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarCondicionEstudiante(string condicion)
        {
            if (!condicionesValidas.Contains(condicion))
                return ResultadoValidacion.Error("CONDICION_INVALIDA", "Condición del estudiante debe ser Activo, Inactivo, Graduado o Nuevo ingreso");

            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarTipoCuenta(string tipoCuenta)
        {
            if (!tiposCuentaValidos.Contains(tipoCuenta))
                return ResultadoValidacion.Error("TIPO_CUENTA_INVALIDO", "Tipo de cuenta debe ser de Ahorro, Corriente o Cheque");

            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarCuentaBancaria(string cuenta)
        {
            if (string.IsNullOrWhiteSpace(cuenta))
                return ResultadoValidacion.Error("CUENTA_BANCARIA_REQUERIDA", "Se requiere cuenta bancaria");

            if (!Regex.IsMatch(cuenta, @"^\d{6,20}$"))
                return ResultadoValidacion.Error("CUENTA_BANCARIA_FORMATO_INVALIDO", "La cuenta bancaria debe ser numérica de 6 a 20 dígitos");

            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarFecha(string fecha, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(fecha))
                return ResultadoValidacion.Error("FECHA_REQUERIDA", $"Se requiere {nombreCampo}");

            var pattern = @"^(0?[1-9]|1[0-2])\/(0?[1-9]|[12][0-9]|3[01])\/\d{4}\s(1[0-2]|0?[1-9]):[0-5][0-9]\s(AM|PM)$";
            if (!Regex.IsMatch(fecha, pattern))
                return ResultadoValidacion.Error("FECHA_FORMATO_INVALIDO", $"{nombreCampo} debe tener formato MM/dd/yyyy h:mm AM/PM");

            return ResultadoValidacion.Exitoso();
        }

        public ResultadoValidacion ValidarEstudianteCompleto(Estudiante estudiante)
        {
            var errores = new List<string>();

            var validaciones = new[]
            {
                ValidarNombre(estudiante.NombreEstudiante),
                ValidarApellidos(estudiante.ApellidosEstudiante),
                ValidarTipoDocumento(estudiante.TipoDocumento),
                ValidarNumeroDocumento(estudiante.NumeroDocumento, estudiante.TipoDocumento),
                ValidarMoneda(estudiante.Moneda),
                ValidarCondicionEstudiante(estudiante.CondicionEstudiante),
                ValidarTipoCuenta(estudiante.TipoCuenta),
                ValidarCuentaBancaria(estudiante.CuentaBancariaEstudiante),
                ValidarFecha(estudiante.AsistenciaFechaHoraEntrada, "asistenciaFechaHoraEntrada"),
                ValidarFecha(estudiante.AsistenciaFechaHoraSalida, "asistenciaFechaHoraSalida")
            };

            foreach (var validacion in validaciones)
            {
                if (!validacion.EsValido)
                {
                    if (validacion.Errores != null && validacion.Errores.Any())
                        errores.AddRange(validacion.Errores);
                    else
                        errores.Add(validacion.DescripcionRespuesta);
                }
            }

            if (errores.Any())
                return ResultadoValidacion.ErrorMultiple("ERRORES", errores);

            return ResultadoValidacion.Exitoso();
        }
    }
}
