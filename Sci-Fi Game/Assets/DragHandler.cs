using UnityEngine;
using UnityEngine.EventSystems;

public static class DragHandler
{
    public static bool isDragging = false;
    public enum Master { PlayerInventory, ItemContainer, Hotbar, Bank }
    public static Master fromMaster = Master.PlayerInventory;

    public static int fromIndex = -1;

    public static int dragItemID = -1;
    public static int dragItemAmount = -1;
    private static Vector3 offset;

    public static void OnBeginDrag (int _fromIndex, int _dragItemID, int _dragItemAmount, Master _fromMaster, Transform _targetObject, Sprite sprite)
    {
        if (isDragging) { Debug.Log ( 1 ); return; }
        if (!ItemDatabase.ItemExists ( _dragItemID )) { Debug.Log ( 2 ); return; }

        DraggingCanvas.instance.Sprite.sprite = sprite;
        DraggingCanvas.instance.CanvasGroup.alpha = 1;
        offset = _targetObject.position - Input.mousePosition;

        isDragging = true;
        fromIndex = _fromIndex;
        dragItemID = _dragItemID;
        dragItemAmount = _dragItemAmount;
        fromMaster = _fromMaster;
    }

    public static void OnDrag ()
    {
        DraggingCanvas.instance.Panel.position = Input.mousePosition + offset;
    }

    public static void OnEndDrag ()
    {
        // This is only ever called if end drag did not hit something with a valid OnDrop interface
        if (isDragging)
        {
            if (EventSystem.current.IsPointerOverGameObject () == false)
            {
                switch (fromMaster)
                {
                    case Master.PlayerInventory:
                        PlayerInventoryController.DropItemToFloorFromInventory ( dragItemID, dragItemAmount );
                        break;
                    case Master.ItemContainer:
                        PlayerInventoryController.DropItemToFloorFromContainer ( dragItemID, dragItemAmount );
                        break;
                    case Master.Hotbar:
                        PlayerInventoryController.DropHotbarEntryToFloor ( dragItemID, fromIndex );
                        break;
                }
            }

            Reset ();
        }
    }

    private static void Reset ()
    {
        DraggingCanvas.instance.CanvasGroup.alpha = 0;
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
                    PlayerInventoryController.SendItemFromContainerToInventory ( dragItemID, dragItemAmount );
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
                    PlayerInventoryController.SendItemFromInventoryToContainer ( dragItemID, dragItemAmount );
                    break;
                case Master.Hotbar:
                    PlayerInventoryController.AddHotbarEntry ( dragItemID, _toIndex );
                    break;
            }
        }
        else if (fromMaster == Master.Hotbar)
        {
            switch (_toMaster)
            {
                case Master.Hotbar:
                    PlayerInventoryController.SwitchHotbarIndices ( fromIndex, _toIndex );
                    break;
            }

        }

        Reset ();
    }
}