public class ItemData_BronyAntiqueHead : ItemGearEquipable
{
    public ItemData_BronyAntiqueHead (int ID) : base ( ID, "AntiqueBronyHead" )
    {
        base.Name = "Antique Brony Head";
        base.Description = "Hmm..";

        base.IsSellable = true;
        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 0;
        this.gearSlot = GearSlot.Head;
        base.FetchSprite ();
    }
}
