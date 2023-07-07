namespace FileReader
{
    public interface IFileReader
    {
        string GetPersistentDataPath();

        string GetStreamingAssetsPath();

        string ReadFromPersistentData(string fileName);

        string ReadFromStreamingAssets(string fileName);

        string ReadFile(string fileName);
    }
}