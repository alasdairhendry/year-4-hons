public class ItemData_1GBFlashDrive : ItemBaseData
{
    public ItemData_1GBFlashDrive (int ID) : base (ID)
    {
        base.Name = "1GB Flash Drive";
        base.Description = "The flash drive used to store the almighty sword.";
        base.category = ItemCategory.Misc;

        base.IsSellable = false;
        
        

        base.IsStackable = true;
        base.RelatedQuestIDs = new string[] { "theswordof100truths" };

        base.BuyPrice = 5000;
        base.FetchSprite ();
    }
}
