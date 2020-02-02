public class ItemData_ChocolateSpongeCake : ItemConsumable
{
    public ItemData_ChocolateSpongeCake (int ID) : base ( ID, ConsumeType.Eat )
    {
        base.Name = "Chocolate Sponge Cake";
        base.Description = "Instantly heals 5% of max health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.MaxStack = int.MaxValue;
        base.RelatedQuestIDs = new string[] { };

        base.SellPrice = 200;
        base.BuyPrice = 750;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.05f;
        EntityManager.instance.PlayerCharacter.Health.AddHealth ( percentOfMaxHealthToHeal, HealType.Consumable );
        EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
    }
}
