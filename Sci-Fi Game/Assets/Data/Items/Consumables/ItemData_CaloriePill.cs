public class ItemData_CaloriePill : ItemConsumable
{
    public ItemData_CaloriePill (int ID) : base ( ID, ConsumeType.Eat )
    {
        base.Name = "Calorie Pill";
        base.Description = "Instantly heals 5% of max health";
        base.category = ItemCategory.Consumable;

        base.IsSellable = true;
        base.IsStackable = true;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 50;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        if (!GameManager.instance.CanFireEvent ( true ))
        {
            return;
        }

        float percentOfMaxHealthToHeal = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.05f;
        float modified = percentOfMaxHealthToHeal + (percentOfMaxHealthToHeal * TalentManager.instance.GetTalentModifier ( TalentType.BigGulp ));

        float added = EntityManager.instance.PlayerCharacter.Health.AddHealth ( modified, HealType.Consumable );
        MessageBox.AddMessage ( "You eat the Calorie Pill. It heals " + string.Format ( "{0:0.#}", added ) + " hitpoints." );
        SoundEffectManager.Play ( EntityManager.instance.eatSoundEffects.GetRandom (), AudioMixerGroup.SFX );

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
