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

        base.IsStackable = true;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 75;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        //float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.2f;
        //EntityManager.instance.PlayerCharacter.Health.AddHealth ( percentOfMaxHealthToHeal, HealType.Consumable );
        //EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );

        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.2f;
        float modified = percentOfMaxHealthToHeal + (percentOfMaxHealthToHeal * TalentManager.instance.GetTalentModifier ( TalentType.BigGulp ));

        float added = EntityManager.instance.PlayerCharacter.Health.AddHealth ( modified, HealType.Consumable );
        MessageBox.AddMessage ( "You eat the Calorie Capsule. It heals " + added + " hitpoints." );

        if (UnityEngine.Random.value < TalentManager.instance.GetTalentModifier ( TalentType.Resourceful ))
        {
            MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.Resourceful ).talentData.talentName + " talent saves the Calorie Capsule from being consumed." );
        }
        else
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
        }
    }
}
