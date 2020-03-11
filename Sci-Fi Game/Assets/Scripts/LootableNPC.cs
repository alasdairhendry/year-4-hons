using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableNPC : MonoBehaviour
{
    [SerializeField] private Interactable interactable;
    private NPCData data;
    private Inventory droppedItems = new Inventory ( 12, true, false );
    private bool rolledTable = false;

    private void Start ()
    {    
        droppedItems.RegisterItemRemoved ( (x, y) =>
        {
            GetComponentInParent<SelfDestruct> ().Initialise ( 10, true );

            if (droppedItems.IsEmpty)
            {
                ItemContainerCanvas.instance.Close ();
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
        interactable.SetInteractName ( npcData.NpcName );
    }

    public void Interact ()
    {
        RollDropTables ();
        ItemContainerCanvas.instance.SetContainerInventory ( droppedItems, data.NpcName );
        ItemContainerCanvas.instance.Open ();
        GetComponentInParent<SelfDestruct> ().Initialise ( 10, true );
    }

    private void RollDropTables ()
    {
        if (rolledTable) return;
        rolledTable = true;

        bool isFactionRoll = false;

        if (ValidateDropTable ( data.UniqueDropTable != null, data.UniqueDropTable )) isFactionRoll = true;
        if (ValidateDropTable ( data.AccessToCoinsDropTable, DropTableManager.instance.CoinsDropTable )) isFactionRoll = true;
        if (ValidateDropTable ( data.AccessToIngredientsDropTable, DropTableManager.instance.IngredientsDropTable )) isFactionRoll = true;
        if (ValidateDropTable ( data.AccessToMeleeDropTable, DropTableManager.instance.MeleeDropTable )) isFactionRoll = true;
        if (ValidateDropTable ( data.AccessToGunDropTable, DropTableManager.instance.GunDropTable )) isFactionRoll = true;
        if (ValidateDropTable ( data.AccessToMaskTable, DropTableManager.instance.MaskDropTable )) isFactionRoll = true;
        if (ValidateDropTable ( data.AccessToPartyHatTable, DropTableManager.instance.PartyHatDropTable )) isFactionRoll = true;

        if (isFactionRoll) MessageBox.AddMessage ( "Your Faction Specialisation helps you find an item", MessageBox.Type.Warning );

        if (droppedItems.IsEmpty)
        {
            Destroy ( this.gameObject );
            GetComponentInParent<SelfDestruct> ().Initialise ( 6, true );
            return;
        }
    }

    private bool ValidateDropTable (bool hasAccess, DropTable dropTable)
    {
        if (hasAccess)
        {
            List<Inventory.ItemStack> drops = new List<Inventory.ItemStack> ();
            bool wasFactionRoll = false;

            if (dropTable.RollTable ( out drops, out wasFactionRoll ))
            {
                AddItemsToDrop ( drops );
            }

            return wasFactionRoll;
        }

        return false;
    }

    private void AddItemsToDrop (List<Inventory.ItemStack> drops)
    {
        droppedItems.AddMultipleItems ( drops, true );
    }
}
