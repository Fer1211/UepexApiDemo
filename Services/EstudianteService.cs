using UepexApiDemo.Models;
using UepexApiDemo.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ClosedXML.Excel;

namespace UepexApiDemo.Services
{
    public class EstudianteService : IEstudianteService
    {
        private readonly IValidacionService _validacionService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EstudianteService> _logger;

        public EstudianteService(IValidacionService validacionService, ApplicationDbContext context, ILogger<EstudianteService> logger)
        {
            _validacionService = validacionService;
            _context = context;
            _logger = logger;
        }

        public Estudiante GetEstudianteEjemplo()
        {
            _logger.LogInformation("Generando estudiante de ejemplo");
            
            return new Estudiante
            {
                TipoDocumento = "Cedula",
                NumeroDocumento = "50231212122",
                NombreEstudiante = "María",
                ApellidosEstudiante = "Pérez Marte",
                CodigoCurso = "01",
                CondicionEstudiante = "Graduado",
                Curso = "técnico informático",
                Regional = "Santo Domingo",
                TipoCuenta = "Ahorro",
                Moneda = "DOP",
                Banco = "Banreservas",
                CuentaBancariaEstudiante = "9602451545",
                NombreCuentaBancaria = "María Pérez",
                Asistencia = 1,
                Inasistencia = 0,
                AsistenciaFechaHoraEntrada = "10/25/2025 2:00 PM",
                AsistenciaFechaHoraSalida = "10/25/2025 4:00 PM"
            };
        }

        public ResultadoValidacion ProcesarEstudiante(Estudiante estudiante)
        {
            try
            {
                _logger.LogInformation("Procesando estudiante {NumeroDocumento}", estudiante.NumeroDocumento);

                // Validar usando el servicio de validación
                var resultadoValidacion = _validacionService.ValidarEstudianteCompleto(estudiante);

                if (!resultadoValidacion.EsValido)
                {
                    _logger.LogWarning("Validación fallida para estudiante {NumeroDocumento}: {Errores}", 
                        estudiante.NumeroDocumento, string.Join(", ", resultadoValidacion.Errores ?? new List<string>()));
                }
                else
                {
                    _logger.LogInformation("Validación exitosa para estudiante {NumeroDocumento}", estudiante.NumeroDocumento);
                }

                return resultadoValidacion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error procesando estudiante {NumeroDocumento}", estudiante.NumeroDocumento);
                return ResultadoValidacion.Error("ERROR_PROCESAMIENTO", "Error interno procesando el estudiante");
            }
        }

        public async Task<ResultadoValidacion> GuardarEstudianteAsync(Estudiante estudiante)
        {
            try
            {
                _logger.LogInformation("Guardando estudiante {NumeroDocumento}", estudiante.NumeroDocumento);

                // Verificar si el estudiante ya existe
                var estudianteExistente = await _context.Estudiantes
                    .FirstOrDefaultAsync(e => e.NumeroDocumento == estudiante.NumeroDocumento);

                if (estudianteExistente != null)
                {
                    _logger.LogWarning("Estudiante con documento {NumeroDocumento} ya existe", estudiante.NumeroDocumento);
                    return ResultadoValidacion.Error("ESTUDIANTE_EXISTENTE", "Ya existe un estudiante con este número de documento");
                }

                // Agregar al contexto
                _context.Estudiantes.Add(estudiante);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Estudiante {NumeroDocumento} guardado exitosamente", estudiante.NumeroDocumento);
                return ResultadoValidacion.Exitoso();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando estudiante {NumeroDocumento}", estudiante.NumeroDocumento);
                return ResultadoValidacion.Error("ERROR_GUARDADO", "Error interno guardando el estudiante");
            }
        }

        public async Task<List<Estudiante>> ObtenerTodosLosEstudiantesAsync()
        {
            try
            {
                _logger.LogInformation("Obteniendo todos los estudiantes");
                
                var estudiantes = await _context.Estudiantes
                    .OrderBy(e => e.NombreEstudiante)
                    .ToListAsync();

                _logger.LogInformation("Se encontraron {Count} estudiantes", estudiantes.Count);
                return estudiantes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estudiantes");
                return new List<Estudiante>();
            }
        }

        public async Task<Estudiante?> ObtenerEstudiantePorDocumentoAsync(string numeroDocumento)
        {
            try
            {
                _logger.LogInformation("Buscando estudiante con documento {NumeroDocumento}", numeroDocumento);
                
                var estudiante = await _context.Estudiantes
                    .FirstOrDefaultAsync(e => e.NumeroDocumento == numeroDocumento);

                if (estudiante == null)
                {
                    _logger.LogWarning("No se encontró estudiante con documento {NumeroDocumento}", numeroDocumento);
                }
                else
                {
                    _logger.LogInformation("Estudiante encontrado: {NombreEstudiante}", estudiante.NombreEstudiante);
                }

                return estudiante;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buscando estudiante con documento {NumeroDocumento}", numeroDocumento);
                return null;
            }
        }

        public async Task<byte[]> ExportarEstudiantesACsvAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando exportación de estudiantes a CSV");

                var estudiantes = await _context.Estudiantes
                    .OrderBy(e => e.NombreEstudiante)
                    .ThenBy(e => e.ApellidosEstudiante)
                    .ToListAsync();

                var csv = new StringBuilder();
                
                // Agregar BOM para UTF-8 (mejor compatibilidad con Excel)
                csv.Append('\uFEFF');
                
                // Encabezados del CSV con nombres más descriptivos y separados por punto y coma
                csv.AppendLine("Tipo Documento;Número Documento;Nombre Completo;Apellidos;Código Curso;Condición;Curso;Regional;Tipo Cuenta;Moneda;Banco;Cuenta Bancaria;Nombre Cuenta;Asistencias;Inasistencias;Fecha Entrada;Fecha Salida");

                // Datos de los estudiantes con formato mejorado
                foreach (var estudiante in estudiantes)
                {
                    var nombreCompleto = $"{estudiante.NombreEstudiante} {estudiante.ApellidosEstudiante}".Trim();
                    
                    csv.AppendLine($"{EscapeCsvField(estudiante.TipoDocumento)};" +
                                 $"{EscapeCsvField(estudiante.NumeroDocumento)};" +
                                 $"{EscapeCsvField(nombreCompleto)};" +
                                 $"{EscapeCsvField(estudiante.ApellidosEstudiante)};" +
                                 $"{EscapeCsvField(estudiante.CodigoCurso)};" +
                                 $"{EscapeCsvField(estudiante.CondicionEstudiante)};" +
                                 $"{EscapeCsvField(estudiante.Curso)};" +
                                 $"{EscapeCsvField(estudiante.Regional)};" +
                                 $"{EscapeCsvField(estudiante.TipoCuenta)};" +
                                 $"{EscapeCsvField(estudiante.Moneda)};" +
                                 $"{EscapeCsvField(estudiante.Banco)};" +
                                 $"{EscapeCsvField(estudiante.CuentaBancariaEstudiante)};" +
                                 $"{EscapeCsvField(estudiante.NombreCuentaBancaria)};" +
                                 $"{estudiante.Asistencia};" +
                                 $"{estudiante.Inasistencia};" +
                                 $"{EscapeCsvField(FormatearFecha(estudiante.AsistenciaFechaHoraEntrada))};" +
                                 $"{EscapeCsvField(FormatearFecha(estudiante.AsistenciaFechaHoraSalida))}");
                }

                var csvBytes = Encoding.UTF8.GetBytes(csv.ToString());
                
                _logger.LogInformation("Exportación CSV completada. {Count} estudiantes exportados, {Size} bytes generados", 
                    estudiantes.Count, csvBytes.Length);

