using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent ( typeof ( Collider ) )]
public class Interactable : MonoBehaviour
{
    [SerializeField] private Mouse.CursorType cursorType = Mouse.CursorType.Interact;
    public Mouse.CursorType GetCursorType { get => cursorType; }

    [SerializeField] private string interactType = "Talk to";
    [SerializeField] private string interactableName = "Interactable Object";
    [SerializeField] private ColourDescription textColour = ColourDescription.InteractionDefault;
    [Space]
    [SerializeField] private UnityEvent onInteractEvent;
    [Space]
    [SerializeField] private float squaredInteractionRadius = 4.0f;
    [SerializeField] private bool mustFaceObject = true;
    [SerializeField] private Space space = Space.Self;
    [SerializeField] private Vector3 localPosition = new Vector3 ();
    [SerializeField] private bool isInteractable = true;

    public System.Action OnInteractAction { get;  set; }

    public string GetDescriptiveName { get { return "<b>" + interactType + "</b>" + " " + interactableName; } }
    public string GetName { get { return interactableName; } }

    public string initialInteractType { get; protected set; } = "";
    public string initialInteractableName { get; protected set; } = "";

    public float SquaredInteractionRadius { get => squaredInteractionRadius; set => squaredInteractionRadius = value; }
    public bool MustFaceObject { get => mustFaceObject; set => mustFaceObject = value; }
    public bool IsInteractable { get => isInteractable; set => isInteractable = value; }
    public string InteractType { get => interactType; set => interactType = value; }
    public ColourDescription TextColour { get => textColour; set => textColour = value; }

    public Vector3 UIWorldPosition ()
    {
        switch (space)
        {
            case Space.World:
                return transform.position + localPosition;
            case Space.Self:
                return transform.TransformPoint ( localPosition );
            default:
                return transform.position + localPosition;
        }
    }

    public void Interact ()
    {
        if (!isInteractable) return;

        onInteractEvent?.Invoke ();
        OnInteractAction?.Invoke ();
    }

    private void Awake ()
    {
        // TODO 
        //DialogueActor da = GetComponentInParent<DialogueActor> ();

        //if (da != null)
        //{
        //interactableName = da.GetActorName ();
        //}

        initialInteractType = interactType;
        initialInteractableName = interactableName;
    }

    public void SetInteractType (string interactType)
    {
        this.interactType = interactType;
    }

    public void ResetInteractType ()
    {
        interactType = initialInteractType;
    }

    public void SetInteractName (string interactName)
    {
        this.interactableName = interactName;
    }

    public void ResetInteractName ()
    {
        interactableName = initialInteractableName;
    }

    //public void OnPointerEnter ()
    //{
    //    InventoryItemInteraction.OnWorldInteractableHovered ( this );
    //}

    //public void OnPointerExit ()
    //{
    //    InventoryItemInteraction.OnWorldInteractableUnhovered (this);
    //}

    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere ( UIWorldPosition(), 0.1f );
    }
}
