using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private DragHandler.Master panelDragType = DragHandler.Master.PlayerInventory;
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject itemAmountPanel;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private TooltipItemUI tooltipItem;
    [SerializeField] private CooldownUIPanel cooldownPanel;
    public int inventoryIndex { get; protected set; } = -1;
    public int ItemID { get; protected set; }
    public int ItemAmount { get; protected set; }

    private void Start ()
    {
        GameManager.instance.OnGlobalCooldownStateChange += OnCooldownStateChange;
    }

    private void OnCooldownStateChange (bool state)
    {
        if (state)
        {
            if (ItemDatabase.ItemExists ( ItemID ))
            {
                if (ItemDatabase.GetItem ( ItemID ).category == ItemCategory.Consumable)
                    cooldownPanel?.SetActive ( state );
            }
        }
        else
        {
            cooldownPanel?.SetActive ( state );
        }
    }

    public void SetInventoryIndex(int index)
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

        if (sprite == null)
        {
            itemIcon.color = new Color ( 0.0f, 0.0f, 0.0f, 0.0f );
        }
        else
        {
            itemIcon.color = new Color ( 1.0f, 1.0f, 1.0f, 1.0f );
        }

        itemIcon.sprite = sprite;

        itemAmountPanel.SetActive ( true );
        itemAmountText.text = this.ItemAmount.ToString ();
        tooltipItem.SetTooltipAction ( () =>
        {
            string s = "";

            if (panelDragType == DragHandler.Master.PlayerInventory)
            {
                if (!string.IsNullOrEmpty ( ItemDatabase.GetInteractionPrefix ( itemID ) ))
                    s += ColourHelper.TagColour ( ItemDatabase.GetInteractionPrefix ( itemID ), ColourDescription.DarkYellowText );
            }
            else if(panelDragType == DragHandler.Master.Bank)
            {
                if (Input.GetKey ( KeyCode.LeftShift ))
                    s += "Take All ";
                else
                    s += "Take ";
            }

            s += ColourHelper.TagColour ( ItemDatabase.GetItem ( itemID ).Name, ColourDescription.OffWhiteText );

            s += ColourHelper.TagColourSize ( "\n" + ItemDatabase.GetGlobalItemSellPrice ( itemID ) + " Crowns", ColourDescription.DarkYellowText, 80.0f );
            s += ColourHelper.TagColourSize ( "\n" + ItemDatabase.GetItem ( itemID ).category.ToString (), ColourDescription.OffWhiteText, 80.0f );
            s += ColourHelper.TagColourSize ( "\n" + ItemDatabase.GetItem ( itemID ).Description, ColourDescription.OffWhiteText, 80.0f );

            if (ItemDatabase.ItemExists ( itemID ) && ItemDatabase.GetItem ( itemID ).IsSellable && StoreCanvas.instance.isOpened && StoreCanvas.instance.currentShopkeeper != null)
                s += ColourHelper.TagColourSize ( "\n\n" + StoreCanvas.instance.currentShopkeeper.Npc.NpcData.NpcName + " will buy this for " + ColourHelper.TagColour ( StoreCanvas.instance.GetItemSellPriceFromCurrentShopkeeper ( itemID ).ToString (), ColourDescription.DarkYellowText ) + " crowns", ColourDescription.OffWhiteText, 80.0f );

            return s;
        } );
    }

    void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
    {
        if (ItemDatabase.ItemExists ( ItemID ))
            DragHandler.OnBeginDrag ( inventoryIndex, ItemID, ItemAmount, panelDragType, contentPanel.transform, ItemDatabase.GetItem ( ItemID ).Sprite );
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
        DragHandler.OnDrop ( inventoryIndex, panelDragType );
    }

    void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
    {
        if (ItemID < 0) return;

        if (panelDragType == DragHandler.Master.PlayerInventory)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                ItemDatabase.InvokeInventoryDefaultAction ( ItemID, inventoryIndex );
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                DropdownMenuCanvas.instance.Open ( ItemDatabase.GetItem ( ItemID ).GetAllInteractionData (), ItemID, inventoryIndex );
            }
        }
        else if (panelDragType == DragHandler.Master.Bank)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                int shiftClick = ItemAmount;

                if (Input.GetKey ( KeyCode.LeftShift ))
                    PlayerInventoryController.SendItemFromBankToInventory ( ItemID, shiftClick );
                else
                    PlayerInventoryController.SendItemFromBankToInventory ( ItemID, 1 );
            }
        }
        else
        {
            Debug.LogError ( "Incorrect panel type for interaction" );
        }
    }
}
