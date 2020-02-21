using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemContainerObject : MonoBehaviour
{
    [SerializeField] private string inventoryName;
    [SerializeField] private DropTable dropTable = null;
    [SerializeField] private int rollsOnOpened = 4;
    [Space]
    [SerializeField] private GameObject graphicsParent = null;
    [SerializeField] private Interactable interactable = null;
    [Space]
    [SerializeField] private bool resuppliesWhenEmpty = true;
    [SerializeField] private float resupplyDelay = 30.0f;
    [SerializeField] private float hideAfterLootDelay = 5.0f;
    [Space]
    [SerializeField] private bool stacksEveryItem = true;
    [SerializeField] private bool canRecieveItems = false;
    [Space]
    [SerializeField] private Animator animator;

    private Inventory inventory = null;
    private float resupplyCounter = 0.0f;

    private float hideAfterLootCounter = 0.0f;
    private bool shouldCountHide = false;
    private bool isHidden = false;

    private void Start ()
    {
        inventory = new Inventory ( 12, stacksEveryItem, canRecieveItems );
        inventory.RegisterInventoryChanged ( () => { if (inventory.IsEmpty) ItemContainerCanvas.instance.Close (); } );
        ResupplyContainer ();
    }

    private void Update ()
    {
        if (isHidden == false)
        {
            if (shouldCountHide)
            {
                hideAfterLootCounter += Time.deltaTime;

                if (hideAfterLootCounter >= hideAfterLootDelay)
                {
                    hideAfterLootCounter = 0.0f;
                    shouldCountHide = false;
                    HideContainer ();
                }
            }
        }

        if (isHidden == true && resuppliesWhenEmpty == true)
        {
            resupplyCounter += Time.deltaTime;

            if (resupplyCounter >= resupplyDelay)
            {
                resupplyCounter = 0.0f;
                ShowContainer ();
            }
        }
    }

    private void ShowContainer ()
    {
        isHidden = false;
        graphicsParent.SetActive ( true );
        interactable.IsInteractable = true;
    }

    private void HideContainer ()
    {
        isHidden = true;
        graphicsParent.SetActive ( false );
        interactable.IsInteractable = false;
    }

    private void ResupplyContainer ()
    {
        for (int i = 0; i < rollsOnOpened; i++)
        {
            Inventory.ItemStack roll = dropTable.RollTable ();

            if (roll != null)
            {
                inventory.AddItem ( roll.ID, roll.Amount );
            }
        }
    }

    public void Open ()
    {
        if(shouldCountHide == false)
        {
            ResupplyContainer ();

            if (inventory.IsEmpty)
            {
                MessageBox.AddMessage ( "You open the container to find nothing inside." );
            }
            else
            {
                MessageBox.AddMessage ( "You open the container and find some lovely items inside." );
            }
        }
        else
        {
            shouldCountHide = false;

        }
        hideAfterLootCounter = 0.0f;

        ItemContainerCanvas.instance.SetContainerInventory ( inventory, inventoryName );
        ItemContainerCanvas.instance.Open ();
        ItemContainerCanvas.instance.OnContainerClosed += Close;

        if(animator != null)
        {
            animator.SetBool ( "open", true );
        }
    }

    private void Close ()
    {
        if (inventory.IsEmpty)
        {
            hideAfterLootCounter = hideAfterLootDelay * 0.75f;
        }

        shouldCountHide = true;


        if (animator != null)
        {
            animator.SetBool ( "open", false );
        }

        ItemContainerCanvas.instance.OnContainerClosed -= Close;
    }
}
