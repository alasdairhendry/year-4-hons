using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemCreator : EditorWindow
{
    private static ItemCreator window;

    [MenuItem ( "Tools/Item Creator Window" )]
    public static void ShowWindow ()
    {
        window = GetWindow<ItemCreator> ();
        window.titleContent = new GUIContent ( "Item Creator Window" );
    }

    private void OnEnable ()
    {
        ValidateWindow ();
    }

    private void OnFocus ()
    {
        ValidateWindow ();
    }

    private void ValidateWindow ()
    {
        DirectoryInfo info = new DirectoryInfo ( Application.dataPath.Replace ( "Assets", "" ) + ItemPath );

        minID = info.GetFiles ().ToList ().Where ( x => x.Extension == ".cs" ).Count ();
        ID = minID;
    }

    private int minID;

    public int ID;
    public string Name;
    public string Description;
    public ItemCategory category;

    public bool IsSellable;
    public bool IsSoulbound;
    public bool IsUnique;

    public int MaxStack;
    public int[] RelatedQuestIDs = new int[] { };

    public int SellPrice;
    public int BuyPrice;
    public Sprite Sprite;

    public const string ItemPath = "Assets/Scripts/ItemSystem/Items/";
    public const string SpritePath = "Assets/Scripts/ItemSystem/Resources/Items/Sprites/";

    private void OnGUI ()
    {
        EditorGUI.BeginChangeCheck ();
        ID = EditorGUILayout.IntField ( "ID", ID );
        if (EditorGUI.EndChangeCheck ())
        {
            DirectoryInfo info = new DirectoryInfo ( Application.dataPath.Replace ( "Assets", "" ) + ItemPath );

            minID = info.GetFiles ().ToList ().Where ( x => x.Extension == ".cs" ).Count ();
            ID = minID;
        }
        Name = EditorGUILayout.TextField ( "Name", Name );
        Description = EditorGUILayout.TextField ( "Description", Description );
        category = (ItemCategory)EditorGUILayout.EnumPopup ( "Category", category );

        IsSellable = EditorGUILayout.Toggle ( "Is Sellable", IsSellable );
        IsSoulbound = EditorGUILayout.Toggle ( "Is Soulbound", IsSoulbound );
        IsUnique = EditorGUILayout.Toggle ( "Is Unique", IsUnique );

        MaxStack = EditorGUILayout.IntField ( "Max Stack", MaxStack );
        SellPrice = EditorGUILayout.IntField ( "Sell Price", SellPrice );
        BuyPrice = EditorGUILayout.IntField ( "Buy Price", BuyPrice );
        Sprite = (Sprite)EditorGUILayout.ObjectField ( "Sprite", Sprite, typeof ( Sprite ), false );

        string quests = "";

        for (int i = 0; i < RelatedQuestIDs.Length; i++)
        {
            if (i <= RelatedQuestIDs.Length - 1)
                quests += RelatedQuestIDs.ToString () + ", ";
            else
                quests += RelatedQuestIDs.ToString ();
        }

        if (GUILayout.Button ( "Create" ))
        {


            string fileName = "ItemData_" + Name.Replace ( " ", "" );

            using (StreamWriter outFile =
               new StreamWriter ( ItemPath + fileName + ".cs" ))
            {
                outFile.WriteLine ( "public class ItemData_" + fileName + " : ItemBaseData" );
                outFile.WriteLine ( "{" );
                outFile.WriteLine ( "public ItemData_" + fileName + " (int ID) : base ( ID )" );
                outFile.WriteLine ( "{" );
                outFile.WriteLine ( "   base.Name = \"" + Name + "\";" );
                outFile.WriteLine ( "base.Description = \"" + Description + "\";" );
                outFile.WriteLine ( "base.category = ItemCategory." + category.ToString () + ";" );
                outFile.WriteLine ( "" );
                outFile.WriteLine ( "base.IsSellable = " + (IsSellable ? "true" : "false") + ";" );
                outFile.WriteLine ( "base.IsSoulbound = " + (IsSoulbound ? "true" : "false") + ";" );
                outFile.WriteLine ( "base.IsUnique = " + (IsUnique ? "true" : "false") + ";" );
                outFile.WriteLine ( "" );
                outFile.WriteLine ( "base.MaxStack = " + MaxStack.ToString () + ";" );
                outFile.WriteLine ( "base.RelatedQuestIDs = new int[] { " + quests + " };" );
                outFile.WriteLine ( "" );
                outFile.WriteLine ( "base.SellPrice = " + SellPrice.ToString () + ";" );
                outFile.WriteLine ( "base.BuyPrice = " + BuyPrice.ToString () + ";" );
                outFile.WriteLine ( "}" );
                outFile.WriteLine ( "}" );
            }

            Debug.Log ( AssetDatabase.GetAssetPath ( Sprite ) );
            Debug.Log ( SpritePath + ID + ".png" );
            AssetDatabase.CopyAsset ( AssetDatabase.GetAssetPath ( Sprite ), SpritePath + ID + ".png" );
            AssetDatabase.Refresh ();
        }
    }
}

public class ItemSpriteCreator : EditorWindow
{
    private static ItemSpriteCreator window;

    [MenuItem ( "Tools/Item Sprite Window" )]
    public static void ShowWindow ()
    {
        window = GetWindow<ItemSpriteCreator> ();
        window.titleContent = new GUIContent ( "Item Sprite Window" );
    }

    public const string SpritePath = "Assets/Scripts/ItemSystem/Resources/Items/Sprites/";

    public Sprite Sprite { get; private set; }
    public bool deleteOriginal { get; protected set; }
    public int itemID { get; protected set; }

    private void OnGUI ()
    {
        EditorGUILayout.BeginHorizontal ();
        itemID = EditorGUILayout.IntField ( "Item", itemID );
        itemID = EditorGUILayout.Popup ( itemID, ItemDatabase.GetStrings () );
        EditorGUILayout.EndHorizontal ();
        Sprite = (Sprite)EditorGUILayout.ObjectField ( "Sprite", Sprite, typeof ( Sprite ), false );
        AssetPreview.GetAssetPreview ( Sprite );
        deleteOriginal = EditorGUILayout.Toggle ( "Delete Original", deleteOriginal );

        if (GUILayout.Button ( "Apply" ))
        {
            AssetDatabase.CopyAsset ( AssetDatabase.GetAssetPath ( Sprite ), SpritePath + itemID + "_" + ItemDatabase.GetItem ( itemID ).Name + ".png" );

            if (deleteOriginal)
            {
                AssetDatabase.DeleteAsset ( AssetDatabase.GetAssetPath ( Sprite ) );
            }

            AssetDatabase.Refresh ();
            itemID++;
        }
    }
}
