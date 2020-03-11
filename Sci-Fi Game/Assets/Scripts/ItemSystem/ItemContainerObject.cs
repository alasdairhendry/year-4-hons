using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemContainerObject : MonoBehaviour
{
    [SerializeField] private string inventoryName;
    [SerializeField] private DropTable dropTable = null;
    [SerializeField] private int rollsOnOpened = 4;
    [SerializeField] private int maxItemsPerRoll = 4;
    [SerializeField] private bool guaranteeDrops = false;
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
    private bool isOpen = false;

    private float hideAfterLootCounter = 0.0f;
    private bool shouldCountHide = false;
    private bool isHidden = false;

    private void Start ()
    {
        inventory = new Inventory ( 12, stacksEveryItem, canRecieveItems );
        inventory.RegisterInventoryChanged ( () => { if (inventory.IsEmpty) ItemContainerCanvas.instance.Close (); } );
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
        if (resuppliesWhenEmpty)
        {
            isHidden = true;
            graphicsParent.SetActive ( false );
            interactable.IsInteractable = false;
        }
        else
        {
            Destroy ( this.gameObject );
        }
    }

    private void ResupplyContainer ()
    {
        bool anyWereFactionRolls = false;

            int count = 0;

        for (int i = 0; i < rollsOnOpened; i++)
        {
            List<Inventory.ItemStack> drops = new List<Inventory.ItemStack> ();
            bool wasFactionRoll = false;

            if (guaranteeDrops)
            {
                if (dropTable.RollTableGuaranteed ( out drops, out wasFactionRoll ))
                {
                    inventory.AddMultipleItems ( drops, true );
                    if (wasFactionRoll) anyWereFactionRolls = true;
                    count++;
                }
            }
            else
            {
                if (dropTable.RollTable ( out drops, out wasFactionRoll ))
                {
                    inventory.AddMultipleItems ( drops, true );
                    if (wasFactionRoll) anyWereFactionRolls = true;
                    count++;
                }
            }

            if (count >= maxItemsPerRoll)
            {
                break;
            }
        }
        

        if (anyWereFactionRolls)
        {
            MessageBox.AddMessage ( "Your Faction Specialisation helps you find an item", MessageBox.Type.Warning );
        }
    }

    public void Open ()
    {
        if (isOpen) return;
        isOpen = true;

        if(shouldCountHide == false)
        {
            inventory.ClearInventoryStacks ();
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

        SoundEffectManager.Play ( AudioClipAsset.ChestOpen, AudioMixerGroup.SFX , 1);
    }

    private void Close ()
    {
        isOpen = false;
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
        SoundEffectManager.Play ( AudioClipAsset.ChestClose, AudioMixerGroup.SFX , 1);
    }
}