                return csvBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando estudiantes a CSV");
                throw;
            }
        }

        private string EscapeCsvField(string? field)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            // Si el campo contiene punto y coma, comillas o saltos de línea, lo envolvemos en comillas
            if (field.Contains(';') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            {
                // Escapamos las comillas dobles duplicándolas
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }

        private string FormatearFecha(string? fecha)
        {
            if (string.IsNullOrEmpty(fecha))
                return string.Empty;

            try
            {
                // Intentar parsear la fecha si está en formato MM/dd/yyyy h:mm AM/PM
                if (DateTime.TryParseExact(fecha, "M/d/yyyy h:mm tt", null, System.Globalization.DateTimeStyles.None, out var fechaParsed))
                {
                    // Formatear a un formato más legible: dd/MM/yyyy HH:mm
                    return fechaParsed.ToString("dd/MM/yyyy HH:mm");
                }
                
                // Si no se puede parsear, devolver el valor original
                return fecha;
            }
            catch
            {
                // En caso de error, devolver el valor original
                return fecha;
            }
        }

        public async Task<byte[]> ExportarEstudiantesAExcelAsync()
        {
            try
            {
                _logger.LogInformation("Iniciando exportación de estudiantes a Excel");

                var estudiantes = await _context.Estudiantes
                    .OrderBy(e => e.NombreEstudiante)
                    .ThenBy(e => e.ApellidosEstudiante)
                    .ToListAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Estudiantes UEPEX");

                // Configurar encabezados
                var headers = new[]
                {
                    "Tipo Documento", "Número Documento", "Nombre Completo", "Apellidos",
                    "Código Curso", "Condición", "Curso", "Regional", "Tipo Cuenta", "Moneda",
                    "Banco", "Cuenta Bancaria", "Nombre Cuenta", "Asistencias", "Inasistencias",
                    "Fecha Entrada", "Fecha Salida"
                };

                // Agregar encabezados con formato
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }

                // Agregar datos
                for (int i = 0; i < estudiantes.Count; i++)
                {
                    var estudiante = estudiantes[i];
                    var row = i + 2; // Empezar desde la fila 2
                    var nombreCompleto = $"{estudiante.NombreEstudiante} {estudiante.ApellidosEstudiante}".Trim();

                    worksheet.Cell(row, 1).Value = estudiante.TipoDocumento;
                    worksheet.Cell(row, 2).Value = estudiante.NumeroDocumento;
                    worksheet.Cell(row, 3).Value = nombreCompleto;
                    worksheet.Cell(row, 4).Value = estudiante.ApellidosEstudiante;
                    worksheet.Cell(row, 5).Value = estudiante.CodigoCurso;
                    worksheet.Cell(row, 6).Value = estudiante.CondicionEstudiante;
                    worksheet.Cell(row, 7).Value = estudiante.Curso;
                    worksheet.Cell(row, 8).Value = estudiante.Regional;
                    worksheet.Cell(row, 9).Value = estudiante.TipoCuenta;
                    worksheet.Cell(row, 10).Value = estudiante.Moneda;
                    worksheet.Cell(row, 11).Value = estudiante.Banco;
                    worksheet.Cell(row, 12).Value = estudiante.CuentaBancariaEstudiante;
                    worksheet.Cell(row, 13).Value = estudiante.NombreCuentaBancaria;
                    worksheet.Cell(row, 14).Value = estudiante.Asistencia;
                    worksheet.Cell(row, 15).Value = estudiante.Inasistencia;
                    worksheet.Cell(row, 16).Value = FormatearFecha(estudiante.AsistenciaFechaHoraEntrada);
                    worksheet.Cell(row, 17).Value = FormatearFecha(estudiante.AsistenciaFechaHoraSalida);

                    // Aplicar bordes a toda la fila
                    var range = worksheet.Range(row, 1, row, headers.Length);
                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    range.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                // Ajustar ancho de columnas automáticamente
                worksheet.Columns().AdjustToContents();

                // Agregar filtros a los encabezados
                worksheet.Range(1, 1, 1, headers.Length).SetAutoFilter();

                // Agregar información del reporte
                var infoRow = estudiantes.Count + 4;
                worksheet.Cell(infoRow, 1).Value = "Reporte generado el:";
                worksheet.Cell(infoRow, 2).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cell(infoRow + 1, 1).Value = "Total de estudiantes:";
                worksheet.Cell(infoRow + 1, 2).Value = estudiantes.Count;

                // Formatear las celdas de información
                var infoRange = worksheet.Range(infoRow, 1, infoRow + 1, 2);
                infoRange.Style.Font.Bold = true;
                infoRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var excelBytes = stream.ToArray();

                _logger.LogInformation("Exportación Excel completada. {Count} estudiantes exportados, {Size} bytes generados", 
                    estudiantes.Count, excelBytes.Length);

                return excelBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exportando estudiantes a Excel");
                throw;
            }
        }
    }
}
