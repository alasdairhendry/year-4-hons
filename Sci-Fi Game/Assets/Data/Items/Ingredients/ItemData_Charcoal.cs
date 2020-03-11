public class ItemData_Charcoal : ItemBaseData
{
    public ItemData_Charcoal (int ID) : base ( ID )
    {
        base.Name = "Charcoal";
        base.Description = "Remnants of burnt wood";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 15;
        base.FetchSprite ();
    }
}
