using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ATI.Services.Serialization
{
    public sealed class ProtobufSerializer : ISerializer
    {
        private static readonly Lazy<ProtobufSerializer> Instance = new Lazy<ProtobufSerializer>();
        public static ProtobufSerializer Default => Instance.Value;

        public byte[] Serialize<T>(T value)
        {
            return InternalProtobufSerializer<T>.Serialize(value);
        }

        public string SerializeToUtf8String<T>(T value)
        {
            return Encoding.UTF8.GetString(Serialize(value));
        }

        public T Deserialize<T>(byte[] bytes)
        {
            return InternalProtobufSerializer<T>.Deserialize(bytes);
        }

        public T Deserialize<T>(string serialized)
        {
            var bytes = Encoding.UTF8.GetBytes(serialized);
            return Deserialize<T>(bytes);
        }

        public T Deserialize<T>(Stream stream)
        {
            return InternalProtobufSerializer<T>.Deserialize(stream);
        }

        public ValueTask<T> DeserializeAsync<T>(Stream stream)
        {
            return new ValueTask<T>(Deserialize<T>(stream));
        }
    }
}