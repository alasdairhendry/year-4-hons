public class ItemData_VitroBits : ItemConsumable
{
    public ItemData_VitroBits (int ID) : base ( ID, ConsumeType.Eat )
    {
        base.Name = "Vitro Bits";
        base.Description = "Heals 20% of max health over 10 seconds";
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

        float overallHealAmount = EntityManager.instance.PlayerCharacter.Health.MaxHealth * 0.2f;
        float modified = overallHealAmount + (overallHealAmount * TalentManager.instance.GetTalentModifier ( TalentType.BigGulp ));

        EntityManager.instance.PlayerCharacter.Health.HealOverTime ( modified, 1, 10 );
        MessageBox.AddMessage ( string.Format ( "The effects of the {0} wears off", Name ), MessageBox.Type.Warning, 10 );

        MessageBox.AddMessage ( string.Format ( "You eat the {0}. It's begins to heal you over time.", Name ) );
        SoundEffectManager.Play ( EntityManager.instance.eatSoundEffects.GetRandom (), AudioMixerGroup.SFX );

        if (UnityEngine.Random.value < TalentManager.instance.GetTalentModifier ( TalentType.Resourceful ))
        {
            MessageBox.AddMessage ( "Your " + TalentManager.instance.GetTalent ( TalentType.Resourceful ).talentData.talentName + " talent saves the Vitro Bits from being consumed." );
        }
        else
        {
            EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
        }
    }
}
