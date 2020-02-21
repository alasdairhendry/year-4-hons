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

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 125;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        //float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.35f;
        //EntityManager.instance.PlayerCharacter.Health.AddHealth ( percentOfMaxHealthToHeal, HealType.Consumable );
        //EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );

        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.35f;
        float modified = percentOfMaxHealthToHeal + (percentOfMaxHealthToHeal * TalentManager.instance.GetTalentModifier ( TalentType.BigGulp ));

        float added = EntityManager.instance.PlayerCharacter.Health.AddHealth ( modified, HealType.Consumable );
        MessageBox.AddMessage ( "You drink the Calorie Capsule. It heals " + added + " hitpoints." );

        if (UnityEngine.Random.value < TalentManager.instance.GetTalentModifier ( TalentType.Resourceful ))
        {
            MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.Resourceful ).talentData.talentName + " talent saves the Calorie Shake from being consumed." );
        }
        else
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
        }
    }
}
