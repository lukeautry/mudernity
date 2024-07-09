using System.Text.Json;

namespace Agent.Common
{
    public class Deserializer
    {
        public static T? Deserialize<T>(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(json, Constants.SerializerOptions);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}