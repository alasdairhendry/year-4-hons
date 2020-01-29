using System.Collections;
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
        contentPanel.SetActive ( false );
        this.ItemID = -1;
        this.ItemAmount = -1;
        tooltipItem.SetTooltipAction ( null );
    }

    public void SetContent (Sprite sprite, int itemID, int amount)
    {
        contentPanel.SetActive ( true );
        this.ItemID = itemID;
        this.ItemAmount = amount;

        itemIcon.sprite = sprite;

        itemAmountPanel.SetActive ( true );
        itemAmountText.text = this.ItemAmount.ToString ();
        tooltipItem.SetTooltipAction ( () => { return "Take " + ColourHelper.TagColour ( ItemDatabase.GetItem ( itemID ).Name, ColourDescription.OffWhiteText ) + "\n" + ColourHelper.TagSize ( ItemDatabase.GetItem ( itemID ).Description, 75.0f ); } );
    }

    void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
    {
        DragHandler.OnBeginDrag ( inventoryIndex, ItemID, ItemAmount, DragHandler.Master.ItemContainer, contentPanel.transform );
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
