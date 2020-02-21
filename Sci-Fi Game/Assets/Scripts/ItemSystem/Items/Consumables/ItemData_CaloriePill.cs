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

        base.IsStackable = true;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 50;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.1f;
        float modified = percentOfMaxHealthToHeal + (percentOfMaxHealthToHeal * TalentManager.instance.GetTalentModifier ( TalentType.BigGulp ));

        float added = EntityManager.instance.PlayerCharacter.Health.AddHealth ( modified, HealType.Consumable );
        MessageBox.AddMessage ( "You eat the Calorie Pill. It heals " + added + " hitpoints." );

        if (UnityEngine.Random.value < TalentManager.instance.GetTalentModifier ( TalentType.Resourceful ))
        {
            MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.Resourceful ).talentData.talentName + " talent saves the Calorie Pill from being consumed." );
        }
        else
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
        }
    }
}
