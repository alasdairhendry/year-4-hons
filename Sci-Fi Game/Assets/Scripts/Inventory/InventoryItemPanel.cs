using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject contentPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private GameObject itemAmountPanel;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private TooltipItemUI tooltipItem;
    public int inventoryIndex { get; protected set; } = -1;
    public int ItemID { get; protected set; }
    public int ItemAmount { get; protected set; }

    public void SetInventoryIndex(int index)
    {        
        inventoryIndex = index;
    }

    public void Disable ()
    {
        //if (contentPanel.activeSelf == false) return;
        contentPanel.SetActive ( false );
        this.ItemID = -1;
        this.ItemAmount = -1;
        tooltipItem.SetTooltipAction ( null );
    }

    public void SetContent (Sprite sprite, int itemID, int amount)
    {
        //if (contentPanel.activeSelf == true) return;

        contentPanel.SetActive ( true );
        this.ItemID = itemID;
        this.ItemAmount = amount;

        itemIcon.sprite = sprite;

        itemAmountPanel.SetActive ( true );
        itemAmountText.text = this.ItemAmount.ToString ();
        tooltipItem.SetTooltipAction ( () => { return ItemDatabase.GetInteractionPrefix ( itemID ) + " " + ColourHelper.TagColour ( ItemDatabase.GetItem ( itemID ).Name, ColourDescription.OffWhiteText ) + "\n" + ColourHelper.TagSize ( ItemDatabase.GetItem ( itemID ).Description, 75.0f ); } );
    }

    void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
    {
        DragHandler.OnBeginDrag ( inventoryIndex, ItemID, ItemAmount, DragHandler.Master.PlayerInventory, contentPanel.transform );       
    }

    void IDragHandler.OnDrag (PointerEventData eventData)
    {
        DragHandler.OnDrag ();
    }

    void IEndDragHandler.OnEndDrag (PointerEventData eventData)
    {
        DragHandler.OnEndDrag ();
    }

    void IDropHandler.OnDrop (PointerEventData eventData)
    {
        DragHandler.OnDrop ( inventoryIndex, DragHandler.Master.PlayerInventory );
    }

    void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
    {
        InventoryItemInteraction.OnInventoryItemHovered ( ItemID );
    }

    void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
    {
        InventoryItemInteraction.OnInventoryItemUnhovered ( ItemID );
    }

    void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
    {
        if (ItemID < 0) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ItemDatabase.InvokeInventoryDefaultAction ( ItemID, inventoryIndex );
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            DropdownMenuCanvas.instance.Open ( ItemDatabase.GetItem ( ItemID ).GetAllInteractionData (), ItemID, inventoryIndex );
        }
    }
}

public static class DragHandler
{
    public static bool isDragging = false;
    public enum Master { PlayerInventory, ItemContainer }
    public static Master fromMaster = Master.PlayerInventory;

    public static int fromIndex = -1;

    public static int dragItemID = -1;
    public static int dragItemAmount = -1;

    private static Transform targetObject;
    private static Transform originalParent;
    private static CanvasGroup targetCanvasGroup;
    private static Vector3 originalPosition;
    private static Vector3 offset;

    public static void OnBeginDrag (int _fromIndex, int _dragItemID, int _dragItemAmount, Master _fromMaster, Transform _targetObject)
    {
        if (isDragging) { Debug.Log ( 1 ); return; }
        if (!ItemDatabase.ItemExists ( _dragItemID )) { Debug.Log ( 2 ); return; }

        targetObject = _targetObject;
        originalPosition = targetObject.position;
        offset = originalPosition - Input.mousePosition;

        originalParent = targetObject.parent;
        targetObject.SetParent ( targetObject.root );
        targetObject.SetAsLastSibling ();

        targetCanvasGroup = targetObject.GetComponent<CanvasGroup> ();
        if (targetCanvasGroup)
            targetCanvasGroup.blocksRaycasts = false;

        isDragging = true;
        fromIndex = _fromIndex;
        dragItemID = _dragItemID;
        dragItemAmount = _dragItemAmount;
        fromMaster = _fromMaster;
    }

    public static void OnDrag ()
    {
        targetObject.transform.position = Input.mousePosition + offset;
    }

    public static void OnEndDrag ()
    {
        // This is only ever called if end drag did not hit something with a valid OnDrop interface
        if (isDragging)
        {
            if(EventSystem.current.IsPointerOverGameObject() == false)
            {
                EntityManager.instance.PlayerInventory.RemoveItem ( dragItemID, dragItemAmount );
            }

            Reset ();
        }        
    }

    private static void Reset ()
    {
        targetObject.transform.position = originalPosition;
        targetObject.SetParent ( originalParent );

        originalPosition = Vector3.zero;
        offset = Vector3.zero;

        originalParent = null;

        if (targetCanvasGroup)
        {
            targetCanvasGroup.blocksRaycasts = true;
            targetCanvasGroup = null;
        }

        targetObject = null;

        isDragging = false;
        fromIndex = -1;
        dragItemID = -1;
        dragItemAmount = -1;
    }

    public static void OnDrop (int _toIndex, Master _toMaster)
    {
        if (!isDragging) return;

        if (fromMaster == Master.ItemContainer)
        {
            switch (_toMaster)
            {
                case Master.PlayerInventory:
                    ItemContainerCanvas.instance.targetInventory.SendTo ( EntityManager.instance.PlayerInventory, dragItemID, dragItemAmount );
                    break;
                case Master.ItemContainer:
                    ItemContainerCanvas.instance.targetInventory.SwitchIndices ( fromIndex, _toIndex );
                    break;
            }
        }
        else if (fromMaster == Master.PlayerInventory)
        {
            switch (_toMaster)
            {
                case Master.PlayerInventory:
                    EntityManager.instance.PlayerInventory.SwitchIndices ( fromIndex, _toIndex );
                    break;
                case Master.ItemContainer:
                    ItemBaseData item = null;

                    if (ItemDatabase.GetItem ( dragItemID, out item ))
                    {
                        if (item.IsSoulbound)
                        {
                            Debug.LogError ( "Cannot remove a soulbound item from your inventory - perhaps give a option to destroy if we implement a way to get it back garaunteed" );
                        }
                        else
                        {
                            EntityManager.instance.PlayerInventory.SendTo ( ItemContainerCanvas.instance.targetInventory, dragItemID, dragItemAmount );
                        }
                    }
                    break;
            }
        }

        Reset ();
    }
}
