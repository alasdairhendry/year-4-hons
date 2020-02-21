public class ItemData_Egg : ItemBaseData
{
    public ItemData_Egg (int ID) : base ( ID )
    {
        base.Name = "Powdered Egg";
        base.Description = "Can mechanical chickens lay eggs?";
        base.category = ItemCategory.Ingredient;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 5;
        base.FetchSprite ();
    }
}