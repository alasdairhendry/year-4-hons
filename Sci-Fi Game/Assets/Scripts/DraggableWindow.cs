using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableWindow : MonoBehaviour, IPointerDownHandler
{
    void IPointerDownHandler.OnPointerDown (PointerEventData eventData)
    {
        if (!isDragging)
        {
            Cursor.visible = false;
            isDragging = true;
            FindObjectOfType<CanvasController> ().PullCanvasToFront ( GetComponentInParent<Canvas> () );
        }
    }

    [SerializeField] private RectTransform windowRect;
    private bool isDragging = false;

    private void Update ()
    {
        if (isDragging)
            if (Input.GetMouseButtonUp ( 0 ))
            {
                isDragging = false;
                Cursor.visible = true;
            }
    }

    private void LateUpdate ()
    {
        if (isDragging)
            windowRect.anchoredPosition3D += (new Vector3 ( Input.GetAxisRaw ( "Mouse X" ), Input.GetAxisRaw ( "Mouse Y" ), 0.0f ) ) / Time.deltaTime * 0.5f;
    }

    private Vector3 normaliseMousePosition (Vector3 position)
    {
        Vector3 n = new Vector3 ();

        n.x = Mathf.InverseLerp ( 0, Screen.width, position.x );
        n.y = Mathf.InverseLerp ( 0, Screen.height, position.y );
        n.z = position.z;

        return n;
    }
}
