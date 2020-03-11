using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemCheckerWIndow : EditorWindow
{
    public static ItemCheckerWIndow instance;

    Vector2 scrollPos = new Vector2 ();

    int idToShow = -1;

    [MenuItem("Window/Item Checker")]
    public static void Open ()
    {
        instance = GetWindow<ItemCheckerWIndow> ();
        instance.Show ();
        instance.titleContent = new GUIContent ( "Item Checker" );
    }

    private void OnGUI ()
    {
        if (ItemDatabase.ItemExists ( idToShow ))
        {
            DrawInfo ();
        }
        else
        {
            DrawList ();
        }
    }

    private void DrawInfo ()
    {
        ItemBaseData item = null;

        if (ItemDatabase.GetItem ( idToShow, out item ))
        {
            EditorGUILayout.BeginHorizontal ();           

            if (GUILayout.Button ( "Prev Item" ))
            {
                idToShow--;
            }

            if (GUILayout.Button ( "Next Item" ))
            {
                idToShow++;
            }

            if (GUILayout.Button ( "Close" ))
            {
                idToShow = -1;
            }

            EditorGUILayout.EndHorizontal ();

            if (item.IsQuestItem)
            {
                EditorGUILayout.LabelField ( "[" + item.ID + "] " + item.Name + " - [Quest Item]", EditorStyles.boldLabel );
            }
            else
            {
                EditorGUILayout.LabelField ( "[" + item.ID + "] " + item.Name, EditorStyles.boldLabel );
            }

            List<DropTable> dropTables = FindAssetsByType<DropTable> ();
            List<NPCData> npcData = FindAssetsByType<NPCData> ();
            List<NPCShopkeeper> shopkeepers = GameObject.FindObjectsOfType<NPCShopkeeper> ().ToList ();
            List<CraftingTable> craftingTables = FindAssetsByType<CraftingTable> ();

            EditorGUILayout.Space ();
            EditorGUILayout.Space ();

            scrollPos = EditorGUILayout.BeginScrollView ( scrollPos, false, false );


            EditorGUILayout.LabelField ( "NPCs" );
            for (int y = 0; y < npcData.Count; y++)
            {
                if (npcData[y].UniqueDropTable != null)
                {
                    if (npcData[y].UniqueDropTable.loot.Exists ( x => x.itemID == item.ID ))
                    {
                        if (GUILayout.Button ( string.Format ( "{0} - [{1}]", npcData[y].NpcName, npcData[y].UniqueDropTable.name ) ))
                        {
                            EditorGUIUtility.PingObject ( npcData[y] );
                        }
                    }
                }

                if (npcData[y].AccessToCoinsDropTable)
                {
                    DropTable dropTable = dropTables.First ( x => x.name.Contains ( "Coins Drop" ) );

                    if (dropTable.loot.Exists ( x => x.itemID == item.ID ))
                    {
                        if (GUILayout.Button ( string.Format ( "{0} - [{1}]", npcData[y].NpcName, dropTable.name ) ))
                        {
                            EditorGUIUtility.PingObject ( npcData[y] );
                        }
                    }
                }

                if (npcData[y].AccessToIngredientsDropTable)
                {
                    DropTable dropTable = dropTables.First ( x => x.name.Contains ( "Ingredients Drop" ) );

                    if (dropTable.loot.Exists ( x => x.itemID == item.ID ))
                    {
                        if (GUILayout.Button ( string.Format ( "{0} - [{1}]", npcData[y].NpcName, dropTable.name ) ))
                        {
                            EditorGUIUtility.PingObject ( npcData[y] );
                        }
                    }
                }

                if (npcData[y].AccessToMeleeDropTable)
                {
                    DropTable dropTable = dropTables.First ( x => x.name.Contains ( "Melee Drop" ) );

                    if (dropTable.loot.Exists ( x => x.itemID == item.ID ))
                    {
                        if (GUILayout.Button ( string.Format ( "{0} - [{1}]", npcData[y].NpcName, dropTable.name ) ))
                        {
                            EditorGUIUtility.PingObject ( npcData[y] );
                        }
                    }
                }

                if (npcData[y].AccessToGunDropTable)
                {
                    DropTable dropTable = dropTables.First ( x => x.name.Contains ( "Gun Drop" ) );

                    if (dropTable.loot.Exists ( x => x.itemID == item.ID ))
                    {
                        if (GUILayout.Button ( string.Format ( "{0} - [{1}]", npcData[y].NpcName, dropTable.name ) ))
                        {
                            EditorGUIUtility.PingObject ( npcData[y] );
                        }
                    }
                }

                if (npcData[y].AccessToMaskTable)
                {
                    DropTable dropTable = dropTables.First ( x => x.name.Contains ( "Masks Drop" ) );

                    if (dropTable.loot.Exists ( x => x.itemID == item.ID ))
                    {
                        if (GUILayout.Button ( string.Format ( "{0} - [{1}]", npcData[y].NpcName, dropTable.name ) ))
                        {
                            EditorGUIUtility.PingObject ( npcData[y] );
                        }
                    }
                }

                if (npcData[y].AccessToPartyHatTable)
                {
                    DropTable dropTable = dropTables.First ( x => x.name.Contains ( "Party Hat Drop" ) );

                    if (dropTable.loot.Exists ( x => x.itemID == item.ID ))
                    {
                        if (GUILayout.Button ( string.Format ( "{0} - [{1}]", npcData[y].NpcName, dropTable.name ) ))
                        {
                            EditorGUIUtility.PingObject ( npcData[y] );
                        }
                    }
                }
            }

            EditorGUILayout.Space ();
            EditorGUILayout.Space ();



            EditorGUILayout.LabelField ( "Drop Tables" );
            for (int y = 0; y < dropTables.Count; y++)
            {
                if (dropTables[y].loot.Exists ( x => x.itemID == item.ID ))
                {
                    if (GUILayout.Button ( dropTables[y].name ))
                    {
                        EditorGUIUtility.PingObject ( dropTables[y] );
                    }
                }
            }

            EditorGUILayout.Space ();
            EditorGUILayout.Space ();

            EditorGUILayout.LabelField ( "Shopkeepers" );
            for (int y = 0; y < shopkeepers.Count; y++)
            {
                if (shopkeepers[y].BaseInventory.Exists ( x => x.itemID == item.ID ))
                {
                    if (GUILayout.Button ( shopkeepers[y].name ))
                    {
                        EditorGUIUtility.PingObject ( shopkeepers[y] );
                    }
                }
            }

            EditorGUILayout.Space ();
            EditorGUILayout.Space ();

            EditorGUILayout.LabelField ( "Crafting" );
            for (int y = 0; y < craftingTables.Count; y++)
            {
                if (craftingTables[y].recipes.Exists ( x => x.resultingItems.Exists ( o => o.ID == item.ID ) ))
                {
                    if (GUILayout.Button ( craftingTables[y].name ))
                    {
                        EditorGUIUtility.PingObject ( craftingTables[y] );
                    }
                }
            }










            EditorGUILayout.EndScrollView ();
        }
        else
        {
            idToShow = -1;
        }
    }

    private void DrawList ()
    {
        List<DropTable> dropTables = FindAssetsByType<DropTable> ();
        List<NPCShopkeeper> shopkeepers = GameObject.FindObjectsOfType<NPCShopkeeper> ().ToList ();

        scrollPos = EditorGUILayout.BeginScrollView ( scrollPos, false, false );

        for (int i = 0; i < 1000; i++)
        {
            ItemBaseData item = null;

            if (ItemDatabase.GetItem ( i, out item ))
            {
                int countFound = 0;

                for (int y = 0; y < dropTables.Count; y++)
                {
                    if (dropTables[y].loot.Exists ( x => x.itemID == item.ID ))
                    {
                        countFound++;
                    }
                }

                for (int y = 0; y < shopkeepers.Count; y++)
                {
                    if (shopkeepers[y].BaseInventory.Exists ( x => x.itemID == item.ID ))
                    {
                        countFound++;
                    }
                }

                string s = string.Format ( "[{0}] {1}", countFound, item.Name );

                Color guiColour = GUI.contentColor;

                if (countFound == 0)
                {
                    GUI.contentColor = Color.red;
                }

                if (GUILayout.Button ( s ))
                {
                    idToShow = item.ID;
                }

                if (countFound == 0)
                {
                    GUI.contentColor = guiColour;
                }
            }
        }

        EditorGUILayout.EndScrollView ();
    }

    private void DisplayItem(int itemID)
    {
        ItemBaseData item = null;

        if (ItemDatabase.GetItem ( itemID, out item ))
        {
            
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
