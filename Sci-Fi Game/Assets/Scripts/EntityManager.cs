using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        inventoryCanvas.SetTargetInventory ( PlayerInventory );
        MainCamera = Camera.main;
    }

    public Character PlayerCharacter { get; protected set; }
    public Inventory PlayerInventory = new Inventory ( 12, false, true );
    public PlayerCameraController CameraController { get; protected set; }
    public Camera MainCamera { get; protected set; }
    public InventoryCanvas InventoryCanvas { get => inventoryCanvas; protected set => inventoryCanvas = value; }
    public List<AudioClip> eatSoundEffects = new List<AudioClip> ();
    public List<AudioClip> drinkSoundEffects = new List<AudioClip> ();
    public List<AudioClip> injectSoundEffects = new List<AudioClip> ();
    public List<AudioClip> dropItemSoundEffects = new List<AudioClip> ();
    public List<AudioClip> footStepSoundEffects = new List<AudioClip> ();
    public List<TeleportationBeam> teleportationBeams = new List<TeleportationBeam> ();

    [SerializeField] private Inventory.ItemStack DEBUG_ADD_TO_PLAYER_INVENTORY_STACK = new Inventory.ItemStack ();

    [SerializeField] private InventoryCanvas inventoryCanvas;

    public void SetPlayerCharacter (Character character)
    {
        if (PlayerCharacter != null)
        {
            Debug.LogError ( "Player character is already assigned" );
        }

        PlayerCharacter = character;
        CameraController = FindObjectOfType<PlayerCameraController> ();
    }
    
    [NaughtyAttributes.Button]
    private void DEBUG_ADD_TO_PLAYER_INVENTORY ()
    {
        if (!Application.isPlaying) return;

        PlayerInventory.AddItem ( DEBUG_ADD_TO_PLAYER_INVENTORY_STACK.ID, DEBUG_ADD_TO_PLAYER_INVENTORY_STACK.Amount );
    }
}
