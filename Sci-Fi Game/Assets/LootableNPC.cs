using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableNPC : MonoBehaviour
{
    private NPCData data;
    private Inventory droppedItems = new Inventory ( 12, false, false );

    private void Start ()
    {
        RollDropTables ();

        if (droppedItems.IsEmpty)
        {
            Destroy ( this.gameObject );
            return;
        }

        droppedItems.RegisterItemRemoved ( (x, y) =>
        {
            if (droppedItems.IsEmpty)
            {
                ItemContainerCanvas.instance.HideContainer ();
                GetComponentInParent<SelfDestruct> ().Initialise ( 3, true );
                Destroy ( this.gameObject );
                return;
            }
        } );
    }

    public void Initialise (Transform target, NPCData npcData)
    {
        transform.SetParent ( target );
        transform.localPosition = Vector3.zero;
        this.data = npcData;
    }

    public void Interact ()
    {
        ItemContainerCanvas.instance.DisplayContainer ( droppedItems, data.NpcName );
    }

    private void RollDropTables ()
    {
        if (data.UniqueDropTable != null)
        {
            AddItemToDrop ( data.UniqueDropTable.RollTable () );
        }

        if (data.AccessToCoinsDropTable)
        {
            AddItemToDrop ( DropTableManager.instance.CoinsDropTable.RollTable () );
        }

        if (data.AccessToGlobalDropTable)
        {
            AddItemToDrop ( DropTableManager.instance.GlobalDropTable.RollTable () );
        }

        if (data.AccessToRareDropTable)
        {
            AddItemToDrop ( DropTableManager.instance.RareDropTable.RollTable () );
        }

        if (data.AccessToSuperRareDropTable)
        {
            AddItemToDrop ( DropTableManager.instance.SuperRareDropTable.RollTable () );
        }
    }

    private void AddItemToDrop (Inventory.ItemStack drop)
    {
        if (drop == null) return;
        if (drop.ID == -1) return;
        if (drop.Amount == 0) { return; }

        if (ItemDatabase.ItemExists ( drop.ID ))
        {
            droppedItems.AddItem ( drop.ID, drop.Amount, true );
            //droppedItems.Add ( new Inventory.ItemStack () { ID = drop.ID, Amount = drop.Amount } );
        }
    }
}
