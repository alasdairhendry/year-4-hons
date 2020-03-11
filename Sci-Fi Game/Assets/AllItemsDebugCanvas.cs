using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllItemsDebugCanvas : UIPanel
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private Transform contentPanel;

    private void Awake ()
    {
        Close ( true );
        Populate ();
    }

    private void Update ()
    {
        if (Input.GetKey ( KeyCode.LeftShift ) && Input.GetKey ( KeyCode.LeftAlt ) && Input.GetKeyDown ( KeyCode.BackQuote ))
        {
            Open ();
        }
    }

    public override void Open ()
    {
        base.Open ();
        mainPanel.SetActive ( true );
        isOpened = true;
        UIPanelController.instance.OnPanelOpened ( this );
    }

    private void Populate ()
    {
        for (int i = 0; i < 1000; i++)
        {
            ItemBaseData item = null;

            if (ItemDatabase.GetItem ( i, out item ))
            {
                GameObject go = Instantiate ( prefab );
                ItemContainerPanel panel = go.GetComponent<ItemContainerPanel> ();
                panel.SetInventoryIndex ( i );
                panel.SetContent ( item.Sprite, item.ID, 1000 );
                go.transform.SetParent ( contentPanel );

                int shiftClick = 1000;
                panel.PanelButton.onClick.AddListener ( () =>
                {
                    if (Input.GetKey ( KeyCode.LeftShift ))
                        EntityManager.instance.PlayerInventory.AddItem ( item.ID, shiftClick );
                    else
                        EntityManager.instance.PlayerInventory.AddItem ( item.ID, 1 );
                } );
            }
        }
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        mainPanel.SetActive ( false );
        isOpened = false;
        UIPanelController.instance.OnPanelClosed ( this );
    }
}
