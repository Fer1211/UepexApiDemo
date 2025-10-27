using System.Text.Json.Serialization;

namespace UepexApiDemo.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum CondicionEstudiante
	{
		Activo = 1,
		Inactivo = 2,
		Discapacitado = 3,
		Graduado = 4
	}
}
