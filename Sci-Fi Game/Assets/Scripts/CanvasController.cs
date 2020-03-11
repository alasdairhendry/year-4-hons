using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;
    private List<Canvas> canvases = new List<Canvas> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy ( this.gameObject );
    }

    private void Start ()
    {
        canvases = FindObjectsOfType<Canvas> ().ToList ();
    }

    private void Update ()
    {
        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField> () == null)
            EventSystem.current.SetSelectedGameObject ( null );
    }

    public void PullCanvasToFront(Canvas canvas)
    {
        int index = canvases.IndexOf ( canvas );

        for (int i = 0; i < canvases.Count; i++)
        {
            if (canvases[i] == canvas) continue;
            if (canvases[i].sortingOrder > index)
                canvases[i].sortingOrder--;
        }

        canvas.sortingOrder = canvases.Count - 1;
    }
}
