using System;
using UnityEngine;

[Serializable]
public class Gold : Currency 
{
    [SerializeField]
    private readonly string id = "Gold";

    private Gold(long value) : base(value)
    {
    }

    public static string ID
    {
        get
        {
            return "Gold";
        }
    }

    public static Gold ValueOf(long value)
    {
        return new Gold(value);
    }

    public Crystal ToCrystal()
    {
        long crystalValue = Mathf.CeilToInt(this.Value / Crystal.ConversionRate);
        if (crystalValue < 1)
        {
            crystalValue = 1;
        }

        return Crystal.ValueOf(crystalValue);
    }

    public Gold Multiply(int multiplier)
    {
        return ValueOf(this.Value * multiplier);
    }

    public override string GetID()
    {
        return (this.id ?? string.Empty).ToString();
    }
}
