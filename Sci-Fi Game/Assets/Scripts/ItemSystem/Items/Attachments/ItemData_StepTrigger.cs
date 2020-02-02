public class ItemData_StepTrigger : ItemGearAttachment
{
    public ItemData_StepTrigger (int ID) : base ( ID )
    {
        base.Name = "Step Trigger";
        base.Description = "Increases fire rate, but also increase bullet spread";

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 1;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 200;
        base.BuyPrice = 60;
        base.FetchSprite ();
    }
}
