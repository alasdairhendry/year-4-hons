using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDatabaseFormatter : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] [NaughtyAttributes.ResizableTextArea] private string result;
    [SerializeField] private List<string> items = new List<string> ();

    [NaughtyAttributes.Button]
    public void Format ()
    {
        string s = "";
        int id = 0;
        
        for (int i = 0; i < items.Count; i++)
        {
            //s += "{" + string.Format ( "{ {0}, new {1}({2}) },\n", id, items[i].GetType ().ToString (), id );
            s += "{ " + id.ToString () + ", new " + items[i] + "(" + id.ToString () + ") },\n";
            if (i % 1 == 0)
                id++;
        }

        result = s;
    }
#endif
}
