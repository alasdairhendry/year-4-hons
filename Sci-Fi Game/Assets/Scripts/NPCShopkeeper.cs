﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCShopkeeper : MonoBehaviour
{
    [SerializeField] private List<ShopItem> baseInventory = new List<ShopItem> ();

    public List<ShopItem> BaseInventory { get => baseInventory; protected set => baseInventory = value; }
    public NPC Npc { get; private set; }

    private float sameFactionModifier = 1.0f;
    private float differentFactionModifier = 0.25f;
    private float opposingFactionModifier = 0.5f;

    private void Awake ()
    {
        Npc = GetComponent<NPC> ();
    }

    public void OpenShop ()
    {
        StoreCanvas.instance.SetShopkeeper ( this );
        StoreCanvas.instance.Open ();

        if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == Npc.Character.cFaction.CurrentFaction.factionType)
        {
            MessageBox.AddMessage ( "Prices will not be inflated at this shop, as you are the same faction.", MessageBox.Type.Info );
        }
        else if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.opposingFaction.factionType == Npc.Character.cFaction.CurrentFaction.factionType)
        {
            if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Emerys)
                MessageBox.AddMessage ( "Prices will be inflated at this shop, but your Faction Specialisation minimises this.", MessageBox.Type.Warning );
            else
                MessageBox.AddMessage ( "Prices will be inflated drastically at this shop, as you are from opposing factions.", MessageBox.Type.Warning );
        }
        else
        {
            if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Emerys)
                MessageBox.AddMessage ( "Prices will not be inflated at this shop because of your Faction Specialisation.", MessageBox.Type.Info );
            else
                MessageBox.AddMessage ( "Prices will be inflated at this shop, as you are from different factions.", MessageBox.Type.Warning );
        }
    }

    public void TrySellItem (int itemID, int amount)
    {
        ItemBaseData item = ItemDatabase.GetItem ( itemID );

        if (!PlayerInventoryController.CanSellItemToShop ( itemID ))
            return;        

        int payout = amount * GetSellPrice ( itemID );

        if (EntityManager.instance.PlayerInventory.CheckCanRecieveItem ( 3, payout ))
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( itemID, amount );
            EntityManager.instance.PlayerInventory.AddCoins ( payout );

            SoundEffectManager.Play ( AudioClipAsset.CoinsRattle, AudioMixerGroup.SFX );
            MessageBox.AddMessage ( "You sell " + amount + " " + ItemDatabase.GetItem ( itemID ).Name + " for " + payout + " crowns from the shopkeeper.", MessageBox.Type.Info );
        }
        else
        {
            MessageBox.AddMessage ( "I need more inventory space to do this.", MessageBox.Type.Warning );
        }
    }

    public void TryBuyItem (int itemID)
    {
        if (baseInventory.Exists ( x => x.itemID == itemID ))
        {
            if (EntityManager.instance.PlayerInventory.CheckHasItemQuantity ( 3, GetBuyPrice ( itemID ) ))
            {
                int x = EntityManager.instance.PlayerInventory.AddItem ( itemID, 1 );

                if (x > 0)
                {
                    MessageBox.AddMessage ( "My inventory is too full to buy this item.", MessageBox.Type.Warning );
                    return;
                }
                else
                {
                    // TODO : Player purchase sound
                    SoundEffectManager.Play ( AudioClipAsset.CoinsRattle, AudioMixerGroup.SFX );
                    MessageBox.AddMessage ( "You buy " + ItemDatabase.GetItem ( itemID ).Name + " for " + GetBuyPrice ( itemID ) + " crowns from the shopkeeper.", MessageBox.Type.Info );
                    EntityManager.instance.PlayerInventory.RemoveCoins ( GetBuyPrice ( itemID ) );
                }
            }
            else
            {
                MessageBox.AddMessage ( "I don't have enough coins to buy this item.", MessageBox.Type.Warning );
            }
        }
        else
        {
            Debug.LogError ( "This shopkeeper doesnt sell item id " + itemID );
        }
    }

    public int GetSellPrice (int itemID)
    {
        float baseInventorySellPriceModifier = 1.0f;

        if (baseInventory.Exists ( x => x.itemID == itemID ))
            baseInventorySellPriceModifier = baseInventory.Find ( x => x.itemID == itemID ).sellPriceModifier;

        Debug.Log ( "Sell price modifier " + baseInventorySellPriceModifier );

        float factionSellPriceModifier = sameFactionModifier;

        if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == Npc.Character.cFaction.CurrentFaction.factionType)
        {
            Debug.Log ( "same faction" );
            factionSellPriceModifier = sameFactionModifier;
        }
        else if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.opposingFaction.factionType == Npc.Character.cFaction.CurrentFaction.factionType)
        {
            Debug.Log ( "opposing faction" );
            factionSellPriceModifier = 1 - opposingFactionModifier;
        }
        else
        {
            Debug.Log ( "different faction" );
            factionSellPriceModifier = 1 - differentFactionModifier;
        }

        Debug.Log ( "Faction price modifier " + factionSellPriceModifier );

        if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Emerys)
        {
            Debug.Log ( "Faction is emerys - Faction modifier is now " + factionSellPriceModifier );
            factionSellPriceModifier += 0.25f;
            factionSellPriceModifier = Mathf.Clamp01 ( factionSellPriceModifier );
        }

        Debug.Log ( "Final outcome " + Mathf.FloorToInt ( ItemDatabase.GetGlobalItemSellPrice ( itemID ) * baseInventorySellPriceModifier * factionSellPriceModifier ) );
        //return Mathf.FloorToInt ( Mathf.FloorToInt ( ItemDatabase.GetItem ( itemID ).BuyPrice * 0.75f ) * baseInventorySellPriceModifier * factionSellPriceModifier * ItemDatabase.GLOBAL_ITEM_SELL_MODIFER );
        return Mathf.FloorToInt ( ItemDatabase.GetGlobalItemSellPrice ( itemID ) * baseInventorySellPriceModifier * factionSellPriceModifier );
    }

    public int GetBuyPrice (int itemID)
    {
        float factionBuyPriceModifier = sameFactionModifier;

        if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == Npc.Character.cFaction.CurrentFaction.factionType)
        {
            factionBuyPriceModifier = sameFactionModifier;
        }
        else if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.opposingFaction.factionType == Npc.Character.cFaction.CurrentFaction.factionType)
        {
            factionBuyPriceModifier = 1 + opposingFactionModifier;
        }
        else
        {
            factionBuyPriceModifier = 1 + differentFactionModifier;
        }

        if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Emerys)
        {
            factionBuyPriceModifier -= 0.25f;
            if (factionBuyPriceModifier < 1.0f) factionBuyPriceModifier = 1.0f;
        }

        return Mathf.FloorToInt ( ItemDatabase.GetItem ( itemID ).BuyPrice * baseInventory.Find ( x => x.itemID == itemID ).buyPriceModifier * factionBuyPriceModifier * ItemDatabase.GLOBAL_ITEM_BUY_MODIFER );
    }


    [System.Serializable]
    public class ShopItem
    {
        [ItemID] public int itemID = 0;
        [Range ( 0.0f, 10.0f )] public float sellPriceModifier = 1.0f;
        [Range ( 0.0f, 10.0f )] public float buyPriceModifier = 1.0f;
    }
}
