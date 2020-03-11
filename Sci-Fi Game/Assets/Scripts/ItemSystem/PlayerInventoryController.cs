using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInventoryController
{
    #region Removing From Inventory
    public static void DropItemToFloorFromInventory (int id, int amount)
    {
        ItemBaseData item = null;

        if (ItemDatabase.GetItem ( id, out item ))
        {
            if (item.IsQuestItem)
            {
                MessageBox.AddMessage ( "Cannot remove a quest item from your inventory", MessageBox.Type.Error );
            }
            else
            {
                EntityManager.instance.PlayerInventory.RemoveItem ( id, amount );
                SoundEffectManager.Play ( EntityManager.instance.dropItemSoundEffects.GetRandom (), AudioMixerGroup.SFX );
            }
        }
    }

    public static void SendItemFromInventoryToContainer (int id, int amount)
    {
        ItemBaseData item = null;

        if (ItemDatabase.GetItem ( id, out item ))
        {
            if (item.IsQuestItem)
            {
                MessageBox.AddMessage ( "Cannot remove a quest item from your inventory", MessageBox.Type.Error );
            }
            else if (ItemContainerCanvas.instance.targetInventory.canRecieveItems == false)
            {
                MessageBox.AddMessage ( "I can't put that in there.", MessageBox.Type.Error );
            }
            else
            {
                EntityManager.instance.PlayerInventory.SendTo ( ItemContainerCanvas.instance.targetInventory, id, amount, true );
            }
        }
    }

    public static void SendItemFromInventoryToBank (int id, int amount)
    {
        ItemBaseData item = null;

        if (ItemDatabase.GetItem ( id, out item ))
        {
            if (item.IsQuestItem)
            {
                MessageBox.AddMessage ( "Cannot remove a quest item from your inventory", MessageBox.Type.Error );
            }
            else if (EntityManager.instance.PlayerBankInventory.canRecieveItems == false)
            {
                MessageBox.AddMessage ( "I can't put that in there.", MessageBox.Type.Error );
            }
            else
            {
                EntityManager.instance.PlayerInventory.SendTo ( EntityManager.instance.PlayerBankInventory, id, amount, true );
            }
        }
    }

    public static bool CanSellItemToShop(int id)
    {
        ItemBaseData item = null;

        if (ItemDatabase.GetItem ( id, out item ))
        {
            if (!item.IsSellable || item.IsQuestItem)
            {
                MessageBox.AddMessage ( "This item is not sellable", MessageBox.Type.Error );
                return false;
            }
            else
            {
                return true;
            }
        }

        return false;
    }
    #endregion

    #region Adding To Inventory
    public static void SendItemFromContainerToInventory (int id, int amount)
    {
        ItemContainerCanvas.instance.targetInventory.SendTo ( EntityManager.instance.PlayerInventory, id, amount, true );
    }

    public static void SendItemFromBankToInventory (int id, int amount)
    {
        EntityManager.instance.PlayerBankInventory.SendTo ( EntityManager.instance.PlayerInventory, id, amount, true );
    }
    #endregion

    #region Containers
    public static void DropItemToFloorFromContainer (int id, int amount)
    {
        ItemContainerCanvas.instance.targetInventory.RemoveItem ( id, amount );
        SoundEffectManager.Play ( EntityManager.instance.dropItemSoundEffects.GetRandom (), AudioMixerGroup.SFX );
    }
    #endregion

    #region Hotbar
    public static void AddHotbarEntry (int dragItemID, int toIndex)
    {
        HotbarCanvas.instance.TryAddItem ( dragItemID, toIndex );
        SoundEffectManager.Play ( EntityManager.instance.dropItemSoundEffects.GetRandom (), AudioMixerGroup.SFX );
    }

    public static void SwitchHotbarIndices(int fromIndex, int toIndex)
    {
        HotbarCanvas.instance.TrySwitchItems ( fromIndex, toIndex );
    }

    public static void DropHotbarEntryToFloor (int dragItemID, int fromIndex)
    {
        HotbarCanvas.instance.TryRemoveItem ( dragItemID, fromIndex );
        SoundEffectManager.Play ( EntityManager.instance.dropItemSoundEffects.GetRandom (), AudioMixerGroup.SFX );
    }
    #endregion
}
