using System.IO;
using FileReader;
using MVC;
using ObjectMapper;
using UnityEngine;

public abstract class JsonResource<T, M> : Repository<T, M> where T : MonoBehaviour where M : Model
{
    protected abstract string JsonFile();

    protected override ISerializer<RepositoryDto<M>> Serializer()
    {
        return this.GetSerializer(this.JsonFile());
    }

    protected virtual ISerializer<RepositoryDto<M>> GetSerializer(string path)
    {
        return new JsonSerializer<RepositoryDto<M>>.Builder(path).BuildWithoutSerializer();
    }

    protected override void Deserialize()
    {
        this.RemoveAll();
        base.Deserialize();
    }
}
