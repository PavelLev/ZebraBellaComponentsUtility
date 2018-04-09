using System.IO;
using Newtonsoft.Json;

namespace ZebraBellaComponentsUtility.Utility.Extensions
{
    public static class JsonSerializerExtension
    {
        public static void SerializeToFile(this JsonSerializer serializer, string filePath, object value)
        {
            using (var streamWriter = new StreamWriter(filePath))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(jsonWriter, value);
            }
        }

        public static T DeserializeFromFile<T>(this JsonSerializer serializer, string filePath)
        {
            using (var streamReader = new StreamReader(filePath))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        public static T DeserializeFromString<T>(this JsonSerializer serializer, string value)
        {
            using (var streamReader = new StringReader(value))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        public static T DeserializeFromStream<T>(this JsonSerializer serializer, Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                return serializer.Deserialize<T>(jsonReader);
            }
        }

        public static T Deserialize<T>(this JsonSerializer serializer, string value) =>
            serializer.Deserialize<T>(new JsonTextReader(new StringReader(value)));
    }
}
