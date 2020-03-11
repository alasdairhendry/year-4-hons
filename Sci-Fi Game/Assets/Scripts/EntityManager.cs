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
        bankCanvas.SetTargetInventory ( PlayerBankInventory );
        MainCamera = Camera.main;
    }

    public Character PlayerCharacter { get; protected set; }
    public Inventory PlayerInventory = new Inventory ( 16, false, true );
    public Inventory PlayerBankInventory = new Inventory ( int.MaxValue, true, true );
    public PlayerCameraController CameraController { get; protected set; }
    public Camera MainCamera { get; protected set; }
    public InventoryCanvas InventoryCanvas { get => inventoryCanvas; protected set => inventoryCanvas = value; }
    public BankCanvas BankCanvas { get => bankCanvas; protected set => bankCanvas = value; }
    public WeaponMeleeData fistsWeaponData;
    [Space]
    public List<AudioClip> eatSoundEffects = new List<AudioClip> ();
    public List<AudioClip> drinkSoundEffects = new List<AudioClip> ();
    public List<AudioClip> injectSoundEffects = new List<AudioClip> ();
    public List<AudioClip> dropItemSoundEffects = new List<AudioClip> ();
    public List<AudioClip> footStepSoundEffects = new List<AudioClip> ();
    public List<TeleportationBeam> teleportationBeams = new List<TeleportationBeam> ();
    public Vector3 PlayerRespawnWorldPosition { get => playerRespawnTransform.position; }
    public Quaternion PlayerRespawnRotation { get => playerRespawnTransform.rotation; }
    public WeaponMeleeData FistsWeaponData { get => fistsWeaponData; }

    [SerializeField] private Transform playerRespawnTransform;

    [SerializeField] private InventoryCanvas inventoryCanvas;
    [SerializeField] private BankCanvas bankCanvas;

    public void SetPlayerCharacter (Character character)
    {
        if (PlayerCharacter != null)
        {
            Debug.LogError ( "Player character is already assigned" );
        }

        PlayerCharacter = character;

        PlayerCharacter.transform.position = PlayerRespawnWorldPosition;
        PlayerCharacter.transform.rotation = PlayerRespawnRotation;

        CameraController = FindObjectOfType<PlayerCameraController> ();
        CameraController.SetTarget ( PlayerCharacter.transform );
        CameraController.SnapToTargetPosition ();
    }
}
