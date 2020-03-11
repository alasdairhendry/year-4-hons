using System.Collections.Generic;
using System.Linq;

public class ItemData_ExperienceGem : ItemConsumable
{
    public ItemData_ExperienceGem (int ID) : base ( ID, ConsumeType.Use )
    {
        base.Name = "Experience Gem";
        base.Description = "When broken, provides the user with experience";
        base.category = ItemCategory.Consumable;

        base.IsSellable = false;
        
        

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { };

        base.BuyPrice = 100;
        base.FetchSprite ();
    }

    protected override void ConsumeItem ()
    {
        if (!GameManager.instance.CanFireEvent ( false ))
        {
            return;
        }

        List<Skill> skills = SkillManager.instance.Skills;

        TeleportCanvas.instance.SetButtons ( "Choose Skill", skills.Select ( x => x.skillName ).ToList (),
            (index) =>
            {
                if (!GameManager.instance.CanFireEvent ( true ))
                {
                    return;
                }
                Skill skill = skills[index];
                float xpToGive = skill.GetNextLevelRelativeXPRequirement () * 0.5f;
                SkillManager.instance.AddXpToSkill ( skill.skillType, xpToGive );
                MessageBox.AddMessage ( "You smash the gem into the ground and it provides " + xpToGive.ToString ( "0.#" ) + " xp in " + skill.skillName );
                EntityManager.instance.PlayerInventory.RemoveItem ( base.ID, 1 );
                SoundEffectManager.Play ( AudioClipAsset.UseGem, AudioMixerGroup.SFX );
            }
        );

        TeleportCanvas.instance.Open ();
    }
}