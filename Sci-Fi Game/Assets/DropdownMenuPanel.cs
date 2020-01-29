using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropdownMenuPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
    {
        Debug.Log ( "Enter" );
    }

    void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
    {
        DropdownMenuCanvas.instance.Hide ();
    }
}
