﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCanvas : UIPanel
{
    public static StoreCanvas instance;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private List<ShopItemPanel> itemPanels = new List<ShopItemPanel> ();
    public NPCShopkeeper currentShopkeeper { get; protected set; } = null;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        Close ();
    }

    private void Update ()
    {
        if (EntityManager.instance.PlayerCharacter.cInput.rawInput != Vector2.zero)
        {
            Close ();
        }
    }

    public void SetShopkeeper(NPCShopkeeper shopkeeper)
    {
        this.currentShopkeeper = shopkeeper;

        for (int i = 0; i < itemPanels.Count; i++)
        {
            itemPanels[i].Disable ();
        }

        for (int i = 0; i < shopkeeper.BaseInventory.Count; i++)
        {
            if(i >= itemPanels.Count)
            {
                Debug.LogError ( "The shop interface cannot hold this many items" );
                continue;
            }

            itemPanels[i].SetContent ( shopkeeper.BaseInventory[i].itemID, shopkeeper.GetBuyPrice ( shopkeeper.BaseInventory[i].itemID ) );
        }
    }

    public override void Open ()
    {
        EntityManager.instance.InventoryCanvas.Open ();
        ItemContainerCanvas.instance.Close ();

        base.Open ();
        isOpened = true;
        mainPanel.SetActive ( true );
    }

    public override void Close ()
    {
        base.Close ();
        isOpened = false;
        mainPanel.SetActive ( false );
    }
}