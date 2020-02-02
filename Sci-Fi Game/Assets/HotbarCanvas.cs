using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarCanvas : MonoBehaviour
{
    public static HotbarCanvas instance;

    [SerializeField] private List<HotbarEntryPanel> panels = new List<HotbarEntryPanel> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    private void Start ()
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].Initialise ( i );
        }
    }

    private void Update ()
    {
        if (Input.GetKeyDown ( KeyCode.Alpha1 ) || Input.GetKeyDown ( KeyCode.Keypad1 ))
        {
            OnHotkeyPressed ( 0 );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha2 ) || Input.GetKeyDown ( KeyCode.Keypad2 ))
        {
            OnHotkeyPressed ( 1 );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha3 ) || Input.GetKeyDown ( KeyCode.Keypad3 ))
        {
            OnHotkeyPressed ( 2 );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha4 ) || Input.GetKeyDown ( KeyCode.Keypad4 ))
        {
            OnHotkeyPressed ( 3 );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha5 ) || Input.GetKeyDown ( KeyCode.Keypad5 ))
        {
            OnHotkeyPressed ( 4 );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha6 ) || Input.GetKeyDown ( KeyCode.Keypad6 ))
        {
            OnHotkeyPressed ( 5 );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha7 ) || Input.GetKeyDown ( KeyCode.Keypad7 ))
        {
            OnHotkeyPressed ( 6 );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha8 ) || Input.GetKeyDown ( KeyCode.Keypad8 ))
        {
            OnHotkeyPressed ( 7 );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha9 ) || Input.GetKeyDown ( KeyCode.Keypad9 ))
        {
            OnHotkeyPressed ( 8 );
        }
    }

    private void OnHotkeyPressed (int hotkey)
    {
        panels[hotkey].Interact ();
    }

    public void TrySwitchItems(int indexA, int indexB)
    {
        int itemIDA = panels[indexA].currentItemID;
        int itemIDB = panels[indexB].currentItemID;
        panels[indexA].SetItem ( itemIDB );
        panels[indexB].SetItem ( itemIDA );
    }

    public void TryAddItem (int dragItemID, int toIndex)
    {
        ItemBaseData item = ItemDatabase.GetItem ( dragItemID );
        bool compatibleInteractType = false;

        switch (item.GetDefaultInteractionData ().interactType)
        {
            case InventoryInteractionData.InteractType.Drink:
                compatibleInteractType = true;
                break;
            case InventoryInteractionData.InteractType.Eat:
                compatibleInteractType = true;
                break;
            case InventoryInteractionData.InteractType.Equip:
                compatibleInteractType = true;
                break;
            default:
                Debug.LogError ( "Incompatible hotbar item" );
                break;
        }

        if (!compatibleInteractType) return;

        panels[toIndex].SetItem ( dragItemID );
    }

    public void TryRemoveItem (int dragItemID, int fromIndex)
    {
        panels[fromIndex].RemoveItem ();
    }
}
