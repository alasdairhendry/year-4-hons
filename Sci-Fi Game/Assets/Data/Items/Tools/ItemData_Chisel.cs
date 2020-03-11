public class ItemData_Chisel : ItemBaseData
{
    public ItemData_Chisel (int ID) : base ( ID )
    {
        base.Name = "Chisel";
        base.Description = "A tool used to break things";
        base.category = ItemCategory.Tool;

        base.IsSellable = true;
        
        
        base.category = ItemCategory.Tool;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 25;
        base.FetchSprite ();
    }
}
