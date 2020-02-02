public class ItemData_SpongeCake : ItemConsumable
{
    public ItemData_SpongeCake (int ID) : base ( ID, ConsumeType.Eat )
    {
        base.Name = "Sponge Cake";
        base.Description = "Instantly heals 2.5% of max health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 120;
        base.BuyPrice = 500;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.025f;
        EntityManager.instance.PlayerCharacter.Health.AddHealth ( percentOfMaxHealthToHeal, HealType.Consumable );
        EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
    }
}
