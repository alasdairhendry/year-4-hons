﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public int stackCapacity { get; protected set; } = 12;
    //public bool usesMaxStacks { get; protected set; } = false;
    public bool stackEveryItem { get; protected set; } = true;
    public bool canRecieveItems = true;
    public List<ItemStack> stacks = new List<ItemStack> ();

    public bool IsEmpty { get => stacks.Count <= 0; }    

    public ItemStack GetStackAtIndex(int index)
    {
        if (index < 0) return null;
        if (index >= stacks.Count) return null;
        return stacks[index];
    }

    public int GetStackCount { get => stacks.Count; }

    private System.Action<int, int> OnItemAdded;
    private System.Action<int, int> OnItemRemoved;

    public void AddCoins (int amount)
    {
        AddItem ( 3, amount );
    }

    public void ClearInventoryStacks ()
    {
        stacks.Clear ();
    }

    public void RemoveCoins (int amount)
    {
        RemoveItem ( 3, amount );
    }

    public bool CheckHasItem (int id)
    {
        for (int i = 0; i < stacks.Count; i++)
        {
            if (stacks[i].ID == id) return true;
        }
        return false;
    }

    public bool CheckHasItemQuantity (int id, int amount)
    {
        for (int i = 0; i < stacks.Count; i++)
        {
            if (stacks[i].ID == id)
            {
                amount -= stacks[i].Amount;
                if (amount <= 0) return true;
            }
        }
        return false;
    }

    public int GetQuantityOfItem(int id)
    {
        int amount = 0;

        for (int i = 0; i < stacks.Count; i++)
        {
            if (stacks[i].ID == id) amount += stacks[i].Amount;
        }

        return amount;
    }

    public bool CheckCanRecieveItem(int id, int amount)
    {
        ItemBaseData item = ItemDatabase.GetItem ( id );

        if(item.IsStackable)
        {
            if (GetStackCount + 1 <= stackCapacity || CheckHasItem(id))
                return true;
        }
        else
        {
            if (GetStackCount + amount <= stackCapacity)
                return true;
        }

        return false;
    }

    public int GetIndexOfItem(int id)
    {
        if (CheckHasItem ( id ))
        {
            return stacks.FindIndex ( x => x.ID == id );
        }
        else
        {
            return -1;
        }
    }

    private System.Action OnInventoryChanged;

    public Inventory (int stackCapacity, bool stackEveryItem, bool canRecieveItems)
    {
        this.stackCapacity = stackCapacity;
        this.stackEveryItem = stackEveryItem;
        //this.usesMaxStacks = usesMaxStacks;
        this.canRecieveItems = canRecieveItems;
    }

    /// <summary>
    /// Adds the amount of item ID to the inventory. If there is not enough room to add the desired amount, it will return how many were not added.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    /// <returns></returns>

    public void AddMultipleItems (List<ItemStack> items, bool bypassRecieveCriteria = false)
    {
        for (int i = 0; i < items.Count; i++)
        {
            AddItem ( items[i].ID, items[i].Amount, bypassRecieveCriteria );
        }
    }

    public int AddItem (int id, int amount = 1, bool bypassRecieveCriteria = false, int index = -1)
    {
        if (amount <= 0) { Debug.LogError ( "Adding a value of " + amount ); return amount; }
        if (canRecieveItems == false && bypassRecieveCriteria == false) { Debug.LogError ( "This inventory cannot recieve items" ); return amount; }

        int amountNotAdded = 0;/* AddItemRecursively ( id, amount, index );*/
        ItemBaseData item = null;

        if(ItemDatabase.GetItem(id, out item ))
        {
            if (stackEveryItem)
            {
                int currentIndex = GetIndexOfItem ( id );

                if (currentIndex >= 0)
                {
                    stacks[currentIndex].Amount += amount;
                    amountNotAdded = 0;
                }
                else
                {
                    if (stacks.Count >= stackCapacity)
                    {
                        // Inventory is full
                        amountNotAdded = amount;
                    }
                    else
                    {
                        stacks.Add ( new ItemStack () { ID = id, Amount = amount } );
                        amountNotAdded = 0;
                    }
                }
            }
            else
            {
                if (item.IsStackable)
                {
                    int currentIndex = GetIndexOfItem ( id );

                    if (currentIndex >= 0)
                    {
                        stacks[currentIndex].Amount += amount;
                        amountNotAdded = 0;
                    }
                    else
                    {
                        if (stacks.Count >= stackCapacity)
                        {
                            // Inventory is full
                            amountNotAdded = amount;
                        }
                        else
                        {
                            stacks.Add ( new ItemStack () { ID = id, Amount = amount } );
                            amountNotAdded = 0;
                        }
                    }
                }
                else
                {
                    int amountToAdd = Mathf.Min ( amount, stackCapacity - stacks.Count );
                    amountNotAdded = amount - amountToAdd;

                    for (int i = 0; i < amountToAdd; i++)
                    {
                        stacks.Add ( new ItemStack () { ID = id, Amount = 1 } );
                    }
                }
            }
            
        }

        //Debug.Log ( string.Format ( "Requested to add {0} {1}. {2} were added and {3} were returned to original inventory", amount, ItemDatabase.GetItem ( id ).Name, (amount - amountNotAdded), amountNotAdded ) );
        OnItemAdded?.Invoke ( id, (amount - amountNotAdded) );
        OnInventoryChanged?.Invoke ();

        return amountNotAdded;
    }

    //List<ItemStack> existingStacks = new List<ItemStack> ();

    //private int AddItemRecursively (int id, int amount = 1, int index = -1)
    //{
    //    ItemBaseData item = null;

    //    if (ItemDatabase.GetItem ( id, out item ))
    //    {
    //        while (amount > 0)
    //        {
    //            existingStacks.Clear ();

    //            for (int i = 0; i < stacks.Count; i++)
    //            {
    //                if (stacks[i].ID == id)
    //                    existingStacks.Add ( stacks[i] );
    //            }

    //            for (int i = 0; i < existingStacks.Count; i++)
    //            {
    //                if (!stackEveryItem)
    //                    if (!item.IsStackable)
    //                        continue;

    //                //int amountToAdd = Mathf.Min ( ((usesMaxStacks) ? item.MaxStack - existingStacks[i].Amount : int.MaxValue), amount );
    //                int amountToAdd = Mathf.Min ()
    //                existingStacks[i].Amount += amountToAdd;
    //                amount -= amountToAdd;

    //                if (amount <= 0)
    //                {
    //                    return 0;
    //                }
    //                else
    //                {
    //                    continue;
    //                }
    //            }

    //            if (stacks.Count >= stackCapacity)
    //            {
    //                // Inventory is full
    //                return amount;
    //            }
    //            else
    //            {
    //                // Can be added
    //                int amountToAdd = Mathf.Min ( ((usesMaxStacks) ? item.MaxStack : int.MaxValue), amount );

    //                if (index < 0)
    //                    stacks.Add ( new ItemStack () { ID = id, Amount = amountToAdd } );
    //                else
    //                    stacks.Insert ( index, new ItemStack () { ID = id, Amount = amountToAdd } );

    //                amount -= amountToAdd;

    //                if (amount <= 0)
    //                {
    //                    return 0;
    //                }
    //                else
    //                {
    //                    continue;
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.LogError ( "Item [" + id + "] does not exist" );
    //        return amount;
    //    }

    //    Debug.LogError ( "NO!" );
    //    return 0;
    //}

    /// <summary>
    /// Removes the amount of item ID from the inventory. If there is not enough available to remove, it will return how many were not removed.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int RemoveItem (int id, int amount = 1)
    {
        if (amount <= 0) { Debug.LogError ( "Removing a value of " + amount ); return 0; }
        int amountNotRemoved = 0;

        ItemBaseData item = null;

        if (ItemDatabase.GetItem ( id, out item ))
        {
            if (stackEveryItem)
            {
                int currentIndex = GetIndexOfItem ( id );

                if (currentIndex >= 0)
                {
                    int amountToRemove = Mathf.Min ( stacks[currentIndex].Amount, amount );
                    stacks[currentIndex].Amount -= amountToRemove;
                    amountNotRemoved = amount - amountToRemove;
                }
                else
                {
                    amountNotRemoved = amount;
                }
            }
            else
            {
                if (item.IsStackable)
                {
                    int currentIndex = GetIndexOfItem ( id );

                    if (currentIndex >= 0)
                    {
                        int amountToRemove = Mathf.Min ( stacks[currentIndex].Amount, amount );
                        stacks[currentIndex].Amount -= amountToRemove;
                        amountNotRemoved = amount - amountToRemove;
                    }
                    else
                    {
                        amountNotRemoved = amount;
                    }
                }
                else
                {
                    int amountRemoved = 0;

                    for (int i = 0; i < stacks.Count; i++)
                    {
                        if (stacks[i].ID == id)
                        {
                            amountRemoved++;
                            stacks[i].Amount--;
                            if (amountRemoved >= amount)
                                break;
                        }
                    }

                    amountNotRemoved = amount - amountRemoved;
                }
            }
        }

        RemoveEmptyStacks ();

        //Debug.Log ( string.Format ( "Requested to remove {0} {1}. {2} were removed and {3} were returned to original inventory", amount, ItemDatabase.GetItem ( id ).Name, (amount - amountNotRemoved), amountNotRemoved ) );
        OnItemRemoved?.Invoke ( id, (amount - amountNotRemoved) );
        OnInventoryChanged?.Invoke ();

        return amountNotRemoved;
    }

    //private int RemoveItemRecursively (int id, int amount = 1)
    //{
    //    ItemBaseData item = null;

    //    if (ItemDatabase.GetItem ( id, out item ))
    //    {
    //        /*List<Stack> */existingStacks = stacks.Where ( x => x.ID == id ).OrderBy ( x => x.Amount ).ToList ();

    //        for (int i = 0; i < existingStacks.Count; i++)
    //        {
    //            int amountToRemove = Mathf.Min ( existingStacks[i].Amount, amount );
    //            existingStacks[i].Amount -= amountToRemove;
    //            amount -= amountToRemove;

    //            if (amount > 0)
    //            {
    //                RemoveEmptyStacks ();
    //                return RemoveItemRecursively ( id, amount );
    //            }
    //            else
    //            {
    //                RemoveEmptyStacks ();
    //                return 0;
    //            }
    //        }

    //        RemoveEmptyStacks ();
    //        return amount;
    //    }
    //    else
    //    {
    //        Debug.LogError ( "Item [" + id + "] does not exist" );
    //        return amount;
    //    }
    //}

    private void RemoveEmptyStacks ()
    {
        for (int x = stacks.Count - 1; x >= 0; x--)
        {
            if (stacks[x].Amount <= 0)
            {
                stacks.RemoveAt ( x );
            }
        }

        OnInventoryChanged?.Invoke ();
    }

    public void SwitchIndices(int indexA, int indexB)
    {
        if (indexA >= stacks.Count || indexB >= stacks.Count) return;
        if (indexA < 0 || indexB < 0) return;

        ItemStack stackA = stacks[indexA];

        stacks[indexA] = stacks[indexB];
        stacks[indexB] = stackA;

        OnInventoryChanged?.Invoke ();
        SoundEffectManager.Play ( AudioClipAsset.InventoryUpdated, AudioMixerGroup.SFX );
    }

    public void RegisterItemAdded (System.Action<int, int> action) { OnItemAdded += action; }

    public void UnregisterItemAdded (System.Action<int, int> action) { OnItemAdded -= action; }

    public void RegisterItemRemoved (System.Action<int, int> action) { OnItemRemoved += action; }

    public void UnregisterItemRemoved (System.Action<int, int> action) { OnItemRemoved -= action; }

    public void RegisterInventoryChanged (System.Action action) { OnInventoryChanged += action; }

    public void UnregisterInventoryChanged (System.Action action) { OnInventoryChanged -= action; }

    [System.Serializable]
    public class ItemStack
    {
        public int ID;
        public int Amount;
    }
}
