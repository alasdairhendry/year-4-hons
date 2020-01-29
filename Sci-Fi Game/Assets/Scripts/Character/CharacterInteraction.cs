using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDisplayRadius = 5.0f;
    [SerializeField] private float SquaredInteractionAvailableRadius;
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform interactionCanvas;

    private List<Collider> interactionsInDisplayRadius = new List<Collider> ();
    private Dictionary<Collider, GameObject> colliderToUIDictionary = new Dictionary<Collider, GameObject> ();
    private Ray ray = new Ray ();
    private RaycastHit hit = new RaycastHit ();
    private Camera mainCamera;

    public float SquaredInteractionDisplayRadius { get => interactionDisplayRadius * interactionDisplayRadius; }
    private Interactable currentlyHoveredInteractable = null;

    private void Start ()
    {
        mainCamera = Camera.main;
    }

    private void Update ()
    {
        GetDisplayRadiusObjects ();
        UpdateCurrentInteractions ();
        UpdateMouseCursors ();

        if (EventSystem.current.IsPointerOverGameObject ()) return;

        GetInteractionRequests ();                 
    }

    private void UpdateMouseCursors ()
    {
        if (EventSystem.current.IsPointerOverGameObject ())
        {
            Mouse.SetCursor ( Mouse.CursorType.Default );
            return;
        }

        ray = mainCamera.ScreenPointToRay ( Input.mousePosition );

        if (Physics.Raycast ( ray, out hit, 500, layerMask ))
        {
            Interactable interactable = hit.collider.gameObject.GetComponent<Interactable> ();

            if(currentlyHoveredInteractable != null)
            {
                currentlyHoveredInteractable.OnPointerExit ();
            }

            currentlyHoveredInteractable = interactable;
            interactable.OnPointerEnter ();

            if (interactable != null)
            {
                if (Extensions.SquaredDistance ( hit.collider.transform.position, transform.position ) > SquaredInteractionAvailableRadius)
                {
                    Mouse.SetCursor ( interactable.GetCursorType, true );
                }
                else
                {
                    Mouse.SetCursor ( interactable.GetCursorType );
                }
            }
            else
            {
                Mouse.SetCursor ( Mouse.CursorType.Default );
            }
        }
        else
        {
            Mouse.SetCursor ( Mouse.CursorType.Default );
        }
    }

    private void GetInteractionRequests ()
    {
        if (Mouse.Click ( 0 ) && EventSystem.current.IsPointerOverGameObject () == false)
        {
            ray = mainCamera.ScreenPointToRay ( Input.mousePosition );

            if (Physics.Raycast ( ray, out hit, 500, layerMask ))
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable> ();

                if (interactable != null)
                {
                    if (Extensions.SquaredDistance ( hit.collider.transform.position, transform.position ) <= SquaredInteractionAvailableRadius)
                    {
                        if (InventoryItemInteraction.IsCurrentlyInteracting)
                        {
                            InventoryItemInteraction.OnClickInteractableItem ( interactable.GetInteractableType );
                        }
                        else
                        {
                            interactable.Interact ();
                        }
                    }
                }
            }
        }

        if (Input.GetKeyUp ( KeyCode.E ) && EventSystem.current.IsPointerOverGameObject () == false)
        {
            ray = new Ray ( mainCamera.transform.position, mainCamera.transform.forward );

            if (Physics.Raycast ( ray, out hit, interactionDisplayRadius, layerMask ))
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable> ();

                if (interactable != null)
                {
                    interactable.Interact ();
                }
            }
        }
    }

    private void GetDisplayRadiusObjects ()
    {
        List<Collider> hits = Physics.OverlapSphere ( transform.position, interactionDisplayRadius, layerMask ).Where ( x => x.GetComponent<Interactable> () != null ).ToList ();

        List<Collider> unavailableHits = interactionsInDisplayRadius.Except ( hits ).ToList ();
        List<Collider> newHits = hits.Except ( interactionsInDisplayRadius ).ToList ();

        DestroyDisplays ( unavailableHits );
        CreateDisplays ( newHits );

        interactionsInDisplayRadius.Clear ();
        interactionsInDisplayRadius = hits;
    }

    private void UpdateCurrentInteractions ()
    {
        for (int i = 0; i < interactionsInDisplayRadius.Count; i++)
        {
            float squaredDistancePlayerToInteractable = Extensions.SquaredDistance ( transform.position, interactionsInDisplayRadius[i].transform.position );
            Interactable inter = interactionsInDisplayRadius[i].GetComponent<Interactable> ();

            //if (inter == null)
            //{
            //    Debug.Log ( "Wtf again" );
            //    continue;
            //}

            if (inter.IsInteractable)
            {
                //if (closestInteractable == null)
                //    closestInteractable = inter;
                //else
                //{
                //    if (dist < Extensions.SquaredDistance ( transform.position, closestInteractable.transform.position ))
                //    {
                //        if (colliderToUIDictionary.ContainsKey ( closestInteractable.GetComponent<Collider> () ))
                //            colliderToUIDictionary[closestInteractable.GetComponent<Collider> ()].GetComponentInChildren<TextMeshProUGUI> ().text = "";
                //        closestInteractable = inter;
                //    }
                //}

                ShowObject ( colliderToUIDictionary[interactionsInDisplayRadius[i]], inter.GetDescriptiveName );
                SetScale ( colliderToUIDictionary[interactionsInDisplayRadius[i]].transform, inter.SquaredInteractionRadius, squaredDistancePlayerToInteractable );
                UpdatePosition ( colliderToUIDictionary[interactionsInDisplayRadius[i]].transform, inter.UIWorldPosition );
            }
            else
            {
                HideObject ( colliderToUIDictionary[interactionsInDisplayRadius[i]] );
                SetScale ( colliderToUIDictionary[interactionsInDisplayRadius[i]].transform, inter.SquaredInteractionRadius, -1.0f );
                UpdatePosition ( colliderToUIDictionary[interactionsInDisplayRadius[i]].transform, inter.UIWorldPosition );
            }
        }

        //if (closestInteractable != null)
        //{
        //    if (Vector3.Distance ( transform.position, closestInteractable.transform.position ) <= closestInteractable.InteractionRadius)
        //    {
        //        if (colliderToUIDictionary.ContainsKey ( closestInteractable.GetComponent<Collider> () ))
        //            colliderToUIDictionary[closestInteractable.GetComponent<Collider> ()].GetComponentInChildren<TextMeshProUGUI> ().text = closestInteractable.GetName;
        //    }
        //    else
        //    {
        //        if (colliderToUIDictionary.ContainsKey ( closestInteractable.GetComponent<Collider> () ))
        //            colliderToUIDictionary[closestInteractable.GetComponent<Collider> ()].GetComponentInChildren<TextMeshProUGUI> ().text = "";
        //    }
        //}
    }

    private void ShowObject(GameObject go, string text)
    {
        go.GetComponentInChildren<TextMeshProUGUI> ().text = text;
        go.GetComponent<Animator> ().SetBool ( "show", true);
    }

    private void HideObject(GameObject go)
    {
        go.GetComponent<Animator> ().SetBool ( "show", false );
    }

    private void SetScale(Transform t, float interactionRadius, float distance)
    {
        if(distance >= 0.0f)
        {
            t.transform.GetChild ( 1 ).GetChild ( 0 ).localScale = Mathf.Lerp ( 0.5f, 1.5f, Mathf.InverseLerp ( SquaredInteractionAvailableRadius, interactionRadius, distance ) ) * Vector3.one;
        }
        else
        {
            t.GetChild ( 1 ).GetChild ( 0 ).localScale = Vector3.Slerp ( t.GetChild ( 1 ).GetChild ( 0 ).localScale, Vector3.zero, Time.deltaTime * 2.5f );
        }
    }

    private void UpdatePosition(Transform t, Vector3 position)
    {
        t.position = position;
    }

    private void DestroyDisplays (List<Collider> interactables)
    {
        for (int i = 0; i < interactables.Count; i++)
        {
            if (colliderToUIDictionary.ContainsKey ( interactables[i] ))
            {
                GameObject go = colliderToUIDictionary[interactables[i]];
                //colliderToUIDictionary.Remove ( interactables[i] );
                HideObject ( go );
                //Destroy ( go );
            }
        }
    }

    private void CreateDisplays(List<Collider> interactables)
    {
        for (int i = 0; i < interactables.Count; i++)
        {
            if (!colliderToUIDictionary.ContainsKey ( interactables[i] ))
            {
                GameObject go = Instantiate ( uiPrefab );
                go.transform.SetParent ( interactionCanvas );
                go.transform.localScale = Vector3.one;
                colliderToUIDictionary.Add ( interactables[i], go );
                ShowObject ( go, "" );
            }
        }
    }

    public bool IsInInteractionRadius(Collider collider)
    {
        return interactionsInDisplayRadius.Contains ( collider );
    }
}
