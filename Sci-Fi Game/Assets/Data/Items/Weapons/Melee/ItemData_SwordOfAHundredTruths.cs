public class ItemData_SwordOfAHundredTruths : ItemGearWeaponMelee
{
    public ItemData_SwordOfAHundredTruths (int ID) : base ( ID, "SwordOfAHundredTruths" )
    {
        base.Name = "Sword of 100 Truths";
        base.Description = "A mystical weapon that can completely drain someone's health";

        base.IsSellable = false;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { "theswordof100truths" };

        base.BuyPrice = 10000;
        base.FetchSprite ();
    }

    public override float GetWeaponDamage (float currentCalculatedDamage, NPC targetNPC)
    {
        return targetNPC.Health.MaxHealth * 2.0f;
    }
}
