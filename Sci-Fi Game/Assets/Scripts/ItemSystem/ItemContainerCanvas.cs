﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemContainerCanvas : UIPanel
{
    public static ItemContainerCanvas instance;

    public Inventory targetInventory { get; protected set; }
    [SerializeField] private List<ItemContainerPanel> containerPanels = new List<ItemContainerPanel> ();
    [SerializeField] private TextMeshProUGUI inventoryNameText;
    [SerializeField] private CanvasGroup cGroup;
    public bool IsActive { get; protected set; }
    public System.Action OnContainerClosed;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
        Close ();
    }

    private void Start ()
    {
        for (int i = 0; i < containerPanels.Count; i++)
        {
            containerPanels[i].SetInventoryIndex ( i );
        }

        OnInventoryChanged ();
    }

    private void Update ()
    {
        if(EntityManager.instance.PlayerCharacter.cInput.rawInput != Vector2.zero)
        {
            Close ();
        }
    }

    public override void Open ()
    {
        if (targetInventory == null) return;

        EntityManager.instance.InventoryCanvas.Open ();
        base.Open ();

        cGroup.alpha = 1;
        cGroup.blocksRaycasts = true;
        IsActive = true;

    }

    public override void Close ()
    {
        base.Close ();
        if (targetInventory != null)
        {
            targetInventory.UnregisterInventoryChanged ( OnInventoryChanged );
        }

        cGroup.alpha = 0;
        cGroup.blocksRaycasts = false;
        IsActive = false;
        OnContainerClosed?.Invoke ();
    }

    public void SetContainerInventory (Inventory target, string name)
    {
        if (target == null) return;

        if (targetInventory != null)
        {
            targetInventory.UnregisterInventoryChanged ( OnInventoryChanged );
        }

        inventoryNameText.text = name;
        targetInventory = target;
        targetInventory.RegisterInventoryChanged ( OnInventoryChanged );
        OnInventoryChanged ();
    }

    //public void DisplayContainer(Inventory target, string name)
    //{

    //}

    private void OnInventoryChanged ()
    {
        if (targetInventory == null)
        {
            for (int i = 0; i < containerPanels.Count; i++)
            {
                containerPanels[i].Disable ();
                containerPanels[i].PanelButton.onClick.RemoveAllListeners ();
            }
        }
        else
        {
            for (int i = 0; i < containerPanels.Count; i++)
            {
                if (i >= targetInventory.GetStackCount)
                {
                    containerPanels[i].Disable ();
                    containerPanels[i].PanelButton.onClick.RemoveAllListeners ();
                }
                else
                {
                    containerPanels[i].PanelButton.onClick.RemoveAllListeners ();

                    ItemBaseData item = null;

                    if (ItemDatabase.GetItem ( targetInventory.GetStackAtIndex ( i ).ID, out item ))
                    {
                        int id = item.ID;
                        int amount = Mathf.Min ( 1, targetInventory.GetStackAtIndex ( i ).Amount );
                        containerPanels[i].SetContent ( item.Sprite, item.ID, targetInventory.GetStackAtIndex ( i ).Amount );

                        int shiftClick = targetInventory.GetStackAtIndex ( i ).Amount;
                        containerPanels[i].PanelButton.onClick.AddListener ( () =>
                        {
                            if (Input.GetKey ( KeyCode.LeftShift ))
                                targetInventory.SendTo ( EntityManager.instance.PlayerInventory, id, shiftClick );
                            else
                                targetInventory.SendTo ( EntityManager.instance.PlayerInventory, id, 1 );
                        } );
                    }
                }
            }
        }

        TooltipCanvas.instance?.Refresh ();
    }
}
