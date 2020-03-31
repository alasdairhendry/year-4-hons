using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankCanvas : UIPanel
{
    public static BankCanvas instance;

    private Inventory targetInventory;
    [SerializeField] private List<InventoryItemPanel> inventoryPanels = new List<InventoryItemPanel> ();
    [SerializeField] private GameObject panel;

    public List<InventoryItemPanel> InventoryPanels { get => inventoryPanels; }

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
        Close ( true );
    }

    private void Start ()
    {
        for (int i = 0; i < inventoryPanels.Count; i++)
        {
            inventoryPanels[i].SetInventoryIndex ( i );
        }

        OnInventoryChanged ();
        Close ( true );
    }

    public override void Open ()
    {
        StoreCanvas.instance.Close ();
        ItemContainerCanvas.instance.Close ();
        EntityManager.instance.InventoryCanvas.Open ();
        base.Open ();
        panel.SetActive ( true );
        isOpened = true;
        UIPanelController.instance.OnPanelOpened ( this );
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        panel.SetActive ( false );
        isOpened = false;
        UIPanelController.instance.OnPanelClosed ( this );
    }

    public void SetTargetInventory (Inventory target)
    {
        if (targetInventory != null)
        {
            targetInventory.UnregisterInventoryChanged ( OnInventoryChanged );
        }

        targetInventory = target;
        targetInventory.RegisterInventoryChanged ( OnInventoryChanged );
        OnInventoryChanged ();
    }

    private void OnInventoryChanged ()
    {
        if (targetInventory == null)
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

        TooltipCanvas.instance?.Refresh ();
    }
}
