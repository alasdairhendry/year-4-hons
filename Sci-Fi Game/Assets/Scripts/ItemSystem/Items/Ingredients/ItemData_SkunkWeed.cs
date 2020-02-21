public class ItemData_SkunkWeed : ItemBaseData
{
    public ItemData_SkunkWeed (int ID) : base ( ID )
    {
        base.Name = "Skunk Weed";
        base.Description = "So smelly it might make you faint";
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
