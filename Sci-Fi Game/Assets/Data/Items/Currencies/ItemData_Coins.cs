public class ItemData_Coins : ItemBaseData
{
    public ItemData_Coins (int ID) : base ( ID )
    {
        base.Name = "Crowns";
        base.Description = "Yummy money";
        base.category = ItemCategory.Currency;

        base.IsSellable = false;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };
        
        base.BuyPrice = 0;
        base.FetchSprite ();
    }
}
