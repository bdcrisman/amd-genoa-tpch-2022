using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions 
{
    public static bool IsNullOrEmpty<T>(this IList<T> l)
    {
        return l == null || l.Count <= 0;
    }

    public static List<T> Shuffle<T>(this IList<T> l) {
        var rnd = new Random();
        return l.OrderBy(item => rnd.Next()).ToList();
    }

    //public static bool IsNullOrEmpty<T>(this IReadOnlyList<T> l)
    //{
    //    return l == null || l.Count <= 0;
    //}
}
