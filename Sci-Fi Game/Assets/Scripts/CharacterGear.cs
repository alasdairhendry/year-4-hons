using System;
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
    //private int weaponSlotID = -1;
    //private int headSlotID = -1;
    //private int neckSlotID = -1;
    //private int bodySlotID = -1;
    //private int wristSlotID = -1;
    //private int feetSlotID = -1;
    //private int fingerSlotID = -1;
    //private int attachmentSlot01ID = -1;
    //private int attachmentSlot02ID = -1;
    //private int attachmentSlot03ID = -1;
    //private int attachmentSlot04ID = -1;

    //public int WeaponSlotID { get => weaponSlotID; set => weaponSlotID = value; }
    //public int HeadSlotID { get => headSlotID; set => headSlotID = value; }
    //public int NeckSlotID { get => neckSlotID; set => neckSlotID = value; }
    //public int BodySlotID { get => bodySlotID; set => bodySlotID = value; }
    //public int WristSlotID { get => wristSlotID; set => wristSlotID = value; }
    //public int FeetSlotID { get => feetSlotID; set => feetSlotID = value; }
    //public int FingerSlotID { get => fingerSlotID; set => fingerSlotID = value; }
    //public int AttachmentSlot01ID { get => attachmentSlot01ID; set => attachmentSlot01ID = value; }
    //public int AttachmentSlot02ID { get => attachmentSlot02ID; set => attachmentSlot02ID = value; }
    //public int AttachmentSlot03ID { get => attachmentSlot03ID; set => attachmentSlot03ID = value; }
    //public int AttachmentSlot04ID { get => attachmentSlot04ID; set => attachmentSlot04ID = value; }

    private GearEntry weaponSlotID = new GearEntry ();
    private GearEntry headSlotID = new GearEntry ();
    private GearEntry neckSlotID = new GearEntry ();
    private GearEntry bodySlotID = new GearEntry ();
    private GearEntry wristSlotID = new GearEntry ();
    private GearEntry feetSlotID = new GearEntry ();
    private GearEntry fingerSlotID = new GearEntry ();
    private GearEntry attachmentSlot01ID = new GearEntry ();
    private GearEntry attachmentSlot02ID = new GearEntry ();
    private GearEntry attachmentSlot03ID = new GearEntry ();
    private GearEntry attachmentSlot04ID = new GearEntry ();

    public GearEntry WeaponSlotID { get => weaponSlotID; set => weaponSlotID = value; }
    public GearEntry HeadSlotID { get => headSlotID; set => headSlotID = value; }
    public GearEntry NeckSlotID { get => neckSlotID; set => neckSlotID = value; }
    public GearEntry BodySlotID { get => bodySlotID; set => bodySlotID = value; }
    public GearEntry WristSlotID { get => wristSlotID; set => wristSlotID = value; }
    public GearEntry FeetSlotID { get => feetSlotID; set => feetSlotID = value; }
    public GearEntry FingerSlotID { get => fingerSlotID; set => fingerSlotID = value; }
    public GearEntry AttachmentSlot01ID { get => attachmentSlot01ID; set => attachmentSlot01ID = value; }
    public GearEntry AttachmentSlot02ID { get => attachmentSlot02ID; set => attachmentSlot02ID = value; }
    public GearEntry AttachmentSlot03ID { get => attachmentSlot03ID; set => attachmentSlot03ID = value; }
    public GearEntry AttachmentSlot04ID { get => attachmentSlot04ID; set => attachmentSlot04ID = value; }

    public void EquipGear (int itemID)
    {
        ItemGear item = ItemDatabase.GetItem ( itemID ) as ItemGear;
        if (item == null) return;

        GearSlot slot = item.gearSlot;

        switch (slot)
        {
            case GearSlot.Weapon:
                if (!EntityManager.instance.PlayerCharacter.cWeapon.CanEquip ())
                {
                    Debug.Log ( "Cannot equip" );
                    break;
                }
                TryEquip ( weaponSlotID, itemID );
                SetCharacterWeaponData ( itemID );
                break;
            case GearSlot.Head:
                TryEquip ( headSlotID, itemID );
                break;
            case GearSlot.Neck:
                TryEquip ( neckSlotID, itemID );
                break;
            case GearSlot.Body:
                TryEquip ( bodySlotID, itemID );
                break;
            case GearSlot.Wrist:
                TryEquip ( wristSlotID, itemID );
                break;
            case GearSlot.Feet:
                TryEquip ( feetSlotID, itemID );
                break;
            case GearSlot.Finger:
                TryEquip ( fingerSlotID, itemID );
                break;
            case GearSlot.Attachment:
                if (attachmentSlot01ID.currentEquippedID == -1)
                {
                    TryEquip ( attachmentSlot01ID, itemID );
                }
                else if (attachmentSlot02ID.currentEquippedID == -1)
                {
                    TryEquip ( attachmentSlot02ID, itemID );
                }
                else if (attachmentSlot03ID.currentEquippedID == -1)
                {
                    TryEquip ( attachmentSlot03ID, itemID );
                }
                else if (attachmentSlot04ID.currentEquippedID == -1)
                {
                    TryEquip ( attachmentSlot04ID, itemID );
                }
                else
                {
                    MessageBox.AddMessage ( "No empty attachment slots", MessageBox.Type.Error );
                }
                break;
        }

        GearCanvas.instance.RefreshUI ( this );
    }

    private void TryEquip (GearEntry gearEntry, int itemID)
    {
        ItemGear itemGear = ItemDatabase.GetItem ( itemID ) as ItemGear;

        if (gearEntry.currentEquippedID == -1)
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( itemID, 1 );
            gearEntry.currentEquippedID = itemID;
            CheckItemGearEquipableGraphics ( gearEntry, itemGear );           
        }
        else
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( itemID, 1 );
            int notAddedAmount = EntityManager.instance.PlayerInventory.AddItem ( gearEntry.currentEquippedID, 1 );

            if (notAddedAmount == 1)
            {
                EntityManager.instance.PlayerInventory.AddItem ( itemID, 1 );
                return;
            }

            gearEntry.currentEquippedID = itemID;
            CheckItemGearEquipableGraphics ( gearEntry, itemGear );
        }
    }

    private void CheckItemGearEquipableGraphics (GearEntry gearEntry, ItemGear itemGear)
    {
        if (itemGear == null)
        {
            if (gearEntry.currentEquippedGameObject != null)
                Destroy ( gearEntry.currentEquippedGameObject );
            return;
        }

        if (itemGear.category == ItemCategory.Gear)
        {
            ItemGearEquipable equipable = itemGear as ItemGearEquipable;

            if (equipable != null)
            {
                if (equipable.gearData.prefab != null)
                {
                    if (gearEntry.currentEquippedGameObject != null)
                        Destroy ( gearEntry.currentEquippedGameObject );

                    gearEntry.currentEquippedGameObject = Instantiate ( equipable.gearData.prefab );
                    gearEntry.currentEquippedGameObject.transform.SetParent ( EntityManager.instance.PlayerCharacter.Animator.GetBoneTransform ( equipable.gearData.equipBodyPart ) );

                    gearEntry.currentEquippedGameObject.transform.localPosition = equipable.gearData.offsetPosition;
                    gearEntry.currentEquippedGameObject.transform.localEulerAngles = equipable.gearData.offsetRotation;
                    gearEntry.currentEquippedGameObject.transform.localScale = equipable.gearData.localScale;
                }
                else
                {
                    if (gearEntry.currentEquippedGameObject != null)
                        Destroy ( gearEntry.currentEquippedGameObject );
                }
            }
        }
    }

    public void SetWeaponIndexNull ()
    {
        weaponSlotID.currentEquippedID = -1;
    }

    public void UnequipGear (int itemID)
    {
        ItemGear item = ItemDatabase.GetItem ( itemID ) as ItemGear;
        if (item == null) return;

        GearSlot slot = item.gearSlot;

        switch (slot)
        {
            case GearSlot.Weapon:
                if (TryUnequip ( weaponSlotID ))
                {
                    SetCharacterWeaponData ( -1 );
                }
                break;
            case GearSlot.Head:
                TryUnequip ( headSlotID );
                break;
            case GearSlot.Neck:
                TryUnequip ( neckSlotID );
                break;
            case GearSlot.Body:
                TryUnequip ( bodySlotID );
                break;
            case GearSlot.Wrist:
                TryUnequip ( wristSlotID );
                break;
            case GearSlot.Feet:
                TryUnequip ( feetSlotID );
                break;
            case GearSlot.Finger:
                TryUnequip ( fingerSlotID );
                break;
            case GearSlot.Attachment:
                if (attachmentSlot01ID.currentEquippedID == itemID)
                {
                    TryUnequip ( attachmentSlot01ID );
                }
                else if (attachmentSlot02ID.currentEquippedID == itemID)
                {
                    TryUnequip ( attachmentSlot02ID );
                }
                else if (attachmentSlot03ID.currentEquippedID == itemID)
                {
                    TryUnequip ( attachmentSlot03ID );
                }
                else if (attachmentSlot04ID.currentEquippedID == itemID)
                {
                    TryUnequip ( attachmentSlot04ID );
                }
                else
                {
                    Debug.LogError ( "Item not in attachments." );
                }
                break;
        }

        GearCanvas.instance.RefreshUI ( this );
    }

    private bool TryUnequip (GearEntry gearEntry)
    {        
        // Make sure we arent unequipping a null item
        if (gearEntry.currentEquippedID != -1)
        {
            // If they player has inventory space we can unequip the item
            int added = EntityManager.instance.PlayerInventory.AddItem ( gearEntry.currentEquippedID, 1 );
            if (added == 1) return false;


            gearEntry.currentEquippedID = -1;
            CheckItemGearEquipableGraphics ( gearEntry, null );
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetCharacterWeaponData (int itemID)
    {
        if (weaponSlotID.currentEquippedID == -1)
        {
            EntityManager.instance.PlayerCharacter.cWeapon.Unequip ( bypassAnimationDelay: true );
        }
        else
        {
            //// TODO - CHANGE TO SUPPORT MELEE TOO
            ItemGearWeapon item = ItemDatabase.GetItem ( itemID ) as ItemGearWeapon;
            if (item == null) { Debug.LogError ( "Big error" ); return; }

            if (item.WeaponType == WeaponAttackType.Gun)
                EntityManager.instance.PlayerCharacter.cWeapon.Equip ( (item as ItemGearWeaponGun).weaponData );
            else
                EntityManager.instance.PlayerCharacter.cWeapon.Equip ( (item as ItemGearWeaponMelee).weaponData );

        }
    }

    public class GearEntry
    {
        public int currentEquippedID;
        public GameObject currentEquippedGameObject;

        public GearEntry ()
        {
            currentEquippedID = -1;
            currentEquippedGameObject = null;
        }
    }
}
