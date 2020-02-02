using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private List<Canvas> canvases = new List<Canvas> ();

    private void Start ()
    {
        canvases = FindObjectsOfType<Canvas> ().ToList ();
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
