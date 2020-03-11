using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillEntryUIPanel : MonoBehaviour
{
    public TextMeshProUGUI skillNameText;
    public TextMeshProUGUI skillXPText;
    public TextMeshProUGUI skillLevelText;
    public ProgressBar skillProgressBar;
    public TooltipItemUI tooltip;

    private SkillType skillType = SkillType.Melee;

    public void Initialise ( SkillType skillType)
    {
        this.skillType = skillType;

        tooltip.SetTooltipAction ( () =>
        {
            string s = SkillManager.instance.GetSkill ( skillType ).skillName;
            s += "\n";
            s += ColourHelper.TagColour ( "XP Remaining " + SkillManager.instance.GetSkill ( skillType ).GetXPRemainingUntilLevelUp ().ToString ( "0" ), ColourDescription.OffWhiteText );
            s += "\n";
            s += ColourHelper.TagColour ( "Total XP " + SkillManager.instance.GetSkill ( skillType ).currentXP.ToString ( "0" ), ColourDescription.OffWhiteText );
            s += "\n";
            s += ColourHelper.TagColour ( "-", ColourDescription.OffWhiteText );
            s += "\n";
            s += ColourHelper.TagSize ( GetPerSkillToolTipData ( ColourDescription.DarkYellowText ), 90f );
            return s;
        } );
    }

    private string GetPerSkillToolTipData (ColourDescription colourDescription)
    {
        string s = "";
        switch (skillType)
        {
            case SkillType.Shooting:
                return GetShootingToolTipData ( colourDescription );
            case SkillType.Melee:
                return GetCombatToolTipData ( colourDescription );
            case SkillType.Driving:
                return GetDrivingToolTipData ( colourDescription );
            case SkillType.Speech:
                return GetSpeechToolTipData ( colourDescription );
            case SkillType.Gathering:
                return GetGatheringToolTipData ( colourDescription );
            case SkillType.Crafting:
                return GetCraftingToolTipData ( colourDescription );                
        }

        return s;
    }

    private string GetShootingToolTipData (ColourDescription colourDescription)
    {
        string s = "";
        s += ColourHelper.TagColour ( "Recoil Reduction +" + SkillModifiers.ShootingRecoilModifierMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Damage +" + SkillModifiers.ShootingDamageModifierMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        return s;
    }

    private string GetCombatToolTipData (ColourDescription colourDescription)
    {
        string s = "";
        s += ColourHelper.TagColour ( "Hit Chance +" + SkillModifiers.MeleeHitChanceMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Special Chance +" + SkillModifiers.MeleeSpecialChanceMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Combo Chance +" + SkillModifiers.MeleeComboChanceMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Crit Chance +" + SkillModifiers.CriticalHitMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Damage +" + SkillModifiers.MeleeDamageMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        return s;
    }

    private string GetDrivingToolTipData (ColourDescription colourDescription)
    {
        string s = "";
        s += ColourHelper.TagColour ( "Damage Reduction +" + SkillModifiers.DrivingDamageReductionMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Turning Speed +" + SkillModifiers.DrivingTurnSpeedMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Acceleration +" + SkillModifiers.DrivingAccelerationMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        return s;
    }

    private string GetSpeechToolTipData (ColourDescription colourDescription)
    {
        return "";
    }

    private string GetGatheringToolTipData (ColourDescription colourDescription)
    {
        string s = "";
        s += ColourHelper.TagColour ( "Yield Chance +" + SkillModifiers.GatheringYieldChanceMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Compost Chance +" + SkillModifiers.GatheringCompostChanceMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Growth Speed +" + SkillModifiers.GatheringGrowthSpeedMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        return s;
    }

    private string GetCraftingToolTipData (ColourDescription colourDescription)
    {
        string s = "";
        s += ColourHelper.TagColour ( "Recipe Crafting Speed +" + SkillModifiers.CraftingSpeedMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        s += "\n";
        s += ColourHelper.TagColour ( "Recipe Success Chance +" + SkillModifiers.CraftingSuccessChanceMultiplicationIncrease.ToString ( "0.00" ) + "x", colourDescription );
        return s;
    }
}
