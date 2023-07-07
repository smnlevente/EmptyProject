namespace ObjectMapper
{
    public sealed class NullSerializer<T> : ISerializer<T>
    {
        private static readonly NullSerializer<T> Shared = new NullSerializer<T>();

        private NullSerializer()
        {
        }
 
        public static NullSerializer<T> Instance
        {
            get
            {
                return Shared;
            }
        }

        public void Serialize(T obj)
        {
        }

        public T Deserialize()
        {
            return default(T);
        }
    }
}