public class ItemData_CaloriePill : ItemConsumable
{
    public ItemData_CaloriePill (int ID) : base ( ID, ConsumeType.Eat )
    {
        base.Name = "Calorie Pill";
        base.Description = "Instantly heals 10% of max health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 10;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 10;
        base.BuyPrice = 50;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.1f;
        EntityManager.instance.PlayerCharacter.Health.AddHealth ( percentOfMaxHealthToHeal, HealType.Consumable );
        EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
    }
}
