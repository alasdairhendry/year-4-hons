public class ItemData_Egg : ItemBaseData
{
    public ItemData_Egg (int ID) : base ( ID )
    {
        base.Name = "Egg";
        base.Description = "Can mechanical chickens lay eggs?";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 5;
        base.BuyPrice = 25;
        base.FetchSprite ();
    }
}