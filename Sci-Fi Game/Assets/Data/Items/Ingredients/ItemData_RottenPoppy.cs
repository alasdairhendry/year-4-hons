public class ItemData_RottenPoppy : ItemBaseData
{
    public ItemData_RottenPoppy (int ID) : base ( ID )
    {
        base.Name = "Rotten Poppy";
        base.Description = "A disgusting flower";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 15;
        base.FetchSprite ();
    }
}
