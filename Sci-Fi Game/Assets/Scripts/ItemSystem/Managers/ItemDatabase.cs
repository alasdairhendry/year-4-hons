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

        { 54, new ItemData_PistolBroken(54) },
        { 55, new ItemData_RifleBroken(55) },
        { 56, new ItemData_BlasterBroken(56) },
        { 57, new ItemData_BlasterRifleBroken(57) },
        { 58, new ItemData_PhaserBroken(58) },
        { 59, new ItemData_PhaserRifleBroken(59) },

        { 60, new ItemData_HammerWeaponBroken(60) },
        { 61, new ItemData_ChiselWeaponBroken(61) },
        { 62, new ItemData_FlamingoBroken(62) },
        { 63, new ItemData_MalletBroken(63) },
        { 64, new ItemData_PanBroken(64) },
        { 65, new ItemData_SaberBroken(65) },
        { 66, new ItemData_ScrewdriverBroken(66) },
        { 67, new ItemData_SwordBroken(67) },
        { 68, new ItemData_WrenchBroken(68) },
        { 69, new ItemData_GmaulBroken(69) },

        { 70, new ItemData_LetterToAggie(70) },
        { 71, new ItemData_OutletNozzle(71) },
        { 72, new ItemData_FrenchFry(72) },
        { 73, new ItemData_PortalGem(73) },
        { 74, new ItemData_SwordOfAHundredTruths(74) },
        { 75, new ItemData_ExperienceGem(75) },
        { 76, new ItemData_1GBFlashDrive(76) },
        { 77, new ItemData_EmporersMask(77) },
        { 78, new ItemData_Balaclava(78) },
        { 79, new ItemData_GasMask(79) },

        { 80, new ItemData_PaperBag(80) },
        { 81, new ItemData_LuchadorMask(81) },
        { 82, new ItemData_HorseHead(82) },
        { 83, new ItemData_HockeyMask(83) },
        { 84, new ItemData_ClownMask(84) },
        { 85, new ItemData_ChickenHead(85) },
        { 86, new ItemData_AlienMask(86) },
        { 87, new ItemData_CapMask(87) },
        { 88, new ItemData_PandaHead(88) },
        { 89, new ItemData_TigerHead(89) },

        { 90, new ItemData_WelderMask(90) },
        { 91, new ItemData_PartyHatWhite(91) },
        { 92, new ItemData_PartyHatRed(92) },
        { 93, new ItemData_PartyHatYellow(93) },
        { 94, new ItemData_PartyHatBlue(94) },
        { 95, new ItemData_PartyHatGreen(95) },
        { 96, new ItemData_ChristmasCracker(96) },
        { 97, new ItemData_StainedSweatPants(97) },
        { 98, new ItemData_EnergyDrink(98) },
        { 99, new ItemData_BronyAntiqueHead(99) },

        { 100, new ItemData_VitroBits(100) },
        { 101, new ItemData_HydroBits(101) },
        { 102, new ItemData_NitroBits(102) },
        { 103, new ItemData_Hoverboard(103) },
        //{ 104, new ItemData_Wrench(104) },
        //{ 105, new ItemData_Wrench(105) },
        //{ 106, new ItemData_Wrench(106) },
        //{ 107, new ItemData_Wrench(107) },
        //{ 108, new ItemData_Wrench(108) },
        //{ 109, new ItemData_Wrench(109) },

        //{ 110, new ItemData_Wrench(110) },
        //{ 111, new ItemData_Wrench(111) },
        //{ 112, new ItemData_Wrench(112) },
        //{ 113, new ItemData_Wrench(113) },
        //{ 114, new ItemData_Wrench(114) },
        //{ 115, new ItemData_Wrench(115) },
        //{ 116, new ItemData_Wrench(116) },
        //{ 117, new ItemData_Wrench(117) },
        //{ 118, new ItemData_Wrench(118) },
        //{ 119, new ItemData_Wrench(119) },

        //{ 120, new ItemData_Wrench(120) },
        //{ 121, new ItemData_Wrench(121) },
        //{ 122, new ItemData_Wrench(122) },
        //{ 123, new ItemData_Wrench(123) },
        //{ 124, new ItemData_Wrench(124) },
        //{ 125, new ItemData_Wrench(125) },
        //{ 126, new ItemData_Wrench(126) },
        //{ 127, new ItemData_Wrench(127) },
        //{ 128, new ItemData_Wrench(128) },
        //{ 129, new ItemData_Wrench(129) },

        //{ 130, new ItemData_Wrench(130) },
        //{ 131, new ItemData_Wrench(131) },
        //{ 132, new ItemData_Wrench(132) },
        //{ 133, new ItemData_Wrench(133) },
        //{ 134, new ItemData_Wrench(134) },
        //{ 135, new ItemData_Wrench(135) },
        //{ 136, new ItemData_Wrench(136) },
        //{ 137, new ItemData_Wrench(137) },
        //{ 138, new ItemData_Wrench(138) },
        //{ 139, new ItemData_Wrench(139) },

        //{ 140, new ItemData_Wrench(140) },
        //{ 141, new ItemData_Wrench(141) },
        //{ 142, new ItemData_Wrench(142) },
        //{ 143, new ItemData_Wrench(143) },
        //{ 144, new ItemData_Wrench(144) },
        //{ 145, new ItemData_Wrench(145) },
        //{ 146, new ItemData_Wrench(146) },
        //{ 147, new ItemData_Wrench(147) },
        //{ 148, new ItemData_Wrench(148) },
        //{ 149, new ItemData_Wrench(149) },
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

    public static bool GetItem (int id, out ItemBaseData item)
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

    public static void SendTo (this Inventory from, Inventory to, int id, int amount, bool playSFX)
    {
        int amountReturned = to.AddItem ( id, amount );
        int amountAdded = amount - amountReturned;

        if (amountAdded > 0)
        {
            from.RemoveItem ( id, amountAdded );

            if (playSFX)
                SoundEffectManager.Play ( AudioClipAsset.InventoryUpdated, AudioMixerGroup.SFX );
        }
    }

    public static string GetInteractionPrefix (int itemID)
    {
        ItemBaseData item = ItemDatabase.GetItem ( itemID );

        if (item == null) return "Null Item";

        if (ItemContainerCanvas.instance.isOpened)
        {
            if (Input.GetKey ( KeyCode.LeftShift ))
                return "Store All ";
            else
                return "Store ";
        }
        else if (StoreCanvas.instance.isOpened)
        {
            if (Input.GetKey ( KeyCode.LeftShift ))
                return "Sell All ";
            else
                return "Sell ";
        }
        else if (BankCanvas.instance.isOpened)
        {
            if (Input.GetKey ( KeyCode.LeftShift ))
                return "Store All ";
            else
                return "Store ";
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

        if (ItemContainerCanvas.instance.isOpened)
        {
            int shiftClick = playerInventory.GetStackAtIndex ( inventoryIndex ).Amount;

            if (Input.GetKey ( KeyCode.LeftShift ))
                PlayerInventoryController.SendItemFromInventoryToContainer ( item.ID, shiftClick );
            else
                PlayerInventoryController.SendItemFromInventoryToContainer ( item.ID, 1 );
        }
        else if (StoreCanvas.instance.isOpened)
        {
            int shiftClick = playerInventory.GetStackAtIndex ( inventoryIndex ).Amount;

            if (Input.GetKey ( KeyCode.LeftShift ))
                StoreCanvas.instance.currentShopkeeper.TrySellItem ( item.ID, shiftClick );
            else
                StoreCanvas.instance.currentShopkeeper.TrySellItem ( item.ID, 1 );
        }
        else if (BankCanvas.instance.isOpened)
        {
            int shiftClick = playerInventory.GetStackAtIndex ( inventoryIndex ).Amount;

            if (Input.GetKey ( KeyCode.LeftShift ))
                PlayerInventoryController.SendItemFromInventoryToBank ( item.ID, shiftClick );
            else
                PlayerInventoryController.SendItemFromInventoryToBank ( item.ID, 1 );
        }
        else
        {
            item.GetDefaultInteractionData ().onInteract?.Invoke ( inventoryIndex );
        }
    }

    public const float GLOBAL_ITEM_SELL_MODIFER = 0.55f;
    public const float GLOBAL_ITEM_BUY_MODIFER = 1.0f;

    public static float GetGlobalItemSellPrice (int itemID)
    {
        ItemBaseData item = null;

        if (GetItem ( itemID, out item ))
        {
            return GetGlobalItemSellPrice ( item );
        }
        else
        {
            return 0.0f;
        }
    }

    public static float GetGlobalItemSellPrice (ItemBaseData item)
    {
        return Mathf.FloorToInt ( ((float)item.BuyPrice * 0.75f) * GLOBAL_ITEM_SELL_MODIFER );
    }
}
