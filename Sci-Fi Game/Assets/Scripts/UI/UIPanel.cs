using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private bool registeredInPanelHistory = true;

    public Canvas Canvas { get => canvas; }
    public bool isOpened { get; protected set; }

    public virtual void Open ()
    {
        if (registeredInPanelHistory)
            UIPanelController.instance?.RegisterPanelHistory ( this );
    }

    public virtual void Close (bool bypassClosedCheck = false)
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
