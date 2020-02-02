using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptCreatorEditorWindow : EditorWindow
{
    private static ScriptCreatorEditorWindow window;
    public enum ScriptType { Default, Editor, EditorWindow, PropertyDrawer, Attribute }

    [MenuItem("Tools/Create Script")]
    public static void OpenWindow ()
    {
        window = EditorWindow.GetWindow<ScriptCreatorEditorWindow> ();
        window.Show ();
        window.titleContent = new GUIContent ( "Create Script" );
    }

    private string className = "MyClass";
    private string inherits = "Monobehaviour";
    private string namespaceName;
    private bool addSingleton = false;
    private bool dontDestroyOnLoadSingleton = false;
    private bool addStartMethod = false;
    private bool addUpdateMethod = false;
    private string assetPath = "Assets/";
    private ScriptType scriptType = ScriptType.Default;

    private void OnGUI ()
    {
        scriptType = (ScriptType)EditorGUILayout.EnumPopup ( "Script Type", scriptType ) ;
        EditorGUILayout.Space ();
        className = EditorGUILayout.TextField ( "Class Name", className );
        inherits = EditorGUILayout.TextField ( "Inherits", inherits );
        namespaceName = EditorGUILayout.TextField ( "Namespace", namespaceName );
        EditorGUILayout.Space ();
        addSingleton = EditorGUILayout.Toggle ( "Singleton", addSingleton );
        dontDestroyOnLoadSingleton = EditorGUILayout.Toggle ( "DontDestroyOnLoad", dontDestroyOnLoadSingleton );
        EditorGUILayout.Space ();
        addStartMethod = EditorGUILayout.Toggle ( "Start Method", addStartMethod );
        addUpdateMethod = EditorGUILayout.Toggle ( "Update Method", addUpdateMethod );
        EditorGUILayout.Space ();

        EditorGUILayout.BeginHorizontal ();
        assetPath = EditorGUILayout.TextField ( "Asset Path",  assetPath );
        if(GUILayout.Button("Select Folder" ))
        {
            assetPath = EditorUtility.OpenFolderPanel ( "Folder", Application.dataPath, "" ).Replace ( Application.dataPath, "Assets" );
        }
        EditorGUILayout.EndHorizontal ();

        GUILayout.FlexibleSpace ();

        EditorGUILayout.BeginHorizontal ();

        if (GUILayout.Button ( "Create" ))
        {
            Create ();
        }

        EditorGUILayout.EndHorizontal ();
    }

    private void Create ()
    {
        string s = "";

        s += "using UnityEngine";
        s += "\n";

        if (!string.IsNullOrEmpty ( namespaceName ))
        {
            s += "namespace " + namespaceName;
            s += "\n";
            s += "{";
            s += "\n";
        }

        if (string.IsNullOrEmpty ( inherits ))
        {
            s += "public class " + className;
        }
        else
        {
            s += "public class " + className + " : " + inherits;
        }

        s += "\n";
        s += "{";
        s += "\n";

        if (addSingleton)
        {
            AddNewLine ( ref s, "public static " + className + " instance;" );
            AddNewLine ( ref s );
            AddNewLine ( ref s, "private void Awake()" );
            AddNewLine ( ref s, "{" );

            AddNewLine ( ref s, "if(instance == null) instance = this;" );
            AddNewLine ( ref s, "else if (instance != this) { Destroy(this.gameObject); return; }" );

            if (dontDestroyOnLoadSingleton)
            {
                AddNewLine ( ref s );
                AddNewLine ( ref s, "DontDestroyOnLoad(this.gameObject);" );
            }

            AddNewLine ( ref s, "}" );
        }

        s += "\n";
        s += "}";

        if (!string.IsNullOrEmpty ( namespaceName ))
        {
            s += "\n";
            s += "}";
        }

        TextAsset text = new TextAsset ( s );
        AssetDatabase.CreateAsset ( text, assetPath + "/" + className + ".cs" );
        AssetDatabase.SaveAssets ();
    }

    private void AddNewLine(ref string s, string content = "")
    {
        s += "\n";
        s += content;
    }
}
