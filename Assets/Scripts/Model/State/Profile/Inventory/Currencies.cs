using System;
using MVC;
using UnityEngine;

[Serializable]
public class Currencies : Model
{
    [SerializeField]
    private Gold gold = Gold.ValueOf(0);
    [SerializeField]
    private Crystal crystal = Crystal.ValueOf(0);
    [SerializeField]
    private Experience experience = Experience.ValueOf(0);

    public Currencies()
    {
    }

    public Gold Gold
    {
        get
        {
            return this.gold;
        }
    }

    public Crystal Crystal
    {
        get
        {
            return this.crystal;
        }
    }

    public Experience Experience
    {
        get
        {
            return this.experience;
        }
    }

    public void Put(Gold gold)
    {
        this.gold = Gold.ValueOf(this.gold.Value + gold.Value);
    }

    public void Put(Crystal crystal)
    {
        this.crystal = Crystal.ValueOf(this.crystal.Value + crystal.Value);
    }

    public void Put(Experience experience)
    {
        this.experience = Experience.ValueOf(this.experience.Value + experience.Value);
    }

    public bool Take(Gold gold)
    {
        if (this.gold.Value >= gold.Value)
        {
            this.gold = Gold.ValueOf(this.gold.Value - gold.Value);
            return true;
        }

        return false;
    }

    public bool Take(Crystal crystal)
    {
        if (this.crystal.Value >= crystal.Value)
        {
            this.crystal = Crystal.ValueOf(this.crystal.Value - crystal.Value);
            return true;
        }

        return false;
    }

    public bool Take(Experience experience)
    {
        if (this.experience.Value >= experience.Value)
        {
            this.experience = Experience.ValueOf(this.experience.Value - experience.Value);
            return true;
        }

        return false;
    }

    public override string GetID()
    {
        return string.Empty;
    }
}