public class ItemData_RottenPoppy : ItemBaseData
{
    public ItemData_RottenPoppy (int ID) : base ( ID )
    {
        base.Name = "Rotten Poppy";
        base.Description = "A disgusting flower";
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
