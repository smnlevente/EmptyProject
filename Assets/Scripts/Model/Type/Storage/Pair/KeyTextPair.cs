using System;
using UnityEngine;

[Serializable]
public class KeyTextPair : Pair<TextAsset>
{
    public KeyTextPair() : base()
    {
    }

    public KeyTextPair(string id, TextAsset value) : base(id, value)
    {
    }
}
