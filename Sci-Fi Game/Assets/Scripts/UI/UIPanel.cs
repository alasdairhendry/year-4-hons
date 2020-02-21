using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private bool registeredInPanelHistory = true;
    public bool isOpened { get; protected set; }

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

    public virtual void Trigger ()
    {
        if (isOpened) Close ();
        else Open ();
    }
}
