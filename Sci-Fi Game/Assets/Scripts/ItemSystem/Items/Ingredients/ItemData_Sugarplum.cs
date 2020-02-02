public class ItemData_Sugarplum : ItemBaseData
{
    public ItemData_Sugarplum (int ID) : base ( ID )
    {
        base.Name = "Sugarplum";
        base.Description = "Extremely sweet, luxurious flower";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 30;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
