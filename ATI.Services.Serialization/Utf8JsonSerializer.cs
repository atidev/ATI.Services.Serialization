using System.IO;
using System.Threading.Tasks;
using Utf8Json;
using Utf8Json.Formatters;
using Utf8Json.Resolvers;

namespace ATI.Services.Serialization
{
    public sealed class Utf8JsonSerializer : ISerializer
    {
        public static readonly Utf8JsonSerializer UpperCamelCase = new Utf8JsonSerializer(StandardResolver.Default);
        public static readonly Utf8JsonSerializer LowerCamelCase = new Utf8JsonSerializer(StandardResolver.CamelCase);
        public static readonly Utf8JsonSerializer SnakeCase = new Utf8JsonSerializer(StandardResolver.SnakeCase);

        private readonly IJsonFormatterResolver _jsonFormatterResolver;

        public Utf8JsonSerializer(IJsonFormatterResolver jsonFormatterResolver)
        {
            var jsonFormatterResolvers = new[] { jsonFormatterResolver };
            var jsonFormatters = new IJsonFormatter[]
            {
                // Форматторы Utf8Json по умолчанию используют формат ISO8601, а нам нужен другой
                new TimeSpanFormatter(),
                new NullableTimeSpanFormatter()
            };
            _jsonFormatterResolver = CompositeResolver.Create(jsonFormatters, jsonFormatterResolvers);
        }

        public byte[] Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, _jsonFormatterResolver);
        }

        public string SerializeToUtf8String<T>(T value)
        {
            return JsonSerializer.ToJsonString(value, _jsonFormatterResolver);
        }

        public T Deserialize<T>(byte[] bytes)
        {
            return JsonSerializer.Deserialize<T>(bytes, _jsonFormatterResolver);
        }

        public T Deserialize<T>(string serialized)
        {
            return JsonSerializer.Deserialize<T>(serialized, _jsonFormatterResolver);
        }

        public ValueTask<T> DeserializeAsync<T>(Stream stream)
        {
            return new ValueTask<T>(Deserialize<T>(stream));
        }

        public T Deserialize<T>(Stream stream)
        {
            return JsonSerializer.Deserialize<T>(stream, _jsonFormatterResolver);
        }
    }
}