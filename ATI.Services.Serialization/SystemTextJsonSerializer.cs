using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ATI.Services.Serialization
{
    public sealed class SystemTextJsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions _options;
        public static readonly ISerializer CamelCase = new SystemTextJsonSerializer(SerializationOptions.CamelCase);
        public static readonly ISerializer SnakeCase = new SystemTextJsonSerializer(SerializationOptions.SnakeCase);

        private SystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        public byte[] Serialize<T>(T value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, _options);
        }

        public string SerializeToUtf8String<T>(T value)
        {
            return JsonSerializer.Serialize(value, _options);
        }

        public T Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes, _options);
        }

        public T Deserialize<T>(string serialized)
        {
            return JsonSerializer.Deserialize<T>(serialized, _options);
        }

        public T Deserialize<T>(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return Deserialize<T>(memoryStream.ToArray());
        }

        public ValueTask<T> DeserializeAsync<T>(Stream stream)
        {
            return JsonSerializer.DeserializeAsync<T>(stream, _options);
        }
    }
}