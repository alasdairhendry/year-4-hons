using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemStaticPanel : MonoBehaviour
{
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMPro.TextMeshProUGUI itemAmountText;
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

    public void SetContent (int itemID, int amount)
    {
        this.ItemID = itemID;
        this.ItemAmount = amount;

        itemIcon.sprite = ItemDatabase.GetItem ( itemID ).Sprite;
        itemIcon.color = new Color ( 1, 1, 1, 1 );

        itemAmountText.text = this.ItemAmount.ToString ();
        tooltipItem.SetTooltipAction ( () =>
        {
            return ColourHelper.TagColour ( ItemDatabase.GetItem ( itemID ).Name, ColourDescription.OffWhiteText ) + "\n" + ColourHelper.TagSize ( ItemDatabase.GetItem ( itemID ).Description, 75.0f );
        }
        );
    }
}
