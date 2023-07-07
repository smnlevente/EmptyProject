using System;
using System.Collections.Generic;
using System.Linq;
using MVC;
using ObjectMapper;
using UnityEngine;

public abstract class Repository<T, M> : Singleton<T> where T : MonoBehaviour where M : Model
{
    [SerializeField]
    private List<M> repository = new List<M>();

    protected List<M> RepositoryData
    {
        get
        {
            return this.repository;
        }
    }

    public M GetFirst()
    {
        return this.repository.FirstOrDefault();
    }

    public M GetLast()
    {
        return this.repository.LastOrDefault();
    }

    public M GetAt(int index)
    {
        return this.repository.ElementAtOrDefault(index);
    }

    public M GetByID(string id)
    {
        return this.repository.FirstOrDefault(item => item.GetID().Equals(id));
    }

    public M[] GetRange(int index, int count)
    {
        return this.repository.Skip(index).Take(count).ToArray();
    }

    public M[] GetAll()
    {
        return this.repository.ToArray();
    }

    public M[] GetAll(Predicate<M> predicate)
    {
        return this.repository.FindAll(predicate).ToArray();
    }

    public bool Exists(M model)
    {
        return this.repository.Contains(model);
    }

    public bool Add(M model)
    {
        if (model != null && !this.Exists(model))
        {
            this.repository.Add(model);
            return true;
        }

        return false;
    }

    public bool Remove(M model)
    {
        return this.repository.Remove(model);
    }

    public bool RemoveByID(string id)
    {
        return this.Remove(this.GetByID(id));
    }

    public void RemoveAll()
    {
        this.repository.Clear();
    }

    public void Reset()
    {
        this.RemoveAll();
        this.repository = this.Initialize();
        this.Serialize();
    }

    public void Save()
    {
        this.Serialize();
    }

    public virtual void Reload()
    {
    }

    protected virtual ISerializer<RepositoryDto<M>> Serializer()
    {
        return NullSerializer<RepositoryDto<M>>.Instance;
    }

    protected virtual List<M> Initialize()
    {
        return new List<M>();
    }

    protected virtual void Deserialize()
    {
        RepositoryDto<M> result = this.Serializer().Deserialize();
        this.repository.AddRange((result == null) ? this.Initialize() : result.Repository);
    }

    protected virtual void Serialize()
    {
        RepositoryDto<M> model = new RepositoryDto<M>(this.repository);
        this.Serializer().Serialize(model);
    }

    private void OnEnable()
    {
        this.Deserialize();
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        this.Serialize();
#endif
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            this.Serialize();
        }
    }
}
