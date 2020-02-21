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

        base.IsStackable = true; 
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 40;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        //float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.025f;
        //EntityManager.instance.PlayerCharacter.Health.AddHealth ( percentOfMaxHealthToHeal, HealType.Consumable );
        //EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );

        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.025f;
        float modified = percentOfMaxHealthToHeal + (percentOfMaxHealthToHeal * TalentManager.instance.GetTalentModifier ( TalentType.BigGulp ));

        float added = EntityManager.instance.PlayerCharacter.Health.AddHealth ( modified, HealType.Consumable );
        MessageBox.AddMessage ( "You eat the cake. It heals " + added + " hitpoints." );

        if (UnityEngine.Random.value < TalentManager.instance.GetTalentModifier ( TalentType.Resourceful ))
        {
            MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.Resourceful ).talentData.talentName + " talent saves the cake from being consumed." );
        }
        else
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
        }
    }
}
