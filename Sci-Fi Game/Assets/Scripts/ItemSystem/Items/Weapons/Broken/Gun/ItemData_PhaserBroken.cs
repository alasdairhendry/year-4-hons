public class ItemData_PhaserBroken : ItemGearBroken
{
    public ItemData_PhaserBroken (int ID) : base ( ID )
    {
        base.Name = "Phaser (Broken)";
        base.Description = "A single-fire pistol gun that uses plasma";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 275;
        base.FetchSprite ();
    }
}
