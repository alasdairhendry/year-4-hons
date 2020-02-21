using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldMapCanvas : UIPanel
{
    public static WorldMapCanvas instance;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    [SerializeField] private GameObject mainPanel;
    [Space]
    [SerializeField] private Transform legendPanel;
    [SerializeField] private GameObject legendEntryPrefab;
    [Space]
    [SerializeField] private Transform regionParentTransform;
    [SerializeField] private Vector2 scaleRange = new Vector2 ( 0.5f, 5.0f );
    [SerializeField] private float scaleSpeed = 5.0f;
    [SerializeField] private float scaleDamping = 10.0f;
    [Space]
    [SerializeField] private float panSpeed = 10.0f;
    [SerializeField] private float panDamping = 10.0f;
    [Space]
    [SerializeField] private Transform blipsParent;
    [SerializeField] private GameObject blipPrefab;
    [SerializeField] private float blipXScaler = 5.181765f;
    [SerializeField] private float blipYScaler = 4.898892f;
    [SerializeField] private List<MapBlipTypeSpriteData> spriteData = new List<MapBlipTypeSpriteData> ();
    [SerializeField] private float scalePosModifier = 100.0f;
    private float currentScale = 1.0f;
    private float targetScale = 1.0f;
    private Vector3 currentPosition = new Vector3 ();
    private Vector3 targetPosition = new Vector3 ();

    private Dictionary<WorldMapObject, WorldMapBlip> blipsDict = new Dictionary<WorldMapObject, WorldMapBlip> ();
    private List<WorldMapBlip> blipsList = new List<WorldMapBlip> ();

    public List<MapBlipTypeSpriteData> SpriteData { get => spriteData; protected set => spriteData = value; }
    [SerializeField] private CanvasGroup toHide;

    private void Start ()
    {
        Close ();
        currentPosition = regionParentTransform.transform.localPosition;
        targetPosition = regionParentTransform.transform.localPosition;

        for (int i = 0; i < spriteData.Count; i++)
        {
            GameObject go = Instantiate ( legendEntryPrefab );
            go.transform.SetParent ( legendPanel );
            go.transform.GetComponentInChildren<Image> ().sprite = spriteData[i].sprite;
            go.transform.GetComponentInChildren<TextMeshProUGUI> ().text = spriteData[i].legendTitle;
        }
    }

    public override void Open ()
    {
        base.Open ();
        isOpened = true;
        mainPanel.SetActive ( true );

        toHide.alpha = 0.0f;
        toHide.interactable = false;
        toHide.blocksRaycasts = false;
    }

    public override void Close ()
    {
        base.Close ();
        isOpened = false;
        mainPanel.SetActive ( false );
        toHide.alpha = 1.0f;
        toHide.interactable = true;
        toHide.blocksRaycasts = true;
    }

    private void Update ()
    {
        if (!isOpened) return;
        UpdateMapScale ();
        UpdateMapPosition ();


        if (Input.GetMouseButtonDown ( 0 ))
        {
            Debug.Log ( EventSystem.current.currentSelectedGameObject );
        }
        if (Input.GetMouseButtonUp ( 0 ))
        {
            Debug.Log ( EventSystem.current.currentSelectedGameObject );
        }
    }

    private void LateUpdate ()
    {
        if (!isOpened) return;
        UpdateBlipPositions ();
    }

    private void UpdateMapScale ()
    {
        targetScale += Input.GetAxis ( "Mouse ScrollWheel" ) * scaleSpeed * Time.deltaTime * currentScale;
        targetScale = Mathf.Clamp ( targetScale, scaleRange.x, scaleRange.y );

        currentScale = Mathf.Lerp ( currentScale, targetScale, Time.deltaTime * scaleDamping );
        regionParentTransform.localScale = new Vector3 ( currentScale, currentScale, 1.0f );
    }

    private void UpdateMapPosition ()
    {
        if (Input.GetMouseButton ( 0 ) && EventSystem.current.IsPointerOverGameObject ())
        {
            targetPosition += new Vector3 ( Input.GetAxis ( "Mouse X" ), Input.GetAxis ( "Mouse Y" ), 0.0f ) * Time.deltaTime * panSpeed;
        }

        currentPosition = Vector3.Slerp ( currentPosition, targetPosition, Time.deltaTime * panDamping );
        regionParentTransform.localPosition = currentPosition;
    }    

    public void RegisterWorldMapObject (MapBlipType blipType, Transform target, WorldMapObject worldMapObject)
    {
        if (blipsDict.ContainsKey ( worldMapObject ))
        {
            Debug.LogError ( "Blip already exists" );
            return;
        }

        MapBlipTypeSpriteData data = spriteData.First ( x => x.blipType == blipType );
        Sprite sprite = data.sprite;
        WorldMapBlip wmb = new WorldMapBlip ( target, worldMapObject );

        GameObject go = Instantiate ( blipPrefab );
        go.GetComponent<TooltipItemUI> ().SetTooltipMessage ( data.legendTitle );
        go.transform.SetParent ( blipsParent );
        go.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( 32.0f, 32.0f );
        go.transform.GetComponent<Image> ().sprite = sprite;
        go.transform.localScale = Vector3.one;
        wmb.blipGameObject = go;

        blipsDict.Add ( worldMapObject, wmb );
        blipsList.Add ( wmb );
    }

    public void UnregisterWorldMapObject(WorldMapObject worldMapObject)
    {
        if (blipsDict.ContainsKey ( worldMapObject ))
        {
            blipsList.Remove ( blipsDict[worldMapObject] );
            Destroy ( blipsDict[worldMapObject].blipGameObject );
            blipsDict.Remove ( worldMapObject );
        }
    }

    private void UpdateBlipPositions ()
    {
        for (int i = 0; i < blipsList.Count; i++)
        {
            if(blipsList[i].target == null)
            {
                // TODO - REmove this item
                continue;
            }

            blipsList[i].blipGameObject.transform.localPosition = new Vector3 ( blipsList[i].target.position.x * blipXScaler, blipsList[i].target.position.z * blipYScaler, 0.0f );
            blipsList[i].blipGameObject.transform.localScale = Vector3.one / Mathf.Clamp ( currentScale, 1, scaleRange.y );

            if (blipsList[i].worldMapObject.IncludeRotation)
            {
                blipsList[i].blipGameObject.transform.localEulerAngles = new Vector3 ( 0.0f, 0.0f, -blipsList[i].target.localEulerAngles.y );
            }
        }
    }
}

public class WorldMapBlip
{
    public Transform target;
    public GameObject blipGameObject;
    public WorldMapObject worldMapObject;

    public WorldMapBlip (Transform target, WorldMapObject worldMapObject)
    {
        this.target = target;
        this.worldMapObject = worldMapObject;
    }
}

[System.Serializable]
public class MapBlipTypeSpriteData
{
    public MapBlipType blipType;
    public Sprite sprite;
    public string legendTitle;
}
