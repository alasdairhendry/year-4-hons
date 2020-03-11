public class ItemData_StepTrigger : ItemGearAttachment
{
    public ItemData_StepTrigger (int ID) : base ( ID )
    {
        base.Name = "Step Trigger";
        base.Description = "Increases fire rate, but also increase bullet spread";

        base.IsSellable = true;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 250;
        base.FetchSprite ();
    }
}
