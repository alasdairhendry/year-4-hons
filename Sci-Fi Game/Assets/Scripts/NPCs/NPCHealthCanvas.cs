using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealthCanvas : MonoBehaviour
{
    public static NPCHealthCanvas instance;
    [SerializeField] private GameObject healthIndicatorPrefab;
    public RectTransform canvasRect { get; protected set; }

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }

        canvasRect = GetComponent<Canvas> ().GetComponent<RectTransform> ();
    }

    public GameObject SpawnHealthIndicator (Transform targetTransform)
    {
        GameObject go = Instantiate ( healthIndicatorPrefab );
        go.transform.SetParent ( this.transform );
        go.transform.position = targetTransform.position;
        go.transform.localScale = Vector3.one;
        return go;
    }
}
