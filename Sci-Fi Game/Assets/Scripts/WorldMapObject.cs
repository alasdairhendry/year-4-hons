using UnityEngine;

public enum MapBlipType { Player, Enemy, TollBarrier, Quest, GeneralShop, WeaponShop, IngredientShop, Furnace, Laboratory, Oven, RepairBench, GunsmithsStation }

public class WorldMapObject : MonoBehaviour
{
    [SerializeField] private MapBlipType mapBlipType;
    [SerializeField] private bool registerOnAwake = true;
    [SerializeField] private bool includeRotation = false;

    public bool IncludeRotation { get => includeRotation; protected set => includeRotation = value; }
    public MapBlipType MapBlipType { get => mapBlipType; protected set => mapBlipType = value; }

    private void Start ()
    {
        if (registerOnAwake)
            Register ();
    }

    public void Register ()
    {
        WorldMapCanvas.instance.RegisterWorldMapObject ( mapBlipType, this.transform, this );
        MiniMapCanvas.instance.RegisterWorldMapObject ( mapBlipType, this.transform, this );
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