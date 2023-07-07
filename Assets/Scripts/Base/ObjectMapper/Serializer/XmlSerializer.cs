namespace ObjectMapper
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    public class XmlSerializer<T> : ISerializer<T>
    {
        private string path;
        private XmlSerializer serializer;
        private Action<T> serializerImpl;
        private Func<T> deserializerImpl;

        private XmlSerializer(string path, XmlSerializer serializer)
        {
            this.path = path;
            this.serializer = serializer;
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

        private static XmlSerializer<T> Both(string path, XmlSerializer serializer)
        {
            XmlSerializer<T> instance = new XmlSerializer<T>(path, serializer);
            return instance;
        }

        private static XmlSerializer<T> OnlySerializator(string path, XmlSerializer serializer)
        {
            XmlSerializer<T> instance = new XmlSerializer<T>(path, serializer);
            instance.deserializerImpl = null;
            return instance;
        }

        private static XmlSerializer<T> OnlyDeserializator(string path, XmlSerializer serializer)
        {
            XmlSerializer<T> instance = new XmlSerializer<T>(path, serializer);
            instance.serializerImpl = null;
            return instance;
        }

        private void SerializeImpl(T obj)
        {
            using (Stream writer = new FileStream(this.path, FileMode.Create))
            {
                this.serializer.Serialize(writer, obj);
            }
        }

        private T DeserializeImpl()
        {
            if (!File.Exists(this.path))
            {
                return default(T);
            }

            using (Stream reader = new FileStream(this.path, FileMode.Open))
            {
                return (T)this.serializer.Deserialize(reader);
            }
        }

        public class Builder
        {
            private string path;
            private XmlRootAttribute root;
            private XmlAttributeOverrides overrides;

            public Builder(string path)
            {
                this.path = path;
                this.overrides = new XmlAttributeOverrides();
            }

            public Builder XmlRoot(string root)
            {
                this.root = new XmlRootAttribute();
                this.root.ElementName = root;
                return this;
            }

            public Builder XmlElement(string originalName, string newName)
            {
                XmlElementAttribute elementAttribute = new XmlElementAttribute();
                elementAttribute.ElementName = newName;
                XmlAttributes attributes = new XmlAttributes();
                attributes.XmlElements.Add(elementAttribute);
                this.overrides.Add(typeof(T), originalName, attributes);
                return this;
            }

            public XmlSerializer<T> Build()
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), this.overrides, null, this.root, string.Empty);
                return XmlSerializer<T>.Both(this.path, serializer);
            }

            public XmlSerializer<T> BuildWithoutSerializer()
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), this.overrides, null, this.root, string.Empty);
                return XmlSerializer<T>.OnlyDeserializator(this.path, serializer);
            }

            public XmlSerializer<T> BuildWithoutDeserializer()
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T), this.overrides, null, this.root, string.Empty);
                return XmlSerializer<T>.OnlySerializator(this.path, serializer);
            }
        }
    }
}