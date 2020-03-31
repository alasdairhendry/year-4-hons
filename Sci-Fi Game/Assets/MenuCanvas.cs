using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCanvas : UIPanel
{
    [SerializeField] private GameObject mainPanel;

    private void Awake ()
    {
        Close ( true );
    }

    public override void Open ()
    {
        base.Open ();
        isOpened = true;
        mainPanel.SetActive ( true );
        UIPanelController.instance.OnPanelOpened ( this );
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        isOpened = false;
        mainPanel.SetActive ( false );
        UIPanelController.instance.OnPanelClosed ( this );
    }

    public void OnClick_ResetUI ()
    {
        DraggableWindow[] draggableWindows = FindObjectsOfType<DraggableWindow> ();

        for (int i = 0; i < draggableWindows.Length; i++)
        {
            draggableWindows[i].ResetPosition ();
        }
    }

    public void OnClick_Quit ()
    {
        LoadingManager.instance.LoadScene ( 1 );
    }
}
