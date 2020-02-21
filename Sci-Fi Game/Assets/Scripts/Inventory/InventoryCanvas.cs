using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCanvas : UIPanel
{
    private Inventory targetInventory;
    [SerializeField] private List<InventoryItemPanel> inventoryPanels = new List<InventoryItemPanel> ();
    [SerializeField] private GameObject panel;

    public List<InventoryItemPanel> InventoryPanels { get => inventoryPanels; }

    [SerializeField] private bool DEBUG_SHOW_ADD_REM_ITEMS = false;
    [SerializeField] private GameObject DEBUG_ITEM_PANEL;
    [SerializeField] private TMP_InputField DEBUG_ID_INPUT;
    [SerializeField] private TMP_InputField DEBUG_AMOUNT_INPUT;
    [SerializeField] private TextMeshProUGUI DEBUG_ITEM_NAME;
    [SerializeField] private Button DEBUG_ADD_BUTTON;
    [SerializeField] private Button DEBUG_REMOVE_BUTTON;

    private void Start ()
    {
        if (DEBUG_SHOW_ADD_REM_ITEMS)
        {
            DEBUG_ITEM_PANEL.SetActive ( true );
            DEBUG_ADD_BUTTON.onClick.AddListener ( () => { EntityManager.instance.PlayerInventory.AddItem ( int.Parse ( DEBUG_ID_INPUT.text ), int.Parse ( DEBUG_AMOUNT_INPUT.text ) ); } );
            DEBUG_REMOVE_BUTTON.onClick.AddListener ( () => { EntityManager.instance.PlayerInventory.RemoveItem ( int.Parse ( DEBUG_ID_INPUT.text ), int.Parse ( DEBUG_AMOUNT_INPUT.text ) ); } );

            DEBUG_ID_INPUT.onValueChanged.AddListener ( (s) => {
                int id = 0;

                if (int.TryParse ( DEBUG_ID_INPUT.text, out id ))
                {
                    ItemBaseData item;
                    if (ItemDatabase.GetItem ( id, out item ))
                    {
                        DEBUG_ITEM_NAME.text = item.Name;
                    }
                }
            } );

            //if(EventSystem.current.currentSelectedGameObject == DEBUG_ID_INPUT.gameObject)
            //{
            //    int id = 0;

            //    if(int.TryParse(DEBUG_ID_INPUT.text, out id ))
            //    {
            //        ItemBaseData item;
            //        if (ItemDatabase.GetItem ( id, out item ))
            //        {
            //            DEBUG_ITEM_NAME.text = item.Name;
            //        }
            //    }
            //}
        }
        else
        {
            DEBUG_ITEM_PANEL.SetActive ( false );
        }

        for (int i = 0; i < inventoryPanels.Count; i++)
        {
            inventoryPanels[i].SetInventoryIndex ( i );
        }

        OnInventoryChanged ();
        Close ();
    }

    public override void Open ()
    {
        base.Open ();
        panel.SetActive ( true );
        isOpened = true;
    }

    public override void Close ()
    {
        base.Close ();
        panel.SetActive ( false );
        isOpened = false;
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

        TooltipCanvas.instance?.Refresh ();
    }   
}
