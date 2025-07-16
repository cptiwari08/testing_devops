using System.Text.Json;

namespace EY.CE.Copilot.API.Models
{
    public class Serializer
    {
        public static string SerializeToJson<T>(T @object)
        {
            return System.Text.Json.JsonSerializer.Serialize(@object, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
