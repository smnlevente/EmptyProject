namespace ObjectMapper
{
    using System;
    using System.IO;
    using UnityEngine;

    public class JsonSerializer<T> : ISerializer<T>
    {
        private string path;
        private Action<T> serializerImpl;
        private Func<T> deserializerImpl;

        private JsonSerializer(string path)
        {
            this.path = path;
            this.serializerImpl = this.SerializeImpl;
            this.deserializerImpl = this.DeserializeImpl;
        }

        public void Serialize(T obj)
        {
            if (this.serializerImpl == null)
            {
                return;
            }

            this.serializerImpl(obj);
        }

        public T Deserialize()
        {
            return (this.deserializerImpl == null) ? default(T) : this.deserializerImpl();
        }

        private static JsonSerializer<T> Both(string path)
        {
            JsonSerializer<T> instance = new JsonSerializer<T>(path);
            return instance;
        }

        private static JsonSerializer<T> OnlySerializator(string path)
        {
            JsonSerializer<T> instance = new JsonSerializer<T>(path);
            instance.deserializerImpl = null;
            return instance;
        }

        private static JsonSerializer<T> OnlyDeserializator(string path)
        {
            JsonSerializer<T> instance = new JsonSerializer<T>(path);
            instance.serializerImpl = null;
            return instance;
        }

        private void SerializeImpl(T obj)
        {
            string filePath = Path.Combine(FileReaders.Get.GetStreamingAssetsPath(), this.path);
            File.WriteAllText(filePath, JsonUtility.ToJson(obj, Debug.isDebugBuild));
        }

        private T DeserializeImpl()
        {
            string json = FileReaders.Get.ReadFile(this.path);
            return string.IsNullOrEmpty(json) ? default(T) : JsonUtility.FromJson<T>(json);
        }

        public class Builder
        {
            private string path;

            public Builder(string path)
            {
                this.path = path;
            }

            public JsonSerializer<T> Build()
            {
                return JsonSerializer<T>.Both(this.path);
            }

            public JsonSerializer<T> BuildWithoutSerializer()
            {
                return JsonSerializer<T>.OnlyDeserializator(this.path);
            }

            public JsonSerializer<T> BuildWithoutDeserializer()
            {
                return JsonSerializer<T>.OnlySerializator(this.path);
            }
        }
    }
}