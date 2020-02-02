using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearCanvas : MonoBehaviour
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
        Close ();
    }

    public void Open ()
    {
        RefreshUI ( EntityManager.instance.PlayerCharacter.cGear );
        mainPanel.SetActive ( true );
    }

    public void Close ()
    {
        mainPanel.SetActive ( false );
    }

    public void Trigger ()
    {
        if (mainPanel.activeSelf) Close ();
        else Open ();
    }

    public void RefreshUI (CharacterGear cGear)
    {
        weaponSlot.SetItem ( cGear.WeaponSlotID );
        attachment01Slot.SetItem ( cGear.AttachmentSlot01ID );
        attachment02Slot.SetItem ( cGear.AttachmentSlot02ID );
        attachment03Slot.SetItem ( cGear.AttachmentSlot03ID );
        attachment04Slot.SetItem ( cGear.AttachmentSlot04ID );

        headSlot.SetItem ( cGear.HeadSlotID );
        bodySlot.SetItem ( cGear.BodySlotID );
        feetSlot.SetItem ( cGear.FeetSlotID );

        neckSlot.SetItem ( cGear.NeckSlotID );
        wristSlot.SetItem ( cGear.WristSlotID );
        fingerSlot.SetItem ( cGear.FingerSlotID );
    }
}
