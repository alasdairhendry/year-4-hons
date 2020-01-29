using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPortraitCamera : MonoBehaviour
{
    public static NPCPortraitCamera instance;
    private GameObject currentNPC;
    private int currentLayer = 0;
    [SerializeField] private LayerMask targetLayer;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    public void SetTarget (GameObject targetNPC, Transform targetBone, Vector3 offset, Vector3 euler)
    {
        if (currentNPC != null)
        {
            currentNPC.layer = currentLayer;
            currentNPC = null;
        }

        currentNPC = targetNPC;
        currentLayer = currentNPC.layer;
        currentNPC.layer = 10;

        transform.SetParent ( targetBone );
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        transform.GetChild ( 0 ).localPosition = offset;
        transform.GetChild ( 0 ).localEulerAngles = euler;
    }
}
