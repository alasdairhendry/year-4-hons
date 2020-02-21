using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TooltipItemUI tooltipItem;
    public int inventoryIndex { get; protected set; } = -1;
    public int ItemID { get; protected set; }
    public int ItemCost { get; protected set; }
    
    public void Disable ()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color ( 0, 0, 0, 0 );
        this.ItemID = -1;
        this.ItemCost = -1;
        tooltipItem.SetTooltipAction ( null );
    }

    public void SetContent (int itemID, int itemCost)
    {
        this.ItemID = itemID;
        this.ItemCost = itemCost;

        itemIcon.sprite = ItemDatabase.GetItem ( itemID ).Sprite;
        itemIcon.color = new Color ( 1, 1, 1, 1 );

        tooltipItem.SetTooltipAction ( () =>
        {
            return ItemDatabase.GetItem ( itemID ).Name
            + "\n"
            + ColourHelper.TagColour ( ColourHelper.TagSize ( ItemCost.ToString ( "0" ) + " crowns", 80.0f ), ColourDescription.OffWhiteText );
        } );

        tooltipItem.SetTooltipAction ( () =>
        {
            return "Buy " + ColourHelper.TagColour ( ItemDatabase.GetItem ( itemID ).Name
            + "\n"
            + ColourHelper.TagSize ( ItemDatabase.GetItem ( itemID ).category
            + "\n"
            + ItemCost.ToString ( "0" ) + " crowns", 80.0f ), ColourDescription.OffWhiteText );
        } );
    }

    public void OnClick ()
    {
        StoreCanvas.instance.currentShopkeeper.TryBuyItem ( ItemID );
    }
}
