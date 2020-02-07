using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static T GetRandom<T> (this List<T> list)
    {
        return list[Random.Range ( 0, list.Count )];
    }

    public static T GetRandom<T> (this List<T> list, T exclude)
    {
        if (list.Count == 1)
            return GetRandom ( list );

        if (!list.Contains ( exclude ))
            return GetRandom ( list );

        int indexOf = list.IndexOf ( exclude );
        int randomIndex = indexOf;

        while(randomIndex == indexOf)
        {
            randomIndex = Random.Range ( 0, list.Count );
        }

        return list[randomIndex];
    }

    public static int GetRandomIndex<T> (this List<T> list)
    {
        return Random.Range ( 0, list.Count );
    }

    public static int GetRandomIndex<T> (this List<T> list, int dodge)
    {
        int x = Random.Range ( 0, list.Count );

        while (x == dodge)
        {
            x = Random.Range ( 0, list.Count );
        }

        return x;
    }
}
