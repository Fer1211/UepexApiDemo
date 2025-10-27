using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UepexApiDemo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Estudiantes",
                columns: table => new
                {
                    NumeroDocumento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TipoDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreEstudiante = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ApellidosEstudiante = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CodigoCurso = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CondicionEstudiante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Curso = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Regional = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TipoCuenta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Moneda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Banco = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CuentaBancariaEstudiante = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NombreCuentaBancaria = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Asistencia = table.Column<int>(type: "int", nullable: false),
                    Inasistencia = table.Column<int>(type: "int", nullable: false),
                    AsistenciaFechaHoraEntrada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AsistenciaFechaHoraSalida = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudiantes", x => x.NumeroDocumento);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_NumeroDocumento",
                table: "Estudiantes",
                column: "NumeroDocumento",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Estudiantes");
        }
    }
}
