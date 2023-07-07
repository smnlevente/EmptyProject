using System;
using UnityEngine;

[Serializable]
public class Range
{
    [SerializeField]
    private float min;
    [SerializeField]
    private float max;

    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    public Range(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public float Min
    {
        get
        {
            return this.min;
        }
    }

    public float Max
    {
        get
        {
            return this.max;
        }
    }

    public int MinInt
    {
        get
        {
            return (int)this.min;
        }
    }

    public int MaxInt
    {
        get
        {
            return (int)this.max;
        }
    }

    public bool IsInRange(int y)
    {
        return y >= this.MinInt && y <= this.MaxInt;
    }

    public bool IsInRange(float y)
    {
        return y >= this.min && y <= this.max;
    }

    public bool IsInRange(Vector2 position)
    {
        return this.IsInRange(position.y);
    }

    public bool IsInRange(Vector3 position)
    {
        return this.IsInRange(position.y);
    }

    public float Clamp(float y)
    {
        return Mathf.Clamp(y, this.min, this.max);
    }

    public int ClampInt(int y)
    {
        return Mathf.Clamp(y, this.MinInt, this.MaxInt);
    }

    public Vector2 Clamp(Vector2 position)
    {
        float y = this.Clamp(position.y);
        return new Vector2(position.x, y);
    }

    public Vector3 Clamp(Vector3 position)
    {
        float y = this.Clamp(position.y);
        return new Vector3(position.x, y, position.z);
    }

    public float RandomRange()
    {
        return UnityEngine.Random.Range(this.Min, this.Max);
    }

    public int RandomRangeInt()
    {
        return UnityEngine.Random.Range(this.MinInt, this.MaxInt + 1);
    }
}
