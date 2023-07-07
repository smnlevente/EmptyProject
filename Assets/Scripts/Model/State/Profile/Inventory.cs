using System;
using MVC;
using UnityEngine;

[Serializable]
public class Inventory : Model
{
    [SerializeField]
    private Items items = new Items();
    [SerializeField]
    private Currencies currencies = new Currencies();

    public Inventory()
    {
    }

    public Items Item
    {
        get
        {
            return this.items;
        }
    }

    public Currencies Currency
    {
        get
        {
            return this.currencies;
        }
    }

    public override string GetID()
    {
        return this.items.GetID() + "_" + this.currencies.GetID();
    }
}
