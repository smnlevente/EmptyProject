using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Items : Storage<StringStack, string>
{
    public Items() : base()
    {
    }

    public List<StringStack> GetAllItems()
    {
        return this.Stacks;
    }

    public bool CheckPut(string item)
    {
        return this.CheckPut(item, 1);
    }

    public bool CheckPut(string item, long amount)
    {
        StringStack stack = StringStack.NewInstance(item, amount);
        return this.CheckPut(stack);
    }

    public bool CheckPut(StringStack itemStack)
    {
        if (this.CheckIfCanPut(itemStack))
        {
            this.Put(itemStack);
            return true;
        }

        return false;
    }

    public bool CheckIfCanPut(string item, long amount)
    {
        return true;
    }

    public bool CheckIfCanPut(StringStack itemStack)
    {
        return true;
    }
}
