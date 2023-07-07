using System;
using System.Collections.Generic;
using System.Linq;
using MVC;
using UnityEngine;

[Serializable]
public abstract class Storage<T1, T2> : Model
    where T1 : Stack<T2, T1>, new()
{
    [SerializeField]
    private List<T1> stacks;

    public Storage()
    {
        this.stacks = new List<T1>();
    }

    public List<T1> Stacks
    {
        get
        {
            return this.stacks;
        }
    }

    public long Count()
    {
        long count = 0;
        foreach (T1 stack in this.stacks)
        {
            count += stack.Amount;
        }

        return count;
    }

    public bool IsUnlocked(T2 model)
    {
        return this.Find(model) != null;
    }

    public long Count(T2 item)
    {
        long count = 0;
        foreach (T1 stack in this.stacks)
        {
            if (stack.Item.Equals(item))
            {
                count += stack.Amount;
            }
        }

        return count;
    }

    public void Put(T2 model)
    {
        this.Put(model, 1);
    }

    public void Put(T2 model, long amount)
    {
        T1 stack = Stack<T2, T1>.NewInstance(model, amount);
        this.Put(stack);
    }

    public void Put(T1 stack)
    {
        if (this.Contains(stack.Item))
        {
            T1 foundStack = this.Find(stack.Item);
            foundStack.Put(stack.Amount);
            return;
        }

        T1 newStack = stack.Clear();
        newStack.Put(stack.Amount);
        this.stacks.Add(newStack);
    }

    public List<T1> Replace(T1 stack)
    {
        List<T1> stacks = this.stacks.ToList();
        stacks.RemoveAll(s => s.Item.Equals(stack.Item));
        stacks.Add(stack);
        return stacks;
    }

    public bool Contains(T2 item)
    {
        T1 stack = this.Find(item);
        return stack != default(T1) && stack.Amount >= 0;
    }

    public T1 Find(T2 item)
    {
        return this.stacks.FirstOrDefault(stack => stack.Item.Equals(item));
    }

    public bool Take(T2 model)
    {
        return this.Take(model, 1);
    }

    public bool Take(T2 model, long amount)
    {
        T1 stack = Stack<T2, T1>.NewInstance(model, amount);
        return this.Take(stack);
    }

    public bool Take(T1 stack)
    {
        T1 bagStack = this.Find(stack.Item);
        if (bagStack != null && bagStack.Has(stack.Amount))
        {
            bagStack.Take(stack.Amount);
            return true;
        }

        return false;
    }

    public void Remove(T2 model)
    {
        T1 stack = this.Find(model);
        this.stacks.Remove(stack);
    }

    public override string GetID()
    {
        return string.Empty;
    }
}
