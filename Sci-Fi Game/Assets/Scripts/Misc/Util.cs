using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public static T GetRandom<T> (this List<T> list)
    {
        return list[Random.Range ( 0, list.Count )];
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
