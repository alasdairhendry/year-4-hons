public class ItemData_CalorieCapsule : ItemConsumable
{
    public ItemData_CalorieCapsule (int ID) : base ( ID, ConsumeType.Eat )
    {
        base.Name = "Calorie Capsule";
        base.Description = "Instantly heals 10% of max health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsStackable = true;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 75;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        if (!GameManager.instance.CanFireEvent ( true ))
        {
            return;
        }

        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.1f;
        float modified = percentOfMaxHealthToHeal + (percentOfMaxHealthToHeal * TalentManager.instance.GetTalentModifier ( TalentType.BigGulp ));

        float added = EntityManager.instance.PlayerCharacter.Health.AddHealth ( modified, HealType.Consumable );
        MessageBox.AddMessage ( "You eat the Calorie Capsule. It heals " + string.Format ( "{0:0.#}", added ) + " hitpoints." );
        SoundEffectManager.Play ( EntityManager.instance.eatSoundEffects.GetRandom (), AudioMixerGroup.SFX );

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
