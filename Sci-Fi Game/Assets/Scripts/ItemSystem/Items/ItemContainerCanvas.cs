using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemContainerCanvas : MonoBehaviour
{
    public static ItemContainerCanvas instance;

    public Inventory targetInventory { get; protected set; }
    [SerializeField] private List<ItemContainerPanel> containerPanels = new List<ItemContainerPanel> ();
    [SerializeField] private TextMeshProUGUI inventoryNameText;
    [SerializeField] private CanvasGroup cGroup;
    public bool IsActive { get; protected set; }

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    private void Start ()
    {
        for (int i = 0; i < containerPanels.Count; i++)
        {
            containerPanels[i].SetInventoryIndex ( i );
        }

        OnInventoryChanged ();
    }

    public void DisplayContainer(Inventory target, string name)
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
        cGroup.alpha = 1;
        cGroup.blocksRaycasts = true;
        IsActive = true;
    }

    public void HideContainer ()
    {
        if (targetInventory != null)
        {
            targetInventory.UnregisterInventoryChanged ( OnInventoryChanged );
        }

        cGroup.alpha = 0;
        cGroup.blocksRaycasts = false;
        IsActive = false;
    }

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
                        containerPanels[i].PanelButton.onClick.AddListener ( () =>
                        {
                            targetInventory.SendTo ( EntityManager.instance.PlayerInventory, id, 1 );
                        } );
                    }
                }
            }
        }
    }
}
