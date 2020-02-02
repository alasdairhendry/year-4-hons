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
    public Inventory PlayerInventory { get; protected set; } = new Inventory ( 12, true, true );
    public Camera MainCamera { get; protected set; }
    [SerializeField] private InventoryCanvas inventoryCanvas;

    public void SetPlayerCharacter (Character character)
    {
        if (PlayerCharacter != null)
        {
            Debug.LogError ( "Player character is already assigned" );
        }

        PlayerCharacter = character;
    }
}
