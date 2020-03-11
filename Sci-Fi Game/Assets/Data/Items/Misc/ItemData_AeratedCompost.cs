public class ItemData_AeratedCompost : ItemBaseData
{
    public ItemData_AeratedCompost (int ID) : base (ID)
    {
        base.Name = "Aerated Compost";
        base.Description = "Used to cultivated flowers and plants";
        base.category = ItemCategory.Misc;

        base.IsSellable = true;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 5;
        base.FetchSprite ();
    }
}
