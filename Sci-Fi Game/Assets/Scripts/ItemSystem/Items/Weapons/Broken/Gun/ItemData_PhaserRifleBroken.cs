public class ItemData_PhaserRifleBroken : ItemGearBroken
{
    public ItemData_PhaserRifleBroken (int ID) : base ( ID )
    {
        base.Name = "Phaser Rifle (Broken)";
        base.Description = "A burst-fire automatic gun that uses plasma";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 400;
        base.FetchSprite ();
    }
}
