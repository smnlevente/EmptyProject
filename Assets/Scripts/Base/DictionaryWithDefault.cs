using System.Collections.Generic;

public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
{
    private readonly TValue defaultValue;

    public DictionaryWithDefault(TValue defaultValue)
    {
        this.defaultValue = defaultValue;
    }
    
    public new TValue this[TKey key]
    {
        get
        {
            TValue t;
            return this.TryGetValue(key, out t) ? t : this.defaultValue;
        }

        set 
        { 
            base[key] = value; 
        }
    }
}