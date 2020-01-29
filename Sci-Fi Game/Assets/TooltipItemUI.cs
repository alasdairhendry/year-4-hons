﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string tooltip = "";
    private System.Func<string> getTooltipAction;
    private bool showingTooltip = false;

    void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
    {
        ShowTooltip ();
    }

    void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
    {
        DisableTooltip ();
    }

    private void ShowTooltip ()
    {
        if (string.IsNullOrEmpty ( tooltip ) && getTooltipAction == null)
        {
            if (showingTooltip)
            {
                DisableTooltip ();
            }

            return;
        }

        if (getTooltipAction != null)
        {
            tooltip = getTooltipAction ();
        }
        else
        {
            tooltip = "";
        }

        if (!InventoryItemInteraction.IsCurrentlyInteracting)
        { 
            TooltipCanvas.instance.ShowTooltip ( tooltip );
        }

        showingTooltip = true;
    }

    private void DisableTooltip ()
    {
        if (InventoryItemInteraction.IsCurrentlyInteracting) return;
        TooltipCanvas.instance.HideTooltip ();
        showingTooltip = false;
    }

    public void SetTooltipAction (System.Func<string> action)
    {
        if(action == null)
        {
            tooltip = "";
        }
        else
        {
            tooltip = action?.Invoke ();
        }
        getTooltipAction = action;
    }

    public void SetTooltipMessage(string message)
    {
        tooltip = message;

        if (showingTooltip)
        {
            ShowTooltip ();
        }
    }
}
