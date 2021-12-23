using System.IO;
using System.Threading.Tasks;

namespace ATI.Services.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T value);
        string SerializeToUtf8String<T>(T value);
        T Deserialize<T>(byte[] bytes);
        T Deserialize<T>(string serialized);
        T Deserialize<T>(Stream stream);
        ValueTask<T> DeserializeAsync<T>(Stream stream);
    }
}