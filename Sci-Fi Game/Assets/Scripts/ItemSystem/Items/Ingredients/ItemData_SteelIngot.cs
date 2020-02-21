public class ItemData_SteelIngot : ItemBaseData
{
    public ItemData_SteelIngot (int ID) : base ( ID )
    {
        base.Name = "Steel Ingot";
        base.Description = "A refined, high quality metal bar";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 200;
        base.FetchSprite ();
    }
}
