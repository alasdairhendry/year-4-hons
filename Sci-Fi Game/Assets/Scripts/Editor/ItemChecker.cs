using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class ItemChecker
{
    [MenuItem ( "Tools/Item Checker" )]
    public static void Check ()
    {
        List<DropTable> dropTables = FindAssetsByType<DropTable> ();
        List<NPCShopkeeper> shopkeepers = GameObject.FindObjectsOfType<NPCShopkeeper> ().ToList ();

        for (int i = 0; i < 1000; i++)
        {
            ItemBaseData item = null;

            if (ItemDatabase.GetItem ( i, out item ))
            {
                string s = "<b>" + item.Name + "</b>" + "\n";

                bool found = false;

                for (int y = 0; y < dropTables.Count; y++)
                {
                    if (dropTables[y].loot.Exists ( x => x.itemID == item.ID ))
                    {
                        found = true;
                        s += "Drop Table: " + dropTables[y].name + "\n";
                    }
                }

                for (int y = 0; y < shopkeepers.Count; y++)
                {
                    if (shopkeepers[y].BaseInventory.Exists ( x => x.itemID == item.ID ))
                    {
                        found = true;
                        s += "Shopkeeper: " + shopkeepers[y].name + "\n";
                    }
                }

                if (!found)
                {
                    s = "<b>" + item.Name + "</b>" + " - NO DATA FOUND";
                }

                Debug.Log ( s );
            }
        }
    }

    public static List<T> FindAssetsByType<T> () where T : UnityEngine.Object
    {
        List<T> assets = new List<T> ();
        string[] guids = AssetDatabase.FindAssets ( string.Format ( "t:{0}", typeof ( T ).ToString ().Replace ( "UnityEngine.", "" ) ) );
        for (int i = 0; i < guids.Length; i++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath ( guids[i] );
            T asset = AssetDatabase.LoadAssetAtPath<T> ( assetPath );
            if (asset != null)
            {
                assets.Add ( asset );
            }
        }
        return assets;
    }
}
