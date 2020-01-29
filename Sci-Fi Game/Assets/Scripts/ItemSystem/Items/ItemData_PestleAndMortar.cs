public class ItemData_PestleAndMortar : ItemBaseData
{
    public ItemData_PestleAndMortar (int ID) : base ( ID )
    {
        base.Name = "Pestle & Mortar";
        base.Description = "Useful for grinding ingredients!";
        base.category = ItemCategory.Tool;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 1000;
        base.BuyPrice = 7500;
        base.FetchSprite ();
    }
}
