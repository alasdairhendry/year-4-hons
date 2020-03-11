using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearCanvas : UIPanel
{
    public static GearCanvas instance;

    [SerializeField] private GameObject mainPanel;
    [Space]
    [SerializeField] private GearUIEntrySlot weaponSlot;
    [SerializeField] private GearUIEntrySlot attachment01Slot;
    [SerializeField] private GearUIEntrySlot attachment02Slot;
    [SerializeField] private GearUIEntrySlot attachment03Slot;
    [SerializeField] private GearUIEntrySlot attachment04Slot;
    [Space]
    [SerializeField] private GearUIEntrySlot headSlot;
    [SerializeField] private GearUIEntrySlot bodySlot;
    [SerializeField] private GearUIEntrySlot feetSlot;
    [Space]
    [SerializeField] private GearUIEntrySlot neckSlot;
    [SerializeField] private GearUIEntrySlot wristSlot;
    [SerializeField] private GearUIEntrySlot fingerSlot;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        RefreshUI ( EntityManager.instance.PlayerCharacter.cGear );
        Close ( true );
    }

    public override void Open ()
    {
        base.Open ();
        isOpened = true;
        RefreshUI ( EntityManager.instance.PlayerCharacter.cGear );
        mainPanel.SetActive ( true );
        UIPanelController.instance.OnPanelOpened ( this );
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        mainPanel.SetActive ( false );
        isOpened = false;
        UIPanelController.instance.OnPanelClosed ( this );
    }

    public void RefreshUI (CharacterGear cGear)
    {
        weaponSlot.SetItem ( cGear.WeaponSlotID.currentEquippedID );
        attachment01Slot.SetItem ( cGear.AttachmentSlot01ID.currentEquippedID );
        attachment02Slot.SetItem ( cGear.AttachmentSlot02ID.currentEquippedID );
        attachment03Slot.SetItem ( cGear.AttachmentSlot03ID.currentEquippedID );
        attachment04Slot.SetItem ( cGear.AttachmentSlot04ID.currentEquippedID );

        headSlot.SetItem ( cGear.HeadSlotID.currentEquippedID );
        bodySlot.SetItem ( cGear.BodySlotID.currentEquippedID );
        feetSlot.SetItem ( cGear.FeetSlotID.currentEquippedID );

        neckSlot.SetItem ( cGear.NeckSlotID.currentEquippedID );
        wristSlot.SetItem ( cGear.WristSlotID.currentEquippedID );
        fingerSlot.SetItem ( cGear.FingerSlotID.currentEquippedID );
    }
}
