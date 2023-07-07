using System;
using UnityEngine;

[Serializable]
public class Stack<T1, T2> 
    where T2 : Stack<T1, T2>, new()
{
    private string id;
    [SerializeField]
    private T1 item;
    [SerializeField]
    private long amount;

    public Stack()
    {
    }

    public T1 Item
    {
        get
        {
            return this.item;
        }
    }

    public long Amount
    {
        get
        {
            return this.amount;
        }
    }

    private string Id
    {
        get
        {
            return this.id == null ? this.id = Guid.NewGuid().ToString() : this.id;
        }
    }

    public static T2 NewInstance(T1 item)
    {
        return NewInstance(item, 0);
    }

    public static T2 NewInstance(T1 item, long amount)
    {
        T2 instance = new T2();
        instance.item = item;
        instance.amount = (amount < 0) ? 0 : amount;
        return instance;
    }

    public T2 Clone()
    {
        return NewInstance(this.item, this.amount);
    }

    public T2 Clear()
    {
        return NewInstance(this.item);
    }

    public void Put(long amount)
    {
        this.amount += amount;
    }

    public void Take(long amount)
    {
        this.Put(-amount);
    }

    public bool Has(long amount)
    {
        return this.amount >= amount;
    }
}
