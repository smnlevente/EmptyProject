using System;

[Serializable]
public class KeyRangePair : Pair<Range>
{
    public KeyRangePair() : base()
    {
    }

    public KeyRangePair(string id, Range value) : base(id, value)
    {
    }
}
