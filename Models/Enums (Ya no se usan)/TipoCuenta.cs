using System.Text.Json.Serialization;

namespace UepexApiDemo.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TipoCuenta
	{
		Ahorro = 1,
		Corriente = 2,
		Cheque = 3
	}
}
