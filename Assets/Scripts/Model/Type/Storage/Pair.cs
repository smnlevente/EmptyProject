using System;
using MVC;
using UnityEngine;

[Serializable]
public class Pair<T> : Model
{
    [SerializeField]
    private string id;
    [SerializeField]
    private T value;

    public Pair() 
    { 
    }

    public Pair(string id, T value)
    {
        this.id = id;
        this.value = value;
    }

    public T Value
    {
        get { return this.value; }
    }

    public override string GetID()
    {
        return (this.id ?? string.Empty).ToString();
    }
}
