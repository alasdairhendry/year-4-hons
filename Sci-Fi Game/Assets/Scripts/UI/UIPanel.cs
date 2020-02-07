using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private bool registeredInPanelHistory = true;

    public virtual void Open ()
    {
        if (registeredInPanelHistory)
            UIPanelController.instance?.RegisterPanelHistory ( this );
    }

    public virtual void Close ()
    {
        if (registeredInPanelHistory)
            UIPanelController.instance?.UnregisterPanelHistory ( this );
    }
}
