﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemContainerPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject itemAmountPanel;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private Button panelButton;
    [SerializeField] private TooltipItemUI tooltipItem;
    public Button PanelButton { get => panelButton; }
    public int inventoryIndex { get; protected set; } = -1;
    public int ItemID { get; protected set; }
    public int ItemAmount { get; protected set; }

    public void SetInventoryIndex (int index)
    {
        inventoryIndex = index;
    }

    public void Disable ()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color ( 0, 0, 0, 0 );
        itemAmountText.text = "";
        this.ItemID = -1;
        this.ItemAmount = -1;
        tooltipItem.SetTooltipAction ( null );
    }

    public void SetContent (Sprite sprite, int itemID, int amount)
    {
        this.ItemID = itemID;
        this.ItemAmount = amount;

        itemIcon.sprite = sprite;
        itemIcon.color = new Color ( 1, 1, 1, 1 );

        itemAmountPanel.SetActive ( true );
        itemAmountText.text = this.ItemAmount.ToString ();
        tooltipItem.SetTooltipAction ( () =>
        {
            string s = "Take ";
            if (Input.GetKey ( KeyCode.LeftShift )) s = "Take All ";

            s += ColourHelper.TagColour ( ItemDatabase.GetItem ( itemID ).Name, ColourDescription.OffWhiteText );

            s += ColourHelper.TagColourSize ( "\n" + ItemDatabase.GetGlobalItemSellPrice ( itemID ) + " Crowns", ColourDescription.DarkYellowText, 80.0f );
            s += ColourHelper.TagColourSize ( "\n" + ItemDatabase.GetItem ( itemID ).category.ToString (), ColourDescription.OffWhiteText, 80.0f );
            s += ColourHelper.TagColourSize ( "\n" + ItemDatabase.GetItem ( itemID ).Description, ColourDescription.OffWhiteText, 80.0f );

            return s;
        }
        );
    }

    void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
    {
        if (ItemDatabase.ItemExists ( ItemID ))
            DragHandler.OnBeginDrag ( inventoryIndex, ItemID, ItemAmount, DragHandler.Master.ItemContainer, contentPanel.transform, ItemDatabase.GetItem ( ItemID ).Sprite );
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
        DragHandler.OnDrop ( inventoryIndex, DragHandler.Master.ItemContainer );
    }
}
