using System.Text.Json;

namespace Agent.Common
{
    public class Serializer
    {
        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, Constants.SerializerOptions);
        }
    }
}