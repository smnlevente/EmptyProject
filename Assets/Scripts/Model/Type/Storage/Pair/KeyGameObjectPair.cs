using System;
using UnityEngine;

[Serializable]
public class KeyGameObjectPair : Pair<GameObject>
{
    public KeyGameObjectPair()
        : base()
    {
    }

    public KeyGameObjectPair(string key, GameObject value)
        : base(key, value)
    {
    }
}
