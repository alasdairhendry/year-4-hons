public class ItemData_CallTheJudgeFudge : ItemBaseData
{
    public ItemData_CallTheJudgeFudge (int ID) : base ( ID )
    {
        base.Name = "Fudge";
        base.Description = "The judge's fudge!";
        base.category = ItemCategory.Misc;

        base.IsSellable = false;
        
        

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { "CallTheJudge" };

        base.BuyPrice = 0;
        base.FetchSprite ();
    }
}
