public class ItemData_PotOfFlour : ItemBaseData
{
    public ItemData_PotOfFlour (int ID) : base ( ID )
    {
        base.Name = "Pot Of Flour";
        base.Description = "I wonder where they grow wheat on this planet";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 10;
        base.FetchSprite ();
    }
}
