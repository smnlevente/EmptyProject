using System.IO;
using FileReader;
using MVC;
using ObjectMapper;
using UnityEngine;

public abstract class XmlResource<T, M> : Repository<T, M> where T : MonoBehaviour where M : Model
{
    protected abstract string XmlFile();

    protected abstract string XmlRoot();

    protected abstract string XmlElement();

    protected override ISerializer<RepositoryDto<M>> Serializer()
    {
        IFileReader reader = FileReaders.Get;
        string data = reader.ReadFromPersistentData(this.XmlFile());
        string path = string.IsNullOrEmpty(data) ? Path.Combine(reader.GetStreamingAssetsPath(), this.XmlFile()) : Path.Combine(reader.GetStreamingAssetsPath(), this.XmlFile());
        return new XmlSerializer<RepositoryDto<M>>.Builder(path)
            .XmlRoot(this.XmlRoot())
            .XmlElement("Repository", this.XmlElement())
            .BuildWithoutSerializer();
    }
}
