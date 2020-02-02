using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class InventoryItemInteraction : MonoBehaviour
{
    private static InventoryItemInteraction instance;

    //    //public static bool IsCurrentlyInteracting { get => currentlySelectedItem != null; }
    //    //private static ItemBaseData currentlySelectedItem = null;
    //    //private static int currentlySelectedItemIndex = -1;

    //    //[SerializeField] private List<ItemInteractionPair> interactionItemPairs = new List<ItemInteractionPair> ();
    //    //private Dictionary<int, List<ItemInteractionPair>> itemInteractionPairDictionary = new Dictionary<int, List<ItemInteractionPair>> ();
    //    //private bool interactedThisFrame = false;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    //        //for (int i = 0; i < interactionItemPairs.Count; i++)
    //        //{
    //        //    if (itemInteractionPairDictionary.ContainsKey ( interactionItemPairs[i].primaryItemID ))
    //        //    {
    //        //        itemInteractionPairDictionary[interactionItemPairs[i].primaryItemID].Add ( interactionItemPairs[i] );
    //        //    }
    //        //    else
    //        //    {
    //        //        itemInteractionPairDictionary.Add ( interactionItemPairs[i].primaryItemID, new List<ItemInteractionPair> () { interactionItemPairs[i] } );
    //        //    }

    //        //    if (interactionItemPairs[i].multiDirectional)
    //        //    {
    //        //        if (itemInteractionPairDictionary.ContainsKey ( interactionItemPairs[i].secondaryItemID ))
    //        //        {
    //        //            itemInteractionPairDictionary[interactionItemPairs[i].secondaryItemID].Add ( interactionItemPairs[i].CloneInverted () );
    //        //        }
    //        //        else
    //        //        {
    //        //            itemInteractionPairDictionary.Add ( interactionItemPairs[i].secondaryItemID, new List<ItemInteractionPair> () { interactionItemPairs[i].CloneInverted () } );
    //        //        }
    //        //    }
    //        //}
    //    }

    //    private void Update ()
    //    {
    //        //if (Mouse.Click ( 1 ))
    //        //{
    //        //    StopInteraction ();
    //        //}
    //    }

    //    private void LateUpdate ()
    //    {
    //        //if (Mouse.Click ( 0 ))
    //        //{
    //        //    if (!interactedThisFrame)
    //        //    {
    //        //        StopInteraction ();
    //        //    }
    //        //}

    //        //interactedThisFrame = false;
    //    }

    private static bool CheckInstance ()
    {
        if (instance == null) { Debug.LogError ( "Instance does not exist" ); return false; }
        return true;
    }

    public static void OnInventoryItemHovered (int itemID)
    {
        if (!CheckInstance ()) return;
        if (itemID < 0) return;

        ItemBaseData item = ItemDatabase.GetItem ( itemID );
        if (item == null) return;

        //if (currentlySelectedItem != null)
        //{
        //TooltipCanvas.instance.ShowTooltip ( string.Format ( "Use {0} on {1}", ColourHelper.TagColour ( currentlySelectedItem.Name, ColourDescription.OffWhiteText ), ColourHelper.TagColour ( item.Name, ColourDescription.OffWhiteText ) ) );
        //}
    }

    public static void OnInventoryItemUnhovered (int itemID)
    {
        if (!CheckInstance ()) return;

        //if (currentlySelectedItem != null)
        //{
        //    TooltipCanvas.instance.ShowTooltip ( string.Format ( "Use {0}", ColourHelper.TagColour ( currentlySelectedItem.Name, ColourDescription.OffWhiteText ) ) );
        //}
    }

    //    public static void OnWorldInteractableHovered (Interactable interactable)
    //    {
    //        if (!CheckInstance ()) return;

    //        //if (currentlySelectedItem != null)
    //        //{
    //        //    TooltipCanvas.instance.ShowTooltip ( string.Format ( "Use {0} on {1}", ColourHelper.TagColour ( currentlySelectedItem.Name, ColourDescription.OffWhiteText ), ColourHelper.TagColour ( interactable.GetName, ColourDescription.OffWhiteText ) ) );
    //        //}
    //    }

    //    public static void OnWorldInteractableUnhovered (Interactable interactable)
    //    {
    //        if (!CheckInstance ()) return;

    //        //if (currentlySelectedItem != null)
    //        //{
    //        //    TooltipCanvas.instance.ShowTooltip ( string.Format ( "Use {0}", ColourHelper.TagColour ( currentlySelectedItem.Name, ColourDescription.OffWhiteText ) ) );
    //        //}
    //    }

    //    public static void OnClickInventoryItem (int itemID/*, int inventoryIndex*/)
    //    {
    //        if (!CheckInstance ()) return;

    //        //ItemBaseData item = ItemDatabase.GetItem ( itemID );
    //        //if (item == null) return;

    //        //int inventoryIndex = EntityManager.instance.PlayerInventory.GetIndexOfItem ( itemID );

    //        //if (inventoryIndex < 0) Debug.LogError ( "Ehh what" );

    //        //if (currentlySelectedItem == null)
    //        //{
    //        //    currentlySelectedItem = item;
    //        //    currentlySelectedItemIndex = inventoryIndex;
    //        //    TooltipCanvas.instance.ShowTooltip ( string.Format ( "Use {0}", ColourHelper.TagColour ( currentlySelectedItem.Name, ColourDescription.OffWhiteText ) ) );
    //        //}
    //        //else
    //        //{
    //        //    CheckInteraction ( itemID , inventoryIndex);
    //        //    TooltipCanvas.instance.HideTooltip ();
    //        //}

    //        //instance.interactedThisFrame = true;
    //    }

    //    public static void OnClickInteractableItem (Interactable.InteractableType interactableType)
    //    {
    //        if (!CheckInstance ()) return;

    //        //if (currentlySelectedItem != null)
    //        //{
    //        //    CheckInteraction ( interactableType );
    //        //    TooltipCanvas.instance.HideTooltip ();
    //        //    instance.interactedThisFrame = true;
    //        //}
    //    }

    //    private static void StopInteraction ()
    //    {
    //        //if (IsCurrentlyInteracting)
    //        //{
    //        //    currentlySelectedItem = null;
    //        //    currentlySelectedItemIndex = -1;
    //        //    TooltipCanvas.instance.HideTooltip ();
    //        //}
    //    }

    //    private static void CheckInteraction (int itemID, int index)
    //    {
    //        if (!CheckInstance ()) return;
    //        //if (currentlySelectedItem == null) return;
    //        //if (!instance.itemInteractionPairDictionary.ContainsKey ( currentlySelectedItem.ID ))
    //        //{
    //        //    Debug.Log ( "No interaction set up for " + currentlySelectedItem.ID + " and " + itemID );
    //        //    StopInteraction ();
    //        //    return;
    //        //}

    //        ItemBaseData item = ItemDatabase.GetItem ( itemID );
    //        if (item == null) return;

    //        //Debug.Log ( "Using " + currentlySelectedItem.Name + " on " + item.Name );

    //        //List<ItemInteractionPair> pairs = instance.itemInteractionPairDictionary[currentlySelectedItem.ID];

    //        //if (instance.itemInteractionPairDictionary.ContainsKey ( itemID ))
    //        //    pairs.AddRange ( instance.itemInteractionPairDictionary[itemID] );

    //        //bool doneInteraction = false;

    //        //for (int i = 0; i < pairs.Count; i++)
    //        //{
    //        //    if (!pairs[i].usesWorldInteraction && pairs[i].primaryItemID == currentlySelectedItem.ID && pairs[i].secondaryItemID == itemID)
    //        //    {
    //        //        pairs[i].DoInteraction ( Mathf.Min ( index, currentlySelectedItemIndex ) );
    //        //        doneInteraction = true;
    //        //        break;
    //        //    }
    //        //}

    //        //if (!doneInteraction)
    //        //{
    //        //    Debug.Log ( "No interaction set up for " + currentlySelectedItem.ID + " and " + itemID );
    //        //}

    //        //StopInteraction ();
    //    }

    //    private static void CheckInteraction (Interactable.InteractableType interactableType)
    //    {
    //        if (!CheckInstance ()) return;
    //        if (currentlySelectedItem == null) return;
    //        if (!instance.itemInteractionPairDictionary.ContainsKey ( currentlySelectedItem.ID ))
    //        {
    //            Debug.Log ( "No interaction set up for " + currentlySelectedItem.ID + " and " + interactableType );
    //            StopInteraction ();
    //            return;
    //        }

    //        Debug.Log ( "Using " + currentlySelectedItem.Name + " on " + interactableType );

    //        List<ItemInteractionPair> pairs = instance.itemInteractionPairDictionary[currentlySelectedItem.ID];

    //        bool doneInteraction = false;

    //        for (int i = 0; i < pairs.Count; i++)
    //        {
    //            if (pairs[i].usesWorldInteraction && pairs[i].primaryItemID == currentlySelectedItem.ID && pairs[i].interactableType == interactableType)
    //            {
    //                pairs[i].DoInteraction (currentlySelectedItemIndex);
    //                doneInteraction = true;
    //                break;
    //            }
    //        }

    //        if (!doneInteraction)
    //        {
    //            Debug.Log ( "No interaction set up for " + currentlySelectedItem.ID + " and " + interactableType );
    //        }

    //        StopInteraction ();
}

