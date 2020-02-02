using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerObject : MonoBehaviour
{
    [SerializeField] private List<Inventory.ItemStack> initialItems = new List<Inventory.ItemStack> ();
    [SerializeField] private bool useMaxStacks = false;
    [SerializeField] private bool canRecieveItems = false;
    [SerializeField] private string inventoryName;
    [SerializeField] private Animator animator;
    private Inventory inventory = null;

    private void Start ()
    {
        inventory = new Inventory ( 12, useMaxStacks, canRecieveItems );

        for (int i = 0; i < initialItems.Count; i++)
        {
            inventory.AddItem ( initialItems[i].ID, initialItems[i].Amount, true );
        }
    }

    public void Open ()
    {
        ItemContainerCanvas.instance.DisplayContainer ( inventory, inventoryName );
        ItemContainerCanvas.instance.OnContainerClosed += Close;

        if(animator != null)
        {
            animator.SetBool ( "open", true );
        }
    }

    private void Close ()
    {
        if (animator != null)
        {
            animator.SetBool ( "open", false );
        }

        ItemContainerCanvas.instance.OnContainerClosed -= Close;
    }

    //[System.Serializable]
    //private class ItemContainerObjectInitialData
    //{
    //    public int id;
    //    public int amount;
    //}
}
