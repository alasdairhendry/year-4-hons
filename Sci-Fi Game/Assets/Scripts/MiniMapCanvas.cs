using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapCanvas : MonoBehaviour
{
    public static MiniMapCanvas instance;

    [SerializeField] private RectTransform mapParent;
    [SerializeField] private Transform blipsParent;
    [SerializeField] private GameObject blipPrefab;
    [Space]
    [SerializeField] private float blipXScaler = 5.181765f;
    [SerializeField] private float blipYScaler = 4.898892f;
    [Space]
    [SerializeField] private TextMeshProUGUI locationText;

    private Dictionary<WorldMapObject, MiniMapBlip> blipsDict = new Dictionary<WorldMapObject, MiniMapBlip> ();
    private List<MiniMapBlip> blipsList = new List<MiniMapBlip> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    private void LateUpdate ()
    {
        UpdateMapPosition ();
        UpdateBlipPositions ();
    }

    private void UpdateMapPosition ()
    {
        Vector3 playerPosition = EntityManager.instance.PlayerCharacter.transform.position;
        mapParent.anchoredPosition3D = new Vector3 ( playerPosition.x * blipXScaler * -1.0f, playerPosition.z * blipYScaler * -1.0f, 0.0f );
    }

    private void UpdateBlipPositions ()
    {
        for (int i = 0; i < blipsList.Count; i++)
        {
            if (blipsList[i].target == null)
            {
                // TODO - REmove this item
                continue;
            }

            //blipsList[i].blipGameObject.transform.localPosition = new Vector3 ( blipsList[i].target.position.x * blipXScaler, blipsList[i].target.position.z * blipYScaler, 0.0f );

            if(blipsList[i].miniMapObject.MapBlipType != MapBlipType.Player)
            {
                blipsList[i].blipGameObject.transform.localPosition = new Vector3 ( blipsList[i].target.position.x * blipXScaler, blipsList[i].target.position.z * blipYScaler, 0.0f );
            }

            if (blipsList[i].miniMapObject.IncludeRotation)
            {
                blipsList[i].blipGameObject.transform.localEulerAngles = new Vector3 ( 0.0f, 0.0f, -blipsList[i].target.localEulerAngles.y );
            }
        }
    }

    public void UpdateLocationText(string location)
    {
        locationText.text = location;
    }

    public void RegisterWorldMapObject (MapBlipType blipType, Transform target, WorldMapObject worldMapObject)
    {
        if (blipsDict.ContainsKey ( worldMapObject ))
        {
            Debug.LogError ( "Blip already exists" );
            return;
        }

        MapBlipTypeSpriteData data = WorldMapCanvas.instance.SpriteData.First ( x => x.blipType == blipType );
        Sprite sprite = data.sprite;
        MiniMapBlip wmb = new MiniMapBlip ( target, worldMapObject );

        GameObject go = Instantiate ( blipPrefab );
        go.GetComponent<TooltipItemUI> ().SetTooltipMessage ( data.legendTitle );

        if (data.blipType == MapBlipType.Player)
            go.transform.SetParent ( mapParent.parent );
        else
            go.transform.SetParent ( blipsParent );

        go.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( 32.0f, 32.0f );
        go.transform.GetComponent<Image> ().sprite = sprite;
        go.transform.localScale = Vector3.one;
        go.transform.localEulerAngles = Vector3.zero;
        go.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;
        wmb.blipGameObject = go;

        blipsDict.Add ( worldMapObject, wmb );
        blipsList.Add ( wmb );
    }

    public void UnregisterWorldMapObject (WorldMapObject worldMapObject)
    {
        if (blipsDict.ContainsKey ( worldMapObject ))
        {
            blipsList.Remove ( blipsDict[worldMapObject] );
            Destroy ( blipsDict[worldMapObject].blipGameObject );
            blipsDict.Remove ( worldMapObject );
        }
    }
}

public class MiniMapBlip
{
    public Transform target;
    public GameObject blipGameObject;
    public WorldMapObject miniMapObject;

    public MiniMapBlip (Transform target, WorldMapObject miniMapObject)
    {
        this.target = target;
        this.miniMapObject = miniMapObject;
    }
}
