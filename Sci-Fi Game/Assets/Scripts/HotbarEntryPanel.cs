﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HotbarEntryPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public int index { get; protected set; } = 0;
    public int currentItemID { get; protected set; } = -1;
    [SerializeField] private Image image;
    [SerializeField] private CooldownUIPanel cooldownPanel;

    public void Initialise (int index)
    {
        this.index = index;
        GetComponent<Button> ().onClick.AddListener ( () => { Interact (); } );
        RemoveItem ();
        GameManager.instance.OnGlobalCooldownStateChange += OnCooldownStateChange;
    }

    private void OnCooldownStateChange (bool state)
    {
        if (state)
        {
            if (ItemDatabase.ItemExists ( currentItemID ))
            {
                if (ItemDatabase.GetItem ( currentItemID ).category == ItemCategory.Consumable)
                    cooldownPanel?.SetActive ( state );
            }
        }
        else
        {
            cooldownPanel?.SetActive ( state );
        }
    }
    
    public void SetItem (int ID)
    {
        if (ID < 0) { RemoveItem (); return; }

        currentItemID = ID;
        image.sprite = ItemDatabase.GetItem ( currentItemID ).Sprite;
        image.enabled = true;
    }

    public void RemoveItem ()
    {
        currentItemID = -1;
        image.sprite = null;
        image.enabled = false;
    }

    public void Interact ()
    {
        if (currentItemID < 0)
        {
            MessageBox.AddMessage ( "No hotkey assigned.", MessageBox.Type.Error );
            return;
        }

        if (EntityManager.instance.PlayerInventory.CheckHasItem ( currentItemID ))
        {
            ItemDatabase.InvokeInventoryDefaultAction ( currentItemID, EntityManager.instance.PlayerInventory.GetIndexOfItem ( currentItemID ) );
        }
        else
        {
            MessageBox.AddMessage ( "I need that item in my inventory.", MessageBox.Type.Error );
        }
    }

    void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
    {
        if (ItemDatabase.ItemExists ( currentItemID ))
            DragHandler.OnBeginDrag ( index, currentItemID, 0, DragHandler.Master.Hotbar, image.transform, ItemDatabase.GetItem ( currentItemID ).Sprite );
    }

    void IDragHandler.OnDrag (PointerEventData eventData)
    {
        DragHandler.OnDrag ();
    }

    void IEndDragHandler.OnEndDrag (PointerEventData eventData)
    {
        DragHandler.OnEndDrag ();
    }

    void IDropHandler.OnDrop (PointerEventData eventData)
    {
        DragHandler.OnDrop ( index, DragHandler.Master.Hotbar );
    }
}
