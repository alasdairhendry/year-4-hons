using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GearSlot
{
    Weapon,
    Head,
    Neck,
    Body,
    Wrist,
    Feet,
    Attachment,
    Finger
}

public class CharacterGear : MonoBehaviour
{
    private int weaponSlotID = -1;
    private int headSlotID = -1;
    private int neckSlotID = -1;
    private int bodySlotID = -1;
    private int wristSlotID = -1;
    private int feetSlotID = -1;
    private int fingerSlotID = -1;
    private int attachmentSlot01ID = -1;
    private int attachmentSlot02ID = -1;
    private int attachmentSlot03ID = -1;
    private int attachmentSlot04ID = -1;

    public int WeaponSlotID { get => weaponSlotID; set => weaponSlotID = value; }
    public int HeadSlotID { get => headSlotID; set => headSlotID = value; }
    public int NeckSlotID { get => neckSlotID; set => neckSlotID = value; }
    public int BodySlotID { get => bodySlotID; set => bodySlotID = value; }
    public int WristSlotID { get => wristSlotID; set => wristSlotID = value; }
    public int FeetSlotID { get => feetSlotID; set => feetSlotID = value; }
    public int FingerSlotID { get => fingerSlotID; set => fingerSlotID = value; }
    public int AttachmentSlot01ID { get => attachmentSlot01ID; set => attachmentSlot01ID = value; }
    public int AttachmentSlot02ID { get => attachmentSlot02ID; set => attachmentSlot02ID = value; }
    public int AttachmentSlot03ID { get => attachmentSlot03ID; set => attachmentSlot03ID = value; }
    public int AttachmentSlot04ID { get => attachmentSlot04ID; set => attachmentSlot04ID = value; }

    public void EquipGear (int itemID)
    {
        ItemGear item = ItemDatabase.GetItem ( itemID ) as ItemGear;
        if (item == null) return;

        GearSlot slot = item.gearSlot;

        switch (slot)
        {
            case GearSlot.Weapon:
                TryEquip ( ref weaponSlotID, itemID );
                Debug.Log ( item.Name );
                Debug.Log ( item.gearSlot );
                SetCharacterWeaponData ( itemID );
                break;
            case GearSlot.Head:
                TryEquip ( ref headSlotID, itemID );
                break;
            case GearSlot.Neck:
                TryEquip ( ref neckSlotID, itemID );
                break;
            case GearSlot.Body:
                TryEquip ( ref bodySlotID, itemID );
                break;
            case GearSlot.Wrist:
                TryEquip ( ref wristSlotID, itemID );
                break;
            case GearSlot.Feet:
                TryEquip ( ref feetSlotID, itemID );
                break;
            case GearSlot.Finger:
                TryEquip ( ref fingerSlotID, itemID );
                break;
            case GearSlot.Attachment:
                if (attachmentSlot01ID == -1)
                {
                    TryEquip ( ref attachmentSlot01ID, itemID );
                }
                else if (attachmentSlot02ID == -1)
                {
                    TryEquip ( ref attachmentSlot02ID, itemID );
                }
                else if (attachmentSlot03ID == -1)
                {
                    TryEquip ( ref attachmentSlot03ID, itemID );
                }
                else if (attachmentSlot04ID == -1)
                {
                    TryEquip ( ref attachmentSlot04ID, itemID );
                }
                else
                {
                    MessageBox.AddMessage ( "No empty attachment slots", MessageBox.Type.Error );
                }
                break;
        }

        GearCanvas.instance.RefreshUI ( this );
    }

    private void TryEquip (ref int slot, int itemID)
    {
        if (slot == -1)
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( itemID, 1 );
            slot = itemID;
        }
        else
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( itemID, 1 );
            int added = EntityManager.instance.PlayerInventory.AddItem ( slot, 1 );

            if (added == 1)
            {
                EntityManager.instance.PlayerInventory.AddItem ( itemID, 1 );
                return;
            }

            slot = itemID;
        }
    }

    private void TryUnequip (ref int slot)
    {
        if (slot != -1)
        {
            int added = EntityManager.instance.PlayerInventory.AddItem ( slot, 1 );
            if (added == 1) return;

            slot = -1;
        }
    }

    public void UnequipGear (int itemID)
    {
        ItemGear item = ItemDatabase.GetItem ( itemID ) as ItemGear;
        if (item == null) return;

        GearSlot slot = item.gearSlot;

        switch (slot)
        {
            case GearSlot.Weapon:
                TryUnequip ( ref weaponSlotID );
                SetCharacterWeaponData ( -1 );
                break;
            case GearSlot.Head:
                TryUnequip ( ref headSlotID );
                break;
            case GearSlot.Neck:
                TryUnequip ( ref neckSlotID );
                break;
            case GearSlot.Body:
                TryUnequip ( ref bodySlotID );
                break;
            case GearSlot.Wrist:
                TryUnequip ( ref wristSlotID );
                break;
            case GearSlot.Feet:
                TryUnequip ( ref feetSlotID );
                break;
            case GearSlot.Finger:
                TryUnequip ( ref fingerSlotID );
                break;
            case GearSlot.Attachment:
                if (attachmentSlot01ID == itemID)
                {
                    TryUnequip ( ref attachmentSlot01ID );
                }
                else if (attachmentSlot02ID == itemID)
                {
                    TryUnequip ( ref attachmentSlot02ID );
                }
                else if (attachmentSlot03ID == itemID)
                {
                    TryUnequip ( ref attachmentSlot03ID );
                }
                else if (attachmentSlot04ID == itemID)
                {
                    TryUnequip ( ref attachmentSlot04ID );
                }
                else
                {
                    Debug.LogError ( "Item not in attachments." );
                }
                break;
        }

        GearCanvas.instance.RefreshUI ( this );
    }

    private void SetCharacterWeaponData (int itemID)
    {
        if (weaponSlotID == -1)
        {
            EntityManager.instance.PlayerCharacter.cWeapon.Unequip ();
        }
        else
        {
            ItemGearWeapon item = ItemDatabase.GetItem ( itemID ) as ItemGearWeapon;
            if (item == null) { Debug.LogError ( "Big error" ); return; }

            EntityManager.instance.PlayerCharacter.cWeapon.Equip ( item.weaponData );
        }
    }
}
