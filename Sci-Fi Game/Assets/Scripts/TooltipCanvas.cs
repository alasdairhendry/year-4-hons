using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private Vector2 boundsMin = new Vector2 ( 32.0f, 32.0f );
    private Vector2 boundsMax = new Vector2 ( 1920.0f, 1080.0f );

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
        if (Input.GetKeyDown ( KeyCode.F ))
        {
            EntityManager.instance.PlayerInventory.AddCoins ( 1 );
        }

        if (Input.GetKeyDown ( KeyCode.G ))
        {
            EntityManager.instance.PlayerInventory.RemoveCoins ( 1 );
        }


        if (!isActive) return;

        boundsMax.x = canvasRect.sizeDelta.x - tooltipRect.sizeDelta.x - boundsMin.x;
        boundsMax.y = canvasRect.sizeDelta.y - tooltipRect.sizeDelta.y - boundsMin.y;

        targetPosition = new Vector2 ( Input.mousePosition.x, Input.mousePosition.y ) + displayOffset;
        targetPosition.x = Mathf.Clamp ( targetPosition.x, boundsMin.x, boundsMax.x );
        targetPosition.y = Mathf.Clamp ( targetPosition.y, boundsMin.y, boundsMax.y );

        tooltipRect.anchoredPosition = Vector2.Lerp ( tooltipRect.anchoredPosition, targetPosition, Time.deltaTime * damping );
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
