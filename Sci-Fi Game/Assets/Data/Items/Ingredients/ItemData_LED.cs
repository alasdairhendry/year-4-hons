public class ItemData_LED : ItemBaseData
{
    public ItemData_LED (int ID) : base ( ID )
    {
        base.Name = "LED";
        base.Description = "An LED light used for crafting weapons and armour";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
