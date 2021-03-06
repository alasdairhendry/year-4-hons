﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemCategory
{
    Consumable,
    Weapon,
    Gear,
    Ingredient,
    Attachment,
    Misc,
    Currency,
    Tool,
    Book,
    Key
}

public abstract class ItemBaseData
{
    // The ID of this item - This is a unique integer and will never be changed
    public int ID { get; set; }                   
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    // The category this item belongs to
    public ItemCategory category { get; protected set; }
    // Is the item able to be sold to shops?
    public bool IsSellable { get; protected set; }
    // Can this item be stacked on top of itself in an inventory?
    public bool IsStackable { get; protected set; }

    // Does this item relate to any quest IDs? If so, it is considered a quest item
    public string[] RelatedQuestIDs { get; protected set; }
    // Relies on RelatedQuestIDs to determine if this is a quest item or not
    public bool IsQuestItem { get { return RelatedQuestIDs.Length > 0; } }
    // A baseline price that the player can buy this item for ( this may be overridden by a merchants price modifier ) (( The sell price will typically be 0.75f of the buy price ))
    public int BuyPrice { get; protected set; }
    // A sprite to display in the game UI 
    public Sprite Sprite { get; protected set; }            

    protected Dictionary<InventoryInteractionData.InteractType, InventoryInteractionData> interactionData { get; set; } = new Dictionary<InventoryInteractionData.InteractType, InventoryInteractionData> ();
    protected InventoryInteractionData.InteractType defaultInteractionData = InventoryInteractionData.InteractType.Use;

    public ItemBaseData(int ID)
    {
        this.ID = ID;
        AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.DoNothing, (inventoryIndex) => { } ), true );
        AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Drop, (inventoryIndex) =>
        {
            int amount = EntityManager.instance.PlayerInventory.GetStackAtIndex ( inventoryIndex ).Amount;
            EntityManager.instance.PlayerInventory.RemoveItem ( ID, amount );
            SoundEffectManager.Play ( EntityManager.instance.dropItemSoundEffects.GetRandom (), AudioMixerGroup.SFX );
        } ), false );
    }

    protected void FetchSprite ()
    {
        Sprite = Resources.Load<Sprite> ( "Items/Sprites/" + ID) as Sprite;
        if(Sprite == null)
        {
            Debug.LogError ( Name + " has no sprite [ID: " + ID + "]" );
        }
    }

    public void AddInteractionData (InventoryInteractionData data, bool setAsDefault = false)
    {
        if (interactionData.ContainsKey ( data.interactType ))
        {
            Debug.LogError ( "Item " + Name + " already contains interaction type " + data.interactType.ToString () );
        }
        else
        {
            interactionData.Add ( data.interactType, data );
            if (setAsDefault)
                defaultInteractionData = data.interactType;
        }
    }

    public void RemoveInteractionData (InventoryInteractionData.InteractType interactionType)
    {
        if (interactionData.ContainsKey ( interactionType ))
        {
            if (defaultInteractionData == interactionType)
                defaultInteractionData = interactionData.First ( x => x.Value != null ).Key;

            interactionData.Remove ( interactionType );
        }
        else
        {
            Debug.LogError ( "Item " + Name + " does not contain interaction type " + interactionType.ToString () );
        }
    }

    public InventoryInteractionData GetDefaultInteractionData ()
    {
        if (interactionData.ContainsKey ( defaultInteractionData ))
        {
            return interactionData[defaultInteractionData];
        }

        return interactionData[InventoryInteractionData.InteractType.Use];
    }

    public List<InventoryInteractionData> GetAllInteractionData ()
    {
        List<InventoryInteractionData> data = new List<InventoryInteractionData> ();

        if (interactionData.ContainsKey ( defaultInteractionData ))
        {
            data.Add ( interactionData[defaultInteractionData] );
        }

        string[] enumNames = System.Enum.GetNames ( typeof ( InventoryInteractionData.InteractType ) );
        InventoryInteractionData.InteractType typedName = InventoryInteractionData.InteractType.Use;

        for (int i = 0; i < enumNames.Length; i++)
        {
            if (enumNames[i] == defaultInteractionData.ToString ())
            {
                continue;
            }

            typedName = (InventoryInteractionData.InteractType)System.Enum.Parse ( typeof ( InventoryInteractionData.InteractType ), enumNames[i] );

            if (interactionData.ContainsKey ( typedName ))
            {
                data.Add ( interactionData[typedName] );
            }
        }

        return data;
    }

    public virtual void OnShopInteract () { }
}

public abstract class ItemBaseDataBook : ItemBaseData
{
    protected string bookTitle { get; private set; } = "";
    protected string bookText { get; private set; } = "";

    public ItemBaseDataBook (int ID) : base ( ID )
    {
        this.category = ItemCategory.Book;
        AddInteractionData ( new InventoryInteractionData ( InventoryInteractionData.InteractType.Read, (inventoryIndex) => { BookDisplayCanvas.instance.SetBook ( bookTitle, bookText ); BookDisplayCanvas.instance.Open (); } ), true );
    }

    protected void SetTitle(string text)
    {
        bookTitle = text;
    }

    protected void InsertChapter (string text)
    {
        text = text.Insert ( 0, "<align=center>" );
        text += "</align>";
        bookText += text;
    }

    protected void InsertHeader (string text)
    {
        text = text.Insert ( 0, "<b><size=115%>" );
        text += "</size></b>";
        bookText += text;
    }

    protected void InsertBreak (int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            this.bookText += "\n";
        }
    }

    protected void InsertParagraph(string text)
    {
        bookText += text;
    }
}


public class InventoryInteractionData
{
    public enum InteractType { Use, Open, Read, Empty, Fill, Drink, Eat, Drop, Equip, DoNothing, Attach }
    public InteractType interactType = InteractType.Use;

    // Takes in an int for the inventory index
    public System.Action<int> onInteract;

    public InventoryInteractionData (InteractType interactType, System.Action<int> onInteract)
    {
        this.interactType = interactType;
        this.onInteract = onInteract;
    }
}

/*
    public ItemData_ (int ID) : base ( ID )
    {
        base.Name = "";
        base.Description = "";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 10;
        base.RelatedQuestIDs = new int[] { };

        base.sellPrice = 0;
        base.costPrice = 0;
    }
*/