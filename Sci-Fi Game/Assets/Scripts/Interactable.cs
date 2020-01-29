using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent ( typeof ( Collider ) )]
public class Interactable : MonoBehaviour
{
    public enum InteractableType { NPC, Cooker }
    [SerializeField] private InteractableType interactableType = InteractableType.NPC;
    [SerializeField] private Mouse.CursorType cursorType = Mouse.CursorType.Interact;

    public Mouse.CursorType GetCursorType { get => cursorType; }
    public InteractableType GetInteractableType { get => interactableType; }

    [SerializeField] private string interactType = "Talk to";
    [SerializeField] private string interactableName = "Interactable Object";
    [Space]
    [SerializeField] private UnityEvent onInteractEvent;
    [Space]
    [SerializeField] private float squaredInteractionRadius = 4.0f;
    [SerializeField] private bool mustFaceObject = true;
    [SerializeField] private Vector3 localPosition = new Vector3 ();
    [SerializeField] private bool isInteractable;

    public System.Action OnInteractAction { get; protected set; }

    public string GetDescriptiveName { get { return "<b>" + interactType + "</b>" + " " + interactableName; } }
    public string GetName { get { return interactableName; } }

    public float SquaredInteractionRadius { get => squaredInteractionRadius; set => squaredInteractionRadius = value; }
    public bool MustFaceObject { get => mustFaceObject; set => mustFaceObject = value; }
    public Vector3 UIWorldPosition { get => transform.TransformPoint ( localPosition ); }
    public bool IsInteractable { get => isInteractable; set => isInteractable = value; }

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
    }

    public void OnPointerEnter ()
    {
        InventoryItemInteraction.OnWorldInteractableHovered ( this );
    }

    public void OnPointerExit ()
    {
        InventoryItemInteraction.OnWorldInteractableUnhovered (this);
    }

    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere ( UIWorldPosition, 0.1f );
    }  
}
