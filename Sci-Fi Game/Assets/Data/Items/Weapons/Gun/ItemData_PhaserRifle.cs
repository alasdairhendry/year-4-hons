public class ItemData_PhaserRifle : ItemGearWeaponGun
{
    public ItemData_PhaserRifle (int ID) : base ( ID, "Phaser Rifle" )
    {
        base.Name = "Phaser Rifle";
        base.Description = "A burst-fire automatic gun that uses plasma";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 1000;
        base.FetchSprite ();
    }
}
