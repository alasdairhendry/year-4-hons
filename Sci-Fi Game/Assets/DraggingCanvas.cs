using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggingCanvas : MonoBehaviour
{
    public static DraggingCanvas instance;

    [SerializeField] private Image sprite;
    [SerializeField] private Transform panel;
    [SerializeField] private CanvasGroup canvasGroup;

    public Image Sprite { get => sprite; set => sprite = value; }
    public Transform Panel { get => panel; set => panel = value; }
    public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }
}
