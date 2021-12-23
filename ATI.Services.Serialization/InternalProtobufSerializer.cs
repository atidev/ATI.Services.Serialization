using System.IO;
using ProtoBuf.Meta;

namespace ATI.Services.Serialization
{
    internal static class InternalProtobufSerializer<T>
    {
        /// <summary>
        /// Для каждого типа T будет вызываться свой статический конструктор РОВНО ОДИН раз.
        /// Поэтому тут добавляем наш тип в общую RuntimeTypeModel.Default (она статическая) и компилируем.
        /// </summary>
        static InternalProtobufSerializer()
        {
            var model = RuntimeTypeModel.Default;
            lock (model)
            {
                model.Add(typeof(T), true);
                model.CompileInPlace();
            }
        }

        public static byte[] Serialize(T value)
        {
            using var memory = new MemoryStream();
            RuntimeTypeModel.Default.Serialize(memory, value);
            return memory.ToArray();
        }

        public static T Deserialize(byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            return Deserialize(memoryStream);
        }

        public static T Deserialize(Stream stream)
        {
            return (T)RuntimeTypeModel.Default.Deserialize(stream, null, typeof(T));
        }
    }
}