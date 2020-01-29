using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemDatabase 
{
    public static bool IsInitialised { get; private set; }

    public static Dictionary<int, ItemBaseData> itemDictionary { get; private set; } = new Dictionary<int, ItemBaseData> ()
    {
        { 0, new ItemData_CaloriePill(0) },
        { 1, new ItemData_CalorieCapsule(1) },
        { 2, new ItemData_CalorieShake(2) },
        { 3, new ItemData_Potion(3) },
        { 4, new ItemData_Coins(4) },
        { 5, new ItemData_Fudge(5) },
        { 6, new ItemData_CallTheJudgeFudge(6) },
        { 7, new ItemData_Egg(7) },
        { 8, new ItemData_BucketOfMilk(8) },
        { 9, new ItemData_PotOfFlour(9) },
        { 10, new ItemData_SliceOfCake(10) },
        { 11, new ItemData_UnfinishedBatter(11) },
        { 12, new ItemData_PotOfBatter(12) },
        { 13, new ItemData_CakeTin(13) },
        { 14, new ItemData_UncookedCake(14) },
        { 15, new ItemData_SpongeCake(15) },
        { 16, new ItemData_ChocolateSpongeCake(16) },
        { 17, new ItemData_GroundChocolate(17) },
        { 18, new ItemData_PestleAndMortar(18) },
        { 19, new ItemData_Knife(19) },
        { 20, new ItemData_ChocolateBar(20) },
        { 21, new ItemData_SliceOfChocolateCake(21) },
        { 22, new ItemData_SliceOfSpongeCake(22) },
        { 23, new ItemData_Bucket(23) },
        { 24, new ItemData_Pot(24) },
        { 25, new ItemBaseData_BakingInstructions(25) },
    };

    public static void Initialise ()
    {
        if (IsInitialised)
        {
            Debug.LogError ( "Item Database has already been initialised" );
            return;
        }

        foreach (KeyValuePair<int, ItemBaseData> item in itemDictionary)
        {
            ValidateItem ( item.Value );
        }

        IsInitialised = true;
    }   

    private static void ValidateItem(ItemBaseData item)
    {
        if (item.IsSoulbound && item.IsSellable)
        {
            Debug.LogError ( string.Format ( "Item [{0} : {1}] is flagged as soulbound and sellable", item.ID, item.Name ) );
        }

        if (item.IsUnique && item.MaxStack > 1)
        {
            Debug.LogError ( string.Format ( "Item [{0} : {1}] is flagged as unique and has a max stack greater than 1", item.ID, item.Name ) );
        }

        if (!item.IsSellable && item.SellPrice > 1)
        {
            Debug.LogError ( string.Format ( "Item [{0} : {1}] is not flagged as sellable but has a sell price assigned", item.ID, item.Name ) );
        }

        if (item.MaxStack < 1)
        {
            Debug.LogError ( string.Format ( "Item [{0} : {1}] can not be assigned to an inventory as it has a max stack of less than 1", item.ID, item.Name ) );
        }

        if (string.IsNullOrEmpty ( item.Name ))
        {
            Debug.LogError ( string.Format ( "Item [{0}] does not have a name", item.ID ) );
        }

        if (string.IsNullOrEmpty ( item.Description ))
        {
            Debug.LogError ( string.Format ( "Item [{0}] does not have a description", item.ID ) );
        }

        if (item.Sprite == null)
        {
            Debug.LogError ( string.Format ( "Item [{0} : {1}] does not have a sprite assigned", item.ID, item.Name ) );
        }
    }

    public static bool GetItem(int id, out ItemBaseData item)
    {
        if (itemDictionary.ContainsKey ( id ))
        {
            item = itemDictionary[id];
            return true;
        }
        else
        {
            item = null;
            return false;
        }
    }

    public static bool ItemExists(int id)
    {
        return itemDictionary.ContainsKey ( id );
    }

    public static ItemBaseData GetItem(int id)
    {
        if (!itemDictionary.ContainsKey ( id )) { Debug.LogError ( "ItemID " + id + " does not exist." ); return null; }
        return itemDictionary[id];
    }

    public static string[] GetStrings ()
    {
        return itemDictionary.Values.Select ( x => x.Name ).ToArray ();
    }

    public static void SendTo (this Inventory from, Inventory to, int id, int amount)
    {
        int amountReturned = to.AddItem ( id, amount );
        int amountAdded = amount - amountReturned;

        if (amountAdded > 0)
            from.RemoveItem ( id, amountAdded );
    }

    public static string GetInteractionPrefix (int itemID)
    {
        ItemBaseData item = ItemDatabase.GetItem ( itemID );

        if (item == null) return "Null Item";

        if (ItemContainerCanvas.instance.IsActive)
        {
            if (item.IsSoulbound || !ItemContainerCanvas.instance.targetInventory.canRecieveItems)
            {
                return "";
            }
            else
            {
                return "Store";
            }
        }
        else
        {
            return item.GetDefaultInteractionData ().interactType.ToString ();
        }
    }

    public static void InvokeInventoryDefaultAction (int itemID, int inventoryIndex)
    {
        if (itemID < 0) return;

        ItemBaseData item = ItemDatabase.GetItem ( itemID );
        if (item == null) return;

        Inventory playerInventory = EntityManager.instance.PlayerInventory;

        if (ItemContainerCanvas.instance.IsActive)
        {
            if (item.IsSoulbound)
            {
                Debug.LogError ( "Cannot remove a soulbound item from your inventory" );
            }
            else
            {
                playerInventory.SendTo ( ItemContainerCanvas.instance.targetInventory, item.ID, 1 );
            }
        }
        else
        {
            item.GetDefaultInteractionData ().onInteract?.Invoke (inventoryIndex);
        }
    }
}
