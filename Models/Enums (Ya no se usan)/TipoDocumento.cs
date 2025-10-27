using System.Text.Json.Serialization;

namespace UepexApiDemo.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TipoDocumento
	{
		Cedula = 1,
		Pasaporte = 2,
		RNC = 3
	}
}
