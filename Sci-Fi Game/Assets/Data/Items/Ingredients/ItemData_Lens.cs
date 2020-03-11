public class ItemData_Lens : ItemBaseData
{
    public ItemData_Lens (int ID) : base ( ID )
    {
        base.Name = "Lens";
        base.Description = "A piece of glass that magnifies items held under it";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
