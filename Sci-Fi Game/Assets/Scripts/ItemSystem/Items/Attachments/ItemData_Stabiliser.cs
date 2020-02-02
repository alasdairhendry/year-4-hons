public class ItemData_Stabiliser : ItemGearAttachment
{
    public ItemData_Stabiliser (int ID) : base ( ID )
    {
        base.Name = "Stabiliser";
        base.Description = "Reduces bullet spread, but also decreases base damage";

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
