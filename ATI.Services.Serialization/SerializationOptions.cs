using System.Text.Json;

namespace ATI.Services.Serialization
{
    internal static class SerializationOptions
    {
        public static JsonSerializerOptions SnakeCase = new JsonSerializerOptions
        {
            IgnoreNullValues = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy()
        };

        public static JsonSerializerOptions CamelCase = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }
}