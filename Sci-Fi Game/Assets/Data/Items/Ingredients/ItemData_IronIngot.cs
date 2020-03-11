public class ItemData_IronIngot : ItemBaseData
{
    public ItemData_IronIngot (int ID) : base ( ID )
    {
        base.Name = "Iron Ingot";
        base.Description = "A refined, poor quality metal bar";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 160;
        base.FetchSprite ();
    }
}