//    [System.Serializable]
//    public class ItemInteractionPair
//    {
//        /// <summary>
//        /// This class uses a property drawer. Renaming items may break the drawer.
//        /// </summary>
//#if UNITY_EDITOR
//        public bool foldout = false;
//#endif
//        public int  primaryItemID = 0;
//        public int  secondaryItemID = 0;

//        public bool usesWorldInteraction = false;
//        public Interactable.InteractableType interactableType = Interactable.InteractableType.NPC;

//        public bool removesPrimary = true;
//        public bool removesSecondary = true;
//        public bool multiDirectional = true;

//        public List<int> resultingItemIDs = new List<int> ();

//        public bool resultsInAction = false;
//        public System.Action resultingAction;
//        public UnityEvent resultingUnityEvent;

//        public void DoInteraction (int primaryIndex)
//        {
//            if (resultsInAction)
//            {
//                if (resultingAction == null && resultingUnityEvent == null)
//                {
//                    Debug.LogError ( string.Format ( "ItemInteractionPairAction {0} + {1} does not invoke a method", primaryItemID, secondaryItemID ) );
//                }
//                else
//                {
//                    CheckItemRemovals ();

//                    resultingAction?.Invoke ();
//                    resultingUnityEvent?.Invoke ();
//                }
//            }
//            else
//            {
//                CheckItemRemovals ();

