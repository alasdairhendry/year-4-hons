public class ItemData_FrenchFry : ItemBaseData
{
    public ItemData_FrenchFry (int ID) : base ( ID )
    {
        base.Name = "French Fry";
        base.Description = "Aggie will love this.";
        base.category = ItemCategory.Misc;

        base.IsSellable = false;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { "onesmallfavour" };

        base.BuyPrice = 0;
        base.FetchSprite ();
    }
}
