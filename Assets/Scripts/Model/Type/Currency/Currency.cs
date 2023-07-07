using System;
using MVC;
using UnityEngine;

[Serializable]
public abstract class Currency : Model
{
    [SerializeField]
    private long value;
    private Sprite icon;

    public Currency(long value)
    {
        this.value = value;
    }

    public long Value
    {
        get
        {
            return this.value;
        }
    }

    public Sprite Icon
    {
        get
        {
            if (this.icon == null)
            {
                this.icon = SpriteRepository.Instance.GetSpriteByID(this.GetID());
            }

            return this.icon;
        }
    }
}
