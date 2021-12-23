using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ATI.Services.Serialization
{
    public sealed class NewtonsoftJsonSerializer : ISerializer
    {
        public static readonly NewtonsoftJsonSerializer CamelCase = new NewtonsoftJsonSerializer();

        public static readonly NewtonsoftJsonSerializer SnakeCase = new NewtonsoftJsonSerializer(
            new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true,
                    OverrideSpecifiedNames = true
                }
            });

        private readonly JsonSerializer _serializer;

        public NewtonsoftJsonSerializer() : this(new DefaultContractResolver())
        {
        }

        public NewtonsoftJsonSerializer(IContractResolver contractResolver)
        {
            _serializer = new JsonSerializer { CheckAdditionalContent = false, ContractResolver = contractResolver };
        }

        public byte[] Serialize<T>(T obj)
        {
            using var stream = new MemoryStream();
            using var streamWriter = new StreamWriter(stream);
            using var jsonWriter = new JsonTextWriter(streamWriter);

            _serializer.Serialize(jsonWriter, obj);
            streamWriter.Flush();
            return stream.ToArray();
        }

        public string SerializeToUtf8String<T>(T value)
        {
            using var textWriter = new StringWriter();
            _serializer.Serialize(textWriter, value);
            return textWriter.ToString();
        }

        public T Deserialize<T>(string serialized)
        {
            var bytes = Encoding.UTF8.GetBytes(serialized);
            return Deserialize<T>(bytes);
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using var jsonStream = new MemoryStream(bytes, false);
            return Deserialize<T>(jsonStream);
        }

        public ValueTask<T> DeserializeAsync<T>(Stream stream)
        {
            return new ValueTask<T>(Deserialize<T>(stream));
        }

        public T Deserialize<T>(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using JsonReader jsonReader = new JsonTextReader(streamReader);
            return _serializer.Deserialize<T>(jsonReader);
        }
    }
}