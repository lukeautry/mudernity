using System.Text.Json;
using System.Text.Json.Serialization;

namespace Agent.Common
{
    public static class Constants
    {
        public static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) },
            WriteIndented = true
        };
    }
}