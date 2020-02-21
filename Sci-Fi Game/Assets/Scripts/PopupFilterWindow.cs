using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PopupFilterWindow : EditorWindow
{
    private string enumName;
    private List<string> valuesRaw = new List<string> ();
    private List<string> valuesFiltered = new List<string> ();
    private string filter;

    private EditorWindow parent;
    private Vector2 scroll = new Vector2 ();
    private System.Action<int> onSelectCallback;

    public void Open(string[] values, string popupName, Rect rect, System.Action<int> onSelect)
    {
        this.parent = focusedWindow;
        this.valuesRaw = values.ToList ();
        this.onSelectCallback = onSelect;
        this.enumName = popupName;
        this.filter = "";
        valuesFiltered.Clear ();
        scroll = new Vector2 ();

        float maxLength = 0.0f;
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i].Length > maxLength) maxLength = values[i].Length;
        }

        Rect screenRect = rect;        
        Vector2 rectSize = new Vector2 ( maxLength * 7.25f, 200 );
        screenRect.position = GUIUtility.GUIToScreenPoint ( screenRect.position );
        ShowAsDropDown ( screenRect, rectSize );
        titleContent = new GUIContent ( popupName );

        Focus ();
        GUI.FocusControl ( "filter" );
        FilterValues ( filter );
    }

    private void OnGUI ()
    {
        GUI.SetNextControlName ( "filter" );
        var filterUpdate = GUILayout.TextField ( filter );
        if (filterUpdate != filter)
            FilterValues ( filterUpdate );

        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
        {
            if (valuesFiltered.Count > 0)
            {
                ReturnValue ( valuesFiltered[0] );
                return;
            }
        }
        else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            Close ();
            parent.Repaint ();
            parent.Focus ();
            return;
        }
        else
        {
            GUI.FocusControl ( "filter" );
        }

        scroll = GUILayout.BeginScrollView ( scroll );

        for (int i = 0; i < valuesFiltered.Count; ++i)
        {
            var value = valuesFiltered[i];

            if (GUILayout.Button ( value ))
            {
                ReturnValue ( value );
                GUILayout.EndScrollView ();
                return;
            }
        }

        GUILayout.EndScrollView ();
    }

    private void ReturnValue (string value)
    {
        int unfilteredIndex = valuesRaw.IndexOf ( value );
        onSelectCallback?.Invoke ( unfilteredIndex );
        Close ();
        parent.Repaint ();
        parent.Focus ();
    }

    private void FilterValues (string filterUpdate)
    {
        filter = filterUpdate;

        var filterLower = filter.ToLower ();

        valuesFiltered.Clear ();

        if (string.IsNullOrEmpty ( filter ))
        {
            for (int i = 0; i < valuesRaw.Count; ++i)
            {
                valuesFiltered.Add ( valuesRaw[i] );
            }
        }
        else
        {
            for (int i = 0; i < valuesRaw.Count; ++i)
            {
                var value = valuesRaw[i];
                var lower = value.ToLower ();
                if (lower.Contains ( filterLower ))
                    valuesFiltered.Add ( value );
            }
        }
    }
}
