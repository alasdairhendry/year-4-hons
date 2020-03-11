using UnityEngine;

public enum MapBlipType { Player, Enemy, TollBarrier, Quest, GeneralShop, WeaponShop, IngredientShop, Furnace, Laboratory, Oven, RepairBench, GunsmithsStation, AttachmentShop, ToolShop, MedicalShop, Bank }

public class WorldMapObject : MonoBehaviour
{
    [SerializeField] private MapBlipType mapBlipType;
    [SerializeField] private bool registerOnAwake = true;
    [SerializeField] private bool includePosition = false;
    [SerializeField] private bool includeRotation = false;

    public CityRegions Region { get; protected set; } = CityRegions.None;
    public bool IncludePosition { get => includePosition; protected set => includePosition = value; }
    public bool IncludeRotation { get => includeRotation; protected set => includeRotation = value; }
    public MapBlipType MapBlipType { get => mapBlipType; protected set => mapBlipType = value; }

    public string overrideName = "";

    private void Start ()
    {
        RefreshRegion ();

        if (registerOnAwake)
            Register ();
    }

    public void RefreshRegion ()
    {
        Region = CityRegionController.GetRegion ( transform.position + Vector3.up );
    }

    public void Register (string _overrideName = "")
    {
        WorldMapCanvas.instance.RegisterWorldMapObject ( mapBlipType, this.transform, this );
        MiniMapCanvas.instance.RegisterWorldMapObject ( mapBlipType, this.transform, this );

        if (!string.IsNullOrEmpty ( _overrideName ))
            overrideName = _overrideName;
    }

    public void Unregister ()
    {
        WorldMapCanvas.instance?.UnregisterWorldMapObject ( this );
        MiniMapCanvas.instance?.UnregisterWorldMapObject ( this );
    }

    private void OnDestroy ()
    {
        Unregister ();
    }
}