public class ItemData_CalorieShake : ItemConsumable
{
    public ItemData_CalorieShake (int ID) : base ( ID, ConsumeType.Drink )
    {
        base.Name = "Calorie Shake";
        base.Description = "Instantly heals 35% of max health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = 10;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 25;
        base.BuyPrice = 125;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.35f;
        EntityManager.instance.PlayerCharacter.Health.AddHealth ( percentOfMaxHealthToHeal, HealType.Consumable );
        EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
    }
}
