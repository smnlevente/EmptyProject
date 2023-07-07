using ObjectMapper;
using UnityEngine;

public abstract class EditorJsonRepository<T> : Repository<T, KeyStringPair>
    where T : MonoBehaviour
{
#if UNITY_EDITOR
    public void Fill()
    {
        this.RemoveAll();
        this.Deserialize();
    }
#endif

    protected abstract string JsonFile();

    protected override ISerializer<RepositoryDto<KeyStringPair>> Serializer()
    {
        return new JsonSerializer<RepositoryDto<KeyStringPair>>.Builder(this.JsonFile()).BuildWithoutSerializer();
    }
}
