using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
    private Inventory targetInventory;
    [SerializeField] private List<InventoryItemPanel> inventoryPanels = new List<InventoryItemPanel> ();

    public List<InventoryItemPanel> InventoryPanels { get => inventoryPanels; }

    private void Start ()
    {
        for (int i = 0; i < inventoryPanels.Count; i++)
        {
            inventoryPanels[i].SetInventoryIndex ( i );
        }

        OnInventoryChanged ();
    }

    public void SetTargetInventory (Inventory target )
    {
        if(targetInventory != null)
        {
            targetInventory.UnregisterInventoryChanged ( OnInventoryChanged );
        }

        targetInventory = target;
        targetInventory.RegisterInventoryChanged ( OnInventoryChanged );
        OnInventoryChanged ();
    }

    private void OnInventoryChanged ()
    {
        if(targetInventory == null)
        {
            for (int i = 0; i < inventoryPanels.Count; i++)
            {
                inventoryPanels[i].Disable ();
            }
        }
        else
        {
            for (int i = 0; i < inventoryPanels.Count; i++)
            {
                if (i >= targetInventory.GetStackCount)
                {
                    inventoryPanels[i].Disable ();
                }
                else
                {
                    ItemBaseData item = null;

                    if (ItemDatabase.GetItem ( targetInventory.GetStackAtIndex ( i ).ID, out item ))
                    {
                        inventoryPanels[i].SetContent ( item.Sprite, item.ID, targetInventory.GetStackAtIndex ( i ).Amount );
                    }
                }
            }
        }
    }   
}
