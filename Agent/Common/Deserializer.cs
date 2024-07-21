using System.Text.Json;

namespace Agent.Common
{
    public class Deserializer
    {
        public static T? Deserialize<T>(string json)
        {
            try
            {
                var options = Constants.SerializerOptions;
                return (T?)JsonSerializer.Deserialize(json, typeof(T), options);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
