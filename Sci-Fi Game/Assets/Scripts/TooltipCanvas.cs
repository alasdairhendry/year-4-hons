using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipCanvas : MonoBehaviour
{
    public static TooltipCanvas instance;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        canvasRect = GetComponent<RectTransform> ();
    }

    private bool isActive = false;
    private string currentTooltip = "";
    private RectTransform canvasRect;
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Vector2 displayOffset = new Vector2 ();
    [SerializeField] private float damping = 5.0f;
    private Vector2 targetPosition = new Vector2 ();

    private Vector2 boundsMin = new Vector2 ( 32.0f, 64.0f );
    private Vector2 boundsMax = new Vector2 ( 1920.0f, 1080.0f);

    public void Refresh ()
    {
        if (EventSystem.current.IsPointerOverGameObject ())
        {
            PointerEventData pointerData = new PointerEventData ( EventSystem.current )
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult> ();

            EventSystem.current.RaycastAll ( pointerData, results );

            if (results.Count > 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    if (results[i].gameObject.GetComponent<TooltipItemUI> ())
                    {
                        results[i].gameObject.GetComponent<TooltipItemUI> ().ShowTooltip ();
                        return;
                    }
                }
            }
        }

        HideTooltip ();
    }

    public void ShowTooltip (string message)
    {
        if (currentTooltip == message) return;
        if (DropdownMenuCanvas.instance.IsActive) { HideTooltip (); return; }
        targetPosition = new Vector2 ( Input.mousePosition.x, Input.mousePosition.y ) + displayOffset;
        tooltipRect.anchoredPosition = targetPosition;
        isActive = true;
        currentTooltip = message;
        tooltipRect.gameObject.SetActive ( true );

        tooltipText.text = currentTooltip;
    }

    private void Update ()
    {
        if (!isActive) return;

        CheckInput (); 

        boundsMax.x = canvasRect.sizeDelta.x - tooltipRect.sizeDelta.x - boundsMin.x;
        boundsMax.y = canvasRect.sizeDelta.y - tooltipRect.sizeDelta.y - (boundsMin.y * 2);

        targetPosition = new Vector2 ( Input.mousePosition.x, Input.mousePosition.y ) + displayOffset;
        targetPosition.x = Mathf.Clamp ( targetPosition.x, boundsMin.x, boundsMax.x );
        targetPosition.y = Mathf.Clamp ( targetPosition.y, boundsMin.y, boundsMax.y );

        tooltipRect.anchoredPosition = Vector2.Lerp ( tooltipRect.anchoredPosition, targetPosition, Time.deltaTime * damping );
    }

    private void CheckInput ()
    {
        if (Input.GetKeyDown ( KeyCode.LeftShift )) Refresh ();
        if (Input.GetKeyUp ( KeyCode.LeftShift )) Refresh ();

        if (Input.GetKeyDown ( KeyCode.LeftControl )) Refresh ();
        if (Input.GetKeyUp ( KeyCode.LeftControl )) Refresh ();

        if (Input.GetKeyDown ( KeyCode.LeftAlt )) Refresh ();
        if (Input.GetKeyUp ( KeyCode.LeftAlt )) Refresh ();

        if (Input.GetKeyDown ( KeyCode.Space )) Refresh ();
        if (Input.GetKeyUp ( KeyCode.Space )) Refresh ();

        if (Input.GetMouseButtonDown ( 0 )) Refresh ();
        if (Input.GetMouseButtonDown ( 1 )) Refresh ();
        if (Input.GetMouseButtonDown ( 2 )) Refresh ();

        if (Input.GetMouseButtonUp ( 0 )) Refresh ();
        if (Input.GetMouseButtonUp ( 1 )) Refresh ();
        if (Input.GetMouseButtonUp ( 2 )) Refresh ();
    }

    public void HideTooltip ()
    {
        if (!isActive) return;

        isActive = false;
        currentTooltip = "";
        tooltipText.text = "";
        tooltipRect.gameObject.SetActive ( false );
    }
}
