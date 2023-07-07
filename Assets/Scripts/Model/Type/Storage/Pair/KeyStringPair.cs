using System;

[Serializable]
public class KeyStringPair : Pair<string>
{
    public KeyStringPair() : base()
    {
    }

    public KeyStringPair(string id, string value) : base(id, value)
    {
    }
}
