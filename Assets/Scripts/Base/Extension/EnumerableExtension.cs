using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtension
{
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.ElementAtOrDefault(UnityEngine.Random.Range(0, source.Count()));
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static T PickRandom<T>(this IEnumerable<T> source, Predicate<T> predicate)
    {
        return source.ToList().FindAll(predicate).PickRandom();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count, Predicate<T> predicate)
    {
        return source.ToList().FindAll(predicate).PickRandom(count);
    }
}