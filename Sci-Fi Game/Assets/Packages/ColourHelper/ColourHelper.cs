using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ColourDescription { OffWhiteText, DarkYellowText, GreyText, DarkGreyText, Black, White, MessageBoxInfo, MessageBoxWarning, MessageBoxError }


public class ColourHelper : MonoBehaviour
{
    public static ColourHelper instance;

    [SerializeField] private List<ColourHelperData> data = new List<ColourHelperData> ();
    private Dictionary<ColourDescription, ColourHelperData> dataDictionary = new Dictionary<ColourDescription, ColourHelperData> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        DontDestroyOnLoad ( this.gameObject );
        CreateDictionary ();
    }

    private void CreateDictionary ()
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (dataDictionary.ContainsKey ( data[i].key ))
            {
                Debug.LogError ( "Colour Key " + data[i].key + " already exists." );
            }
            else
            {
                dataDictionary.Add ( data[i].key, data[i] );
            }
        }
    }

    public static string TagColour (string message, ColourDescription colour)
    {
        if (instance == null)
        {
            Debug.LogError ( "Instance does not exist." );
            return message;
        }

        message = message.Insert ( 0, "<color=#" + ColorUtility.ToHtmlStringRGBA ( instance.dataDictionary[colour].colour ) + ">" );
        return message + "</color>";
    }

    public static string TagSize (string message, float size)
    {
        if (instance == null)
        {
            Debug.LogError ( "Instance does not exist." );
            return message;
        }

        message = message.Insert ( 0, "<size=" + size.ToString() + "%>" );
        return message + "</size>";
    }

    public static Color GetEditorColour(ColourDescription colourDescription)
    {
        if(FindObjectOfType<ColourHelper> () == null)
        {
            Debug.LogError ( "afafo" );
            return Color.white;
        }

        if(FindObjectOfType<ColourHelper> ().data == null || FindObjectOfType<ColourHelper> ().data.Count <= 0)
        {
            Debug.LogError ( "afafo" );
            return Color.white;
        }

        return FindObjectOfType<ColourHelper> ().data.FirstOrDefault ( x => x.key == colourDescription ).colour;
    }

}
