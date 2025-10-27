using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UepexApiDemo.Models;
using UepexApiDemo.Services;

namespace UepexApiDemo.Controllers
{
    /// <summary>
    /// Controlador para gestión de estudiantes UEPEX
    /// </summary>
    [ApiController]
    [Route("api/v1/uepex")]
    public class UepexController : ControllerBase
    {
        private readonly IEstudianteService _estudianteService;
        private readonly ILogger<UepexController> _logger;

        public UepexController(
            IEstudianteService estudianteService,
            ILogger<UepexController> logger)
        {
            _estudianteService = estudianteService;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint público de ejemplo. Devuelve un estudiante de prueba.
        /// No requiere token. Útil para probar que la API está viva.
        /// </summary>
        /// <returns>Datos de un estudiante de ejemplo</returns>
        /// <response code="200">Estudiante de ejemplo obtenido exitosamente</response>
        [HttpGet]
        [AllowAnonymous] // <-- este endpoint es público
        [ProducesResponseType(typeof(UepexResponse), 200)]
        public IActionResult Get()
        {
            try
            {
                _logger.LogInformation("Solicitando estudiante de ejemplo");

                var estudiante = _estudianteService.GetEstudianteEjemplo();

                var response = new UepexResponse
                {
                    Estudiante = estudiante,
                    Mensaje = "Resultado Exitoso",
                    Codigo = "OK"
                };

                _logger.LogInformation("Estudiante de ejemplo generado exitosamente");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando estudiante de ejemplo");
                return StatusCode(500, new UepexResponse
                {
                    Estudiante = null,
                    Mensaje = "Error interno del servidor",
                    Codigo = "ERROR_INTERNO",
                    Errores = new List<string> { "Error interno del servidor" }
                });
            }
        }

        /// <summary>
        /// Crea/valida/guarda un estudiante.
        /// Requiere autenticación y rol Administrador.
        /// </summary>
        /// <param name="estudiante">Datos del estudiante a procesar</param>
        /// <returns>Resultado del procesamiento</returns>
        /// <response code="200">Estudiante procesado exitosamente</response>
        /// <response code="400">Datos del estudiante inválidos</response>
        /// <response code="401">No autenticado</response>
        /// <response code="403">No autorizado (no es Administrador)</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpPost]
        [Authorize(Roles = "Administrador")] // <-- restringido por rol
        [ProducesResponseType(typeof(UepexResponse), 200)]
        [ProducesResponseType(typeof(UepexResponse), 400)]
        [ProducesResponseType(typeof(UepexResponse), 500)]
        public async Task<IActionResult> Post([FromBody] Estudiante estudiante)
        {
            try
            {
                if (estudiante == null)
                {
                    _logger.LogWarning("Se recibió un estudiante nulo");
                    return BadRequest(new UepexResponse
                    {
                        Estudiante = null,
                        Mensaje = "Datos del estudiante son requeridos",
                        Codigo = "DATOS_REQUERIDOS",
                        Errores = new List<string> { "El objeto estudiante no puede ser nulo" }
                    });
                }

                _logger.LogInformation("Procesando estudiante {NumeroDocumento}", estudiante.NumeroDocumento);

                // Validación de negocio
                var resultadoValidacion = _estudianteService.ProcesarEstudiante(estudiante);

                if (!resultadoValidacion.EsValido)
                {
                    _logger.LogWarning("Validación fallida para estudiante {NumeroDocumento}", estudiante.NumeroDocumento);
                    return BadRequest(new UepexResponse
                    {
                        Estudiante = null,
                        Mensaje = "Solicitud inválida",
                        Codigo = resultadoValidacion.CodigoRespuesta,
                        Errores = resultadoValidacion.Errores ?? new List<string>()
                    });
                }

                // Guardar en base de datos
                var resultadoGuardado = await _estudianteService.GuardarEstudianteAsync(estudiante);

                if (!resultadoGuardado.EsValido)
                {
                    _logger.LogWarning(
                        "Error guardando estudiante {NumeroDocumento}: {Error}",
                        estudiante.NumeroDocumento,
                        resultadoGuardado.DescripcionRespuesta
                    );

                    return BadRequest(new UepexResponse
                    {
                        Estudiante = null,
                        Mensaje = "Error guardando estudiante",
                        Codigo = resultadoGuardado.CodigoRespuesta,
                        Errores = resultadoGuardado.Errores
                                   ?? new List<string> { resultadoGuardado.DescripcionRespuesta }
                    });
                }

                var response = new UepexResponse
                {
                    Estudiante = estudiante,
                    Mensaje = "Datos recibidos y validados correctamente",
                    Codigo = "OK"
                };

                _logger.LogInformation("Estudiante {NumeroDocumento} procesado exitosamente", estudiante.NumeroDocumento);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando estudiante");
                return StatusCode(500, new UepexResponse
                {
                    Estudiante = null,
                    Mensaje = "Error interno del servidor",
                    Codigo = "ERROR_INTERNO",
                    Errores = new List<string> { "Error interno del servidor" }
                });
            }
        }

        /// <summary>
        /// Obtiene todos los estudiantes guardados.
        /// Requiere un JWT válido (cualquier rol).
        /// </summary>
        /// <returns>Lista de todos los estudiantes</returns>
        /// <response code="200">Lista de estudiantes obtenida exitosamente</response>
        /// <response code="401">No autenticado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("estudiantes")]
        [Authorize] // <-- sólo usuarios logueados
        [ProducesResponseType(typeof(List<Estudiante>), 200)]
        [ProducesResponseType(typeof(UepexResponse), 500)]
        public async Task<IActionResult> GetEstudiantes()
        {
            try
            {
                _logger.LogInformation("Solicitando lista de estudiantes");

                var estudiantes = await _estudianteService.ObtenerTodosLosEstudiantesAsync();

                _logger.LogInformation("Se retornaron {Count} estudiantes", estudiantes.Count);
                return Ok(estudiantes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo lista de estudiantes");
                return StatusCode(500, new UepexResponse
                {
                    Estudiante = null,
                    Mensaje = "Error interno del servidor",
                    Codigo = "ERROR_INTERNO",
                    Errores = new List<string> { "Error interno del servidor" }
                });
            }
        }

        /// <summary>
        /// Obtiene un estudiante específico por su número de documento.
        /// Requiere un JWT válido (cualquier rol).
        /// </summary>
        /// <param name="numeroDocumento">Número de documento del estudiante</param>
        /// <returns>Datos del estudiante</returns>
        /// <response code="200">Estudiante encontrado</response>
        /// <response code="404">Estudiante no encontrado</response>
        /// <response code="401">No autenticado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("estudiantes/{numeroDocumento}")]
        [Authorize]
        [ProducesResponseType(typeof(Estudiante), 200)]
        [ProducesResponseType(typeof(UepexResponse), 404)]
        [ProducesResponseType(typeof(UepexResponse), 500)]
        public async Task<IActionResult> GetEstudiante(string numeroDocumento)
        {
            try
            {
                _logger.LogInformation("Buscando estudiante con documento {NumeroDocumento}", numeroDocumento);

                var estudiante = await _estudianteService.ObtenerEstudiantePorDocumentoAsync(numeroDocumento);

                if (estudiante == null)
                {
                    _logger.LogWarning("Estudiante con documento {NumeroDocumento} no encontrado", numeroDocumento);
                    return NotFound(new UepexResponse
                    {
                        Estudiante = null,
                        Mensaje = "Estudiante no encontrado",
                        Codigo = "ESTUDIANTE_NO_ENCONTRADO",
                        Errores = new List<string> { $"No se encontró estudiante con documento {numeroDocumento}" }
                    });
                }

                _logger.LogInformation("Estudiante {NumeroDocumento} encontrado", numeroDocumento);
                return Ok(estudiante);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buscando estudiante {NumeroDocumento}", numeroDocumento);
                return StatusCode(500, new UepexResponse
                {
                    Estudiante = null,
                    Mensaje = "Error interno del servidor",
                    Codigo = "ERROR_INTERNO",
                    Errores = new List<string> { "Error interno del servidor" }
                });
            }
        }

        /// <summary>
        /// Exporta todos los estudiantes a CSV.
        /// Requiere un JWT válido.
        /// </summary>
        /// <returns>Archivo CSV con todos los estudiantes</returns>
        /// <response code="200">Archivo CSV generado exitosamente</response>
        /// <response code="401">No autenticado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("estudiantes/exportar-csv")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(UepexResponse), 500)]
        public async Task<IActionResult> ExportarEstudiantesCsv()
        {
            try
            {
                _logger.LogInformation("Solicitando exportación CSV de estudiantes");

                var csvBytes = await _estudianteService.ExportarEstudiantesACsvAsync();

                var fileName = $"estudiantes_uepex_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";

                _logger.LogInformation(
                    "Archivo CSV generado exitosamente: {FileName}, {Size} bytes",
                    fileName,
                    csvBytes.Length
                );

                return File(
                    csvBytes,
                    "text/csv; charset=utf-8",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando archivo CSV");
                return StatusCode(500, new UepexResponse
                {
                    Estudiante = null,
                    Mensaje = "Error interno del servidor",
                    Codigo = "ERROR_INTERNO",
                    Errores = new List<string> { "Error generando archivo CSV" }
                });
            }
        }

        /// <summary>
        /// Exporta todos los estudiantes a Excel.
        /// Requiere un JWT válido.
        /// </summary>
        /// <returns>Archivo Excel con todos los estudiantes</returns>
        /// <response code="200">Archivo Excel generado exitosamente</response>
        /// <response code="401">No autenticado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("estudiantes/exportar-excel")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(UepexResponse), 500)]
        public async Task<IActionResult> ExportarEstudiantesExcel()
        {
            try
            {
                _logger.LogInformation("Solicitando exportación Excel de estudiantes");

                var excelBytes = await _estudianteService.ExportarEstudiantesAExcelAsync();

                var fileName = $"estudiantes_uepex_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx";

                _logger.LogInformation(
                    "Archivo Excel generado exitosamente: {FileName}, {Size} bytes",
                    fileName,
                    excelBytes.Length
                );

                return File(
                    excelBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando archivo Excel");
                return StatusCode(500, new UepexResponse
                {
                    Estudiante = null,
                    Mensaje = "Error interno del servidor",
                    Codigo = "ERROR_INTERNO",
                    Errores = new List<string> { "Error generando archivo Excel" }
                });
            }
        }
    }
}
