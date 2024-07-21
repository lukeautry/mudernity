using System.Text.Json;

namespace Agent.Common
{
    public class Serializer
    {
        public static string Serialize<T>(T obj)
        {
            var options = Constants.SerializerOptions;
            return JsonSerializer.Serialize(obj, typeof(T), options);
        }
    }
}