using System;
using MVC;
using UnityEngine;

[Serializable]
public class Profile : Model
{
    [SerializeField]
    private Inventory inventory = new Inventory();
    [SerializeField]
    private long saveCreateTime = 0;
    [SerializeField]
    private long lastSaveTime = 0;
    private bool initialized = false;

    public Profile()
    {
    }

    public Inventory Inventory
    {
        get
        {
            return this.inventory;
        }
    }

    public DateTime SaveCreateTime
    {
        get
        {
            long now = this.saveCreateTime;
            if (this.saveCreateTime == 0)
            {
                this.saveCreateTime = DateTime.Now.ToUnixTimeMilliseconds();
            }

            return now.FromUnixTimeMilliseconds();
        }
    }

    public long LastSave
    {
        get
        {
            return this.lastSaveTime;
        }
    }

    public void Initialize()
    {
        if (this.initialized)
        {
            return;
        }

        this.initialized = true;
    }

    public void SetLastSave()
    {
        this.lastSaveTime = DateTime.Now.ToUnixTimeMilliseconds();
    }

    public override string GetID()
    {
        return string.Empty;
    }
}
