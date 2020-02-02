public class ItemData_CalorieCapsule : ItemConsumable
{
    public ItemData_CalorieCapsule (int ID) : base ( ID, ConsumeType.Eat )
    {
        base.Name = "Calorie Capsule";
        base.Description = "Instantly heals 20% of max health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 10;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 15;
        base.BuyPrice = 75;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.2f;
        EntityManager.instance.PlayerCharacter.Health.AddHealth ( percentOfMaxHealthToHeal, HealType.Consumable );
        EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
    }
}