//                for (int i = 0; i < resultingItemIDs.Count; i++)
//                {
//                    if (ItemDatabase.GetItem ( resultingItemIDs[i] ) == null)
//                    {
//                        Debug.LogError ( string.Format ( "ItemInteractionPairItem {0} + {1} does not return a valid item {2}", primaryItemID, secondaryItemID, resultingItemIDs ) );
//                    }
//                    else
//                    {
//                        if (primaryIndex == -1)
//                            EntityManager.instance.PlayerInventory.AddItem ( resultingItemIDs[i] );
//                        else
//                            EntityManager.instance.PlayerInventory.AddItem ( resultingItemIDs[i], 1, false, primaryIndex );
//                    }
//                }
//            }
//        }

//        protected void CheckItemRemovals ()
//        {
//            if (removesPrimary)
//                EntityManager.instance.PlayerInventory.RemoveItem ( primaryItemID );

//            if (removesSecondary && !usesWorldInteraction)
//                EntityManager.instance.PlayerInventory.RemoveItem ( secondaryItemID );
//        }

//        public ItemInteractionPair CloneInverted ()
//        {
//            ItemInteractionPair pair = new ItemInteractionPair ();

//            pair.foldout = foldout;

//            pair.primaryItemID = secondaryItemID;
//            pair.secondaryItemID = primaryItemID;

//            pair.usesWorldInteraction = usesWorldInteraction;
//            pair.interactableType = interactableType;

//            bool rPrimary = removesPrimary;
//            pair.removesPrimary = removesSecondary;
//            pair.removesSecondary = rPrimary;
//            pair.multiDirectional = multiDirectional;

//            pair.resultingItemIDs = resultingItemIDs;

//            pair.resultsInAction = resultsInAction;
//            pair.resultingAction = resultingAction;
//            pair.resultingUnityEvent = resultingUnityEvent;

//            return pair;
//        }
//    }
//}