using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GearUIEntrySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private GearSlot slot = GearSlot.Weapon;
    [SerializeField] private TooltipItemUI tooltip;
    public int currentItemID { get; protected set; } = -1;

    private void Start ()
    {
        tooltip.SetTooltipAction ( () =>
        {
            if (currentItemID == -1)
            {
                return slot.ToString () + " Slot";
            }
            else
            {
                return "Unequip " + ColourHelper.TagColour ( ItemDatabase.GetItem ( currentItemID ).Name, ColourDescription.OffWhiteText );
            }
        } );
    }

    public void SetItem (int itemID)
    {
        if (itemID < 0)
        {
            TurnOffSlot ();
        }
        else
        {
            ItemGear item = ItemDatabase.GetItem ( itemID ) as ItemGear;
            if (item == null)
            {
                TurnOffSlot ();
                return;
            }

            if (item.gearSlot == slot)
            {
                TurnOnSlot ( item );
            }
            else
            {
                TurnOffSlot ();
                return;
            }
        }
    }

    private void TurnOnSlot (ItemGear item)
    {
        currentItemID = item.ID;
        backgroundImage.enabled = false;
        itemImage.enabled = true;
        itemImage.sprite = item.Sprite;
    }

    private void TurnOffSlot ()
    {
        currentItemID = -1;
        itemImage.enabled = false;
        backgroundImage.enabled = true;
    }

    void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
    {
        if(currentItemID >= 0)
        {
            EntityManager.instance.PlayerCharacter.cGear.UnequipGear ( currentItemID );
        }
    }
}
