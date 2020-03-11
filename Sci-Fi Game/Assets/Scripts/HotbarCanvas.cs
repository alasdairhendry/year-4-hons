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

    public void OnHotkeyPressed (KeyCode keyCode, bool isShift, bool isControl, bool isAlt)
    {
        if (keyCode == KeyCode.Alpha1)
        {
            OnHotkeyPressed ( 0 );
        }

        if (keyCode == KeyCode.Alpha2)
        {
            OnHotkeyPressed ( 1 );
        }

        if (keyCode == KeyCode.Alpha3)
        {
            OnHotkeyPressed ( 2 );
        }

        if (keyCode == KeyCode.Alpha4)
        {
            OnHotkeyPressed ( 3 );
        }

        if (keyCode == KeyCode.Alpha5)
        {
            OnHotkeyPressed ( 4 );
        }

        if (keyCode == KeyCode.Alpha6)
        {
            OnHotkeyPressed ( 5 );
        }

        if (keyCode == KeyCode.Alpha7)
        {
            OnHotkeyPressed ( 6 );
        }

        if (keyCode == KeyCode.Alpha8)
        {
            OnHotkeyPressed ( 7 );
        }

        if (keyCode == KeyCode.Alpha9)
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
            case InventoryInteractionData.InteractType.Attach:
                compatibleInteractType = true;
                break;
            default:
                Debug.LogError ( "Incompatible hotbar item" );
                break;
        }

        if (item.category == ItemCategory.Consumable) compatibleInteractType = true;

        if (!compatibleInteractType) return;

        panels[toIndex].SetItem ( dragItemID );
    }

    public void TryRemoveItem (int dragItemID, int fromIndex)
    {
        panels[fromIndex].RemoveItem ();
    }
}
