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

    // Convert the string to Pascal case.
    public static string ToPascalCase (this string the_string)
    {
        // If there are 0 or 1 characters, just return the string.
        if (the_string == null) return the_string;
        if (the_string.Length < 2) return the_string.ToUpper ();

        // Split the string into words.
        string[] words = the_string.Split (
            new char[] { },
            System.StringSplitOptions.RemoveEmptyEntries );

        // Combine the words.
        string result = "";
        foreach (string word in words)
        {
            result +=
                word.Substring ( 0, 1 ).ToUpper () +
                word.Substring ( 1 );
        }

        return result;
    }

    // Convert the string to camel case.
    public static string ToCamelCase (this string the_string)
    {
        // If there are 0 or 1 characters, just return the string.
        if (the_string == null || the_string.Length < 2)
            return the_string;

        // Split the string into words.
        string[] words = the_string.Split (
            new char[] { },
            System.StringSplitOptions.RemoveEmptyEntries );

        // Combine the words.
        string result = words[0].ToLower ();
        for (int i = 1; i < words.Length; i++)
        {
            result +=
                words[i].Substring ( 0, 1 ).ToUpper () +
                words[i].Substring ( 1 );
        }

        return result;
    }

    // Capitalize the first character and add a space before
    // each capitalized letter (except the first character).
    public static string ToProperCase (this string the_string)
    {
        // If there are 0 or 1 characters, just return the string.
        if (the_string == null) return the_string;
        if (the_string.Length < 2) return the_string.ToUpper ();

        // Start with the first character.
        string result = the_string.Substring ( 0, 1 ).ToUpper ();

        // Add the remaining characters.
        for (int i = 1; i < the_string.Length; i++)
        {
            if (char.IsUpper ( the_string[i] )) result += " ";
            result += the_string[i];
        }

        return result;
    }
}
