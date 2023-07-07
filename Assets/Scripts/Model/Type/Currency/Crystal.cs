using System;
using UnityEngine;

[Serializable]
public class Crystal : Currency
{
    public const long ConversionRate = 15;
    [SerializeField]
    private readonly string id = "Crystal";

    private Crystal(long value) : base(value)
    {
    }

    public static string ID
    {
        get
        {
            return "Crystal";
        }
    }

    public static Crystal ValueOf(long value)
    {
        return new Crystal(value);
    }

    public Gold ToGold()
    {
        long goldValue = this.Value * ConversionRate;
        return Gold.ValueOf(goldValue);
    }

    public Crystal Multiply(int multiplier)
    {
        return ValueOf(this.Value * multiplier);
    }

    public override string GetID()
    {
        return (this.id ?? string.Empty).ToString();
    }
}