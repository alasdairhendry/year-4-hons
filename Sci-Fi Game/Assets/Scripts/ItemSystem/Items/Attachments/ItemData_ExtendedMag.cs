public class ItemData_ExtendedMag : ItemGearAttachment
{
    public ItemData_ExtendedMag (int ID) : base ( ID )
    {
        base.Name = "Extended Mag";
        base.Description = "Increases ammo capacity, but also increases reload speed";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 200;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
