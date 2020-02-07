using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelController : MonoBehaviour
{
    public static UIPanelController instance;
    private List<UIPanel> panelHistory = new List<UIPanel> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    private void Update ()
    {
        if (Input.GetKeyDown ( KeyCode.Escape ))
        {
            if(panelHistory.Count > 0)
            {
                panelHistory[panelHistory.Count - 1].Close ();
            }
        }
    }

    public void RegisterPanelHistory(UIPanel panel)
    {
        if (panelHistory.Contains ( panel ))
        {
            panelHistory.Remove ( panel );
        }

        panelHistory.Add ( panel );
    }

    public void UnregisterPanelHistory(UIPanel panel)
    {
        if (panelHistory.Contains ( panel ))
        {
            panelHistory.Remove ( panel );
        }
    }
}
