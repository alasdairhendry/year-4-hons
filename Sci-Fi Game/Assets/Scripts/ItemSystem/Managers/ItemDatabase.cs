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
        { 3, new ItemData_Coins(3) },
        { 4, new ItemData_Fudge(4) },
        { 5, new ItemData_CallTheJudgeFudge(5) },
        { 6, new ItemData_Egg(6) },
        { 7, new ItemData_BucketOfMilk(7) },
        { 8, new ItemData_PotOfFlour(8) },
        { 9, new ItemData_CakeTin(9) },
        { 10, new ItemData_SpongeCake(10) },
        { 11, new ItemData_ChocolateSpongeCake(11) },
        { 12, new ItemData_Knife(12) },
        { 13, new ItemData_ChocolateBar(13) },
        { 14, new ItemData_BakingInstructions(14) },
        { 15, new ItemData_Rifle(15) },
        { 16, new ItemData_IronIngot(16) },
        { 17, new ItemData_IronOre(17) },
        { 18, new ItemData_SteelIngot(18) },
        { 19, new ItemData_Charcoal(19) },
        { 20, new ItemData_MetalScraps(20) },
        { 21, new ItemData_LED(21) },
        { 22, new ItemData_Lens(22) },

        { 23, new ItemData_Pistol(23) },
        { 24, new ItemData_Blaster(24) },
        { 25, new ItemData_BlasterRifle(25) },
        { 26, new ItemData_Phaser(26) },
        { 27, new ItemData_PhaserRifle(27) },

        { 28, new ItemData_Wildflower(28) },
        { 29, new ItemData_Sugarplum(29) },
        { 30, new ItemData_SkunkWeed(30) },
        { 31, new ItemData_RottenPoppy(31) },
        { 32, new ItemData_AeratedCompost(32) },

        { 33, new ItemData_Chisel(33) },
        { 34, new ItemData_Hammer(34) },
        { 35, new ItemData_PlasticScraps(35) },

        { 36, new ItemData_Silencer(36) },
        { 37, new ItemData_Compensator(37) },
        { 38, new ItemData_Stabiliser(38) },
        { 39, new ItemData_ExtendedBarrel(39) },
        { 40, new ItemData_ExtendedMag(40) },
        { 41, new ItemData_StepTrigger(41) },
        { 42, new ItemData_Selector(42) },
        { 43, new ItemData_AxleCharger(43) },

        { 44, new ItemData_HammerWeapon(44) },
        { 45, new ItemData_ChiselWeapon(45) },
        { 46, new ItemData_Flamingo(46) },
        { 47, new ItemData_Mallet(47) },
        { 48, new ItemData_Pan(48) },
        { 49, new ItemData_Saber(49) },
        { 50, new ItemData_Screwdriver(50) },
        { 51, new ItemData_Sword(51) },
        { 52, new ItemData_Wrench(52) },
        { 53, new ItemData_Gmaul(53) },

        //{ 54, new ItemData_Wrench(54) },
        //{ 55, new ItemData_Wrench(55) },
        //{ 56, new ItemData_Wrench(56) },
        //{ 57, new ItemData_Wrench(57) },
        //{ 58, new ItemData_Wrench(58) },
        //{ 59, new ItemData_Wrench(59) },

        //{ 60, new ItemData_Wrench(60) },
        //{ 61, new ItemData_Wrench(61) },
        //{ 62, new ItemData_Wrench(62) },
        //{ 63, new ItemData_Wrench(63) },
        //{ 64, new ItemData_Wrench(64) },
        //{ 65, new ItemData_Wrench(65) },
        //{ 66, new ItemData_Wrench(66) },
        //{ 67, new ItemData_Wrench(67) },
        //{ 68, new ItemData_Wrench(68) },
        //{ 69, new ItemData_Wrench(69) },

        //{ 70, new ItemData_Wrench(70) },
        //{ 71, new ItemData_Wrench(71) },
        //{ 72, new ItemData_Wrench(72) },
        //{ 73, new ItemData_Wrench(73) },
        //{ 74, new ItemData_Wrench(74) },
        //{ 75, new ItemData_Wrench(75) },
        //{ 76, new ItemData_Wrench(76) },
        //{ 77, new ItemData_Wrench(77) },
        //{ 78, new ItemData_Wrench(78) },
        //{ 79, new ItemData_Wrench(79) },

        //{ 80, new ItemData_Wrench(80) },
        //{ 81, new ItemData_Wrench(81) },
        //{ 82, new ItemData_Wrench(82) },
        //{ 83, new ItemData_Wrench(83) },
        //{ 84, new ItemData_Wrench(84) },
        //{ 85, new ItemData_Wrench(85) },
        //{ 86, new ItemData_Wrench(86) },
        //{ 87, new ItemData_Wrench(87) },
        //{ 88, new ItemData_Wrench(88) },
        //{ 89, new ItemData_Wrench(89) },

        //{ 90, new ItemData_Wrench(90) },
        //{ 91, new ItemData_Wrench(91) },
        //{ 92, new ItemData_Wrench(92) },
        //{ 93, new ItemData_Wrench(93) },
        //{ 94, new ItemData_Wrench(94) },
        //{ 95, new ItemData_Wrench(95) },
        //{ 96, new ItemData_Wrench(96) },
        //{ 97, new ItemData_Wrench(97) },
        //{ 98, new ItemData_Wrench(98) },
        //{ 99, new ItemData_Wrench(99) },
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
            Debug.LogWarning ( string.Format ( "Item [{0} : {1}] is flagged as soulbound and sellable", item.ID, item.Name ) );
        }

        if (item.IsUnique && item.MaxStack > 1)
        {
            Debug.LogWarning ( string.Format ( "Item [{0} : {1}] is flagged as unique and has a max stack greater than 1", item.ID, item.Name ) );
        }

        if (!item.IsSellable && item.SellPrice > 1)
        {
            Debug.LogWarning ( string.Format ( "Item [{0} : {1}] is not flagged as sellable but has a sell price assigned", item.ID, item.Name ) );
        }

        if (item.MaxStack < 1)
        {
            Debug.LogWarning ( string.Format ( "Item [{0} : {1}] can not be assigned to an inventory as it has a max stack of less than 1", item.ID, item.Name ) );
        }

        if (string.IsNullOrEmpty ( item.Name ))
        {
            Debug.LogWarning ( string.Format ( "Item [{0}] does not have a name", item.ID ) );
        }

        if (string.IsNullOrEmpty ( item.Description ))
        {
            Debug.LogWarning ( string.Format ( "Item [{0}] does not have a description", item.ID ) );
        }

        if (item.Sprite == null)
        {
            Debug.LogWarning ( string.Format ( "Item [{0} : {1}] does not have a sprite assigned", item.ID, item.Name ) );
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
        return itemDictionary.Values.Select ( x => x.ID + " - " +  x.Name + " [" + x.category + "]" ).ToArray ();
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
                if (Input.GetKey ( KeyCode.LeftShift ))
                    return "Store All ";
                else
                    return "Store ";
            }
        }
        else
        {
            if (item.GetDefaultInteractionData ().interactType == InventoryInteractionData.InteractType.DoNothing) return "";
            return item.GetDefaultInteractionData ().interactType.ToString () + " ";
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
                int shiftClick = playerInventory.GetStackAtIndex ( inventoryIndex ).Amount;

                if (Input.GetKey ( KeyCode.LeftShift ))
                    playerInventory.SendTo ( ItemContainerCanvas.instance.targetInventory, item.ID, shiftClick );
                else
                    playerInventory.SendTo ( ItemContainerCanvas.instance.targetInventory, item.ID, 1 );
            }
        }
        else
        {
            item.GetDefaultInteractionData ().onInteract?.Invoke (inventoryIndex);
        }
    }
}
