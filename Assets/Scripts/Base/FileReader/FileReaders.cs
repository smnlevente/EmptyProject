using FileReader;
using UnityEngine;

public static class FileReaders
{
    private static readonly DefaultFileReader DefaultReader = new DefaultFileReader();
    private static readonly IOSFileReader IOSReader = new IOSFileReader();
    private static readonly AndroidFileReader AndroidReader = new AndroidFileReader();

    private static readonly DictionaryWithDefault<RuntimePlatform, IFileReader> Readers = 
        new DictionaryWithDefault<RuntimePlatform, IFileReader>(DefaultReader) 
        {
            { RuntimePlatform.IPhonePlayer, IOSReader },
            { RuntimePlatform.Android, AndroidReader }
        };

    public static IFileReader Get
    {
        get
        {
            return Readers[Application.platform];
        }
    }
}