using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootCanvas : MonoBehaviour
{
    //public static LootCanvas instance;

    //private void Awake ()
    //{
    //    if (instance == null) instance = this;
    //    else if (instance != this) { Destroy ( this.gameObject ); return; }
    //}

    //[SerializeField] private GameObject panel;
    //[SerializeField] private List<GameObject> lootEntries = new List<GameObject> ();
    //private Inventory currentLoot = new Inventory ( 30, false, true );

    //public void Open (List<Inventory.ItemStack> items)
    //{
    //    currentLoot.ClearInventoryStacks ();

    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        currentLoot.AddItem ( items[i].ID, items[i].Amount );
    //    }

    //    for (int i = 0; i < lootEntries.Count; i++)
    //    {
    //        if(i > currentLoot.GetStackCount)
    //        {
    //            lootEntries[i].SetActive ( false );
    //        }
    //        else
    //        {
    //            lootEntries[i].SetActive ( true );
    //            lootEntries[i].GetComponentInChildren<TextMeshProUGUI> ().text = ItemDatabase.GetItem ( currentLoot.GetStackAtIndex ( i ).ID ).Name + "[" + currentLoot.GetStackAtIndex ( i ).Amount + "]";
    //            lootEntries[i].GetComponentInChildren<Button>()
    //        }
    //    }

    //    for (int i = 0; i < currentLoot.stacks.Count; i++)
    //    {
    //        loot
    //    }
    //}

}
