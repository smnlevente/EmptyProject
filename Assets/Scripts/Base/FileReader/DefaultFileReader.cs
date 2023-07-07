namespace FileReader
{
    using System.IO;
    using UnityEngine;

    public class DefaultFileReader : IFileReader
    {
        private static string persistantPath;
        private static string streamingPath;

        public DefaultFileReader()
        {
            streamingPath = Application.streamingAssetsPath;
            persistantPath = Application.persistentDataPath;
        }

        private static string PersistantPath
        {
            get
            {
                if (string.IsNullOrEmpty(persistantPath))
                {
                    persistantPath = Application.persistentDataPath;
                }

                return persistantPath;
            }
        }

        private static string StreamingPath
        {
            get
            {
                if (string.IsNullOrEmpty(streamingPath))
                {
                    streamingPath = Application.streamingAssetsPath;
                }

                return streamingPath;
            }
        }

        public virtual string GetPersistentDataPath()
        {
            return PersistantPath;
        }

        public virtual string GetStreamingAssetsPath()
        {
            return StreamingPath;
        }

        public virtual string ReadFromPersistentData(string fileName)
        {
            string filePath = Path.Combine(PersistantPath, fileName);
            return File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
        }

        public virtual string ReadFromStreamingAssets(string fileName)
        {
            string filePath = Path.Combine(StreamingPath, fileName);
            return File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;
        }

        public virtual string ReadFile(string fileName)
        {
            string result = this.ReadFromPersistentData(fileName);
            return string.IsNullOrEmpty(result) ? this.ReadFromStreamingAssets(fileName) : result;
        }
    }
}