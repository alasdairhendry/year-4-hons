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
    }

    public Character PlayerCharacter { get; protected set; }
    public Inventory PlayerInventory { get; protected set; } = new Inventory ( 12, true, true );
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
