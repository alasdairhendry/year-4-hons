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

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 15;
        base.FetchSprite ();
    }
}
