using System.Text.Json.Serialization;

namespace UepexApiDemo.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum Moneda
	{
		DOP = 1,
		USD = 2,
		EUR = 3
	}
}
