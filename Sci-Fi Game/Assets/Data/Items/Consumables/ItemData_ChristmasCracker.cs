using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemData_ChristmasCracker : ItemConsumable
{
    public ItemData_ChristmasCracker (int ID) : base ( ID, ConsumeType.Use )
    {
        base.Name = "Christmas Cracker";
        base.Description = "This may contain a prize inside!";
        base.category = ItemCategory.Consumable;

        base.IsSellable = false;
        base.IsStackable = true;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 50;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        if (!GameManager.instance.CanFireEvent ( true ))
        {
            return;
        }

        if (EntityManager.instance.PlayerInventory.CheckCanRecieveItem ( 91, 1 ))
        {
            List<Inventory.ItemStack> drops = new List<Inventory.ItemStack> ();
            bool wasFactionRoll = false;

            if (DropTableManager.instance.PartyHatDropTable.RollTable ( out drops, out wasFactionRoll ))
            {
                EntityManager.instance.PlayerInventory.AddMultipleItems ( drops );
                if (wasFactionRoll) MessageBox.AddMessage ( "Your Faction Specialisation helps you find an item", MessageBox.Type.Warning );
                MessageBox.AddMessage ( "You pulled the cracker and find a party hat inside!" );
            }
            else
            {
                MessageBox.AddMessage ( "You pulled the cracker, but it's empty inside.." );

            }

            EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
        }
        else
        {
            MessageBox.AddMessage ( "I need more inventory space to do this" );
        }
    }
}