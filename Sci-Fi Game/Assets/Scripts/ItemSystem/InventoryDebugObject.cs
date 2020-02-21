using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDebugObject : MonoBehaviour
{
    public Inventory inventory = new Inventory ( 12, false, true );

    [SerializeField] InventoryCanvas targetCanvas;

    private void Start ()
    {
        inventory = EntityManager.instance.PlayerInventory;
        //targetCanvas.SetTargetInventory ( inventory );
    }
}
