using System;
using UnityEngine;

[Serializable]
public class Experience : Currency
{
    [SerializeField]
    private readonly string id = "Experience";

    private Experience(long value) : base(value)
    {
    }

    public static string ID
    {
        get
        {
            return "Experience";
        }
    }

    public static Experience ValueOf(long value)
    {
        return new Experience(value);
    }

    public Experience Multiply(int multiplier)
    {
        return ValueOf(this.Value * multiplier);
    }

    public override string GetID()
    {
        return (this.id ?? string.Empty).ToString();
    }
}
