public class ItemData_UncookedCake : ItemBaseData
{
    public ItemData_UncookedCake (int ID) : base ( ID )
    {
        base.Name = "Uncooked Cake";
        base.Description = "Yuck. Maybe an oven would help.";
        base.category = ItemCategory.Misc;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 60;
        base.BuyPrice = 375;
        base.FetchSprite ();
    }
}
