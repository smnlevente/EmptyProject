namespace FileReader
{
    using System;
    using System.IO;
    using UnityEngine.Networking;

    public class AndroidFileReader : DefaultFileReader
    {
        public AndroidFileReader() : base()
        {
        }

        public override string ReadFromStreamingAssets(string fileName)
        {
            string filePath = Path.Combine(this.GetStreamingAssetsPath(), fileName);
            UnityWebRequest reader = new UnityWebRequest(filePath);
            while (!reader.isDone)
            {
            }

            return string.IsNullOrEmpty(reader.error) ? reader.downloadHandler.text : String.Empty;
        }
    }
}