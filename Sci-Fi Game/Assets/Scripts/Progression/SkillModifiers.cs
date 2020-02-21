using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillModifiers
{
    public const int MAX_SKILL_LEVEL = 99;
    public const float GLOBAL_PLAYER_DAMAGE_MODIFIER = 1.0f;

    public const float minHealth = 100.0f;
    public const float maxHealth = 1000.0f;

    public static float GetMaxHealth ()
    {
        return Mathf.Lerp ( minHealth, maxHealth, (SkillManager.instance.CombatLevel - 1) / (float)MAX_SKILL_LEVEL );
    }

    // MELEE -----------------------------------------------------------------------------------------------
    private static float meleeBaseHitChance = 0.65f;
    private static float meleeMaxHitChance = 1.0f;

    private static float meleeBaseSpecialChance = 0.10f;
    private static float meleeMaxSpecialChance = 0.20f;

    private static float meleeBaseComboChance = 0.025f;
    private static float meleeMaxComboChance = 0.05f;

    private static float meleeBaseDamageModifier = 1.0f;
    private static float meleeMaxDamageModifier = 10.0f;

    public static float MeleeHitChance
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Melee ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, meleeMaxHitChance - meleeBaseHitChance, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return meleeBaseHitChance + lerp;
        }
    }

    public static float MeleeSpecialChance
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Melee ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, meleeMaxSpecialChance - meleeBaseSpecialChance, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return meleeBaseSpecialChance + lerp;
        }
    }

    public static float MeleeComboChance
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Melee ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, meleeMaxComboChance - meleeBaseComboChance, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return meleeBaseComboChance + lerp;
        }
    }

    public static float MeleeDamageModifier
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Melee ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, meleeMaxDamageModifier - meleeBaseDamageModifier, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return meleeBaseDamageModifier + lerp;
        }
    }

    public static float MeleeHitChanceMultiplicationIncrease
    {
        get
        {
            return MeleeHitChance / meleeBaseHitChance;
        }
    }

    public static float MeleeSpecialChanceMultiplicationIncrease
    {
        get
        {
            return MeleeSpecialChance / meleeBaseSpecialChance;
        }
    }

    public static float MeleeComboChanceMultiplicationIncrease
    {
        get
        {
            return MeleeComboChance / meleeBaseComboChance;
        }
    }

    public static float MeleeDamageMultiplicationIncrease
    {
        get
        {
            return MeleeDamageModifier / meleeBaseDamageModifier;
        }
    }



    // SHOOTING -----------------------------------------------------------------------------------------------
    private static float shootingBaseRecoilModifier = 1.0f;
    private static float shootingMaxRecoilModifier = 0.25f;

    private static float shootingBaseDamageModifier = 1.0f;
    private static float shootingMaxDamageModifier = 10.0f;

    public static float ShootingRecoilModifier
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Shooting ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, shootingMaxRecoilModifier - shootingBaseRecoilModifier, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return shootingBaseRecoilModifier + lerp;
        }
    }

    public static float ShootingDamageModifier
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Shooting ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, shootingMaxDamageModifier - shootingBaseDamageModifier, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return shootingBaseDamageModifier + lerp;
        }
    }

    public static float ShootingRecoilModifierMultiplicationIncrease
    {
        get
        {
            return shootingBaseRecoilModifier / ShootingRecoilModifier;
        }
    }

    public static float ShootingDamageModifierMultiplicationIncrease
    {
        get
        {
            return ShootingDamageModifier / shootingBaseDamageModifier;
        }
    }



    // GLOBAL COMBAT -----------------------------------------------------------------------------------------------
    private static float baseCriticalChance = 0.025f;
    private static float maxCriticalChance = 0.05f;

    public static float CriticalHitChance
    {
        get
        {
            float currentLevel = Mathf.Max ( SkillManager.instance.GetSkill ( SkillType.Melee ).currentLevel, SkillManager.instance.GetSkill ( SkillType.Shooting ).currentLevel );
            float lerp = Mathf.Lerp ( 0.0f, maxCriticalChance - baseCriticalChance, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return baseCriticalChance + lerp;
        }
    }

    public static float CriticalHitMultiplicationIncrease
    {
        get
        {
            return CriticalHitChance / baseCriticalChance;
        }
    }



    // CRAFTING -----------------------------------------------------------------------------------------------
    private static float craftingSpeedBaseModifier = 0.01f;
    private static float craftingSuccessBaseChance = 0.75f;
    private static float craftingSuccessMaxChance = 0.95f;

    public static float CraftingRecipeSpeedModifier
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Crafting ).currentLevel;
            return 1 + ((currentLevel - 1) * craftingSpeedBaseModifier);
        }
    }

    public static float CraftingSuccessChance
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Crafting ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, craftingSuccessMaxChance - craftingSuccessBaseChance, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return craftingSuccessBaseChance + lerp;
        }
    }

    public static float CraftingSpeedMultiplicationIncrease
    {
        get
        {
            return CraftingRecipeSpeedModifier;
        }
    }

    public static float CraftingSuccessChanceMultiplicationIncrease
    {
        get
        {
            return (CraftingSuccessChance / craftingSuccessBaseChance);
        }
    }



    // GATHERING -----------------------------------------------------------------------------------------------
    private static float gatheringBaseYieldChance = 0.25f;
    private static float gatheringMaxYieldChance = 0.75f;

    private static float gatheringBaseCompostChance = 0.40f;
    private static float gatheringMaxCompostChance = 0.75f;

    private static float gatheringBaseGrowthSpeedModifier = 1.0f;
    private static float gatheringMaxGrowthSpeedModifier = 2.0f;

    public static float GatheringYieldChance
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Gathering ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, gatheringMaxYieldChance - gatheringBaseYieldChance, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return gatheringBaseYieldChance + lerp;
        }
    }

    public static float GatheringCompostChance
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Gathering ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, gatheringMaxCompostChance - gatheringBaseCompostChance, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return gatheringBaseCompostChance + lerp;
        }
    }

    public static float GatheringGrowthSpeedModifier
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Gathering ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, gatheringMaxGrowthSpeedModifier - gatheringBaseGrowthSpeedModifier, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return gatheringBaseGrowthSpeedModifier + lerp;
        }
    }

    public static float GatheringYieldChanceMultiplicationIncrease
    {
        get
        {
            return (GatheringYieldChance / gatheringBaseYieldChance);
        }
    }

    public static float GatheringCompostChanceMultiplicationIncrease
    {
        get
        {
            return (GatheringCompostChance / gatheringBaseCompostChance);
        }
    }

    public static float GatheringGrowthSpeedMultiplicationIncrease
    {
        get
        {
            return (GatheringGrowthSpeedModifier / gatheringBaseGrowthSpeedModifier);
        }
    }


    // DRIVING -----------------------------------------------------------------------------------------------
    private static float drivingBaseDamageReductionModifier = 1.0f;
    private static float drivingMaxDamageReductionModifier = 0.10f;

    private static float drivingBaseTurnSpeedModifier = 1.0f;
    private static float drivingMaxTurnSpeedModifier = 2.5f;

    private static float drivingBaseAccelerationModifier = 1.0f;
    private static float drivingMaxAccelerationModifier = 2.5f;

    public static float DrivingDamageReduction
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Gathering ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, drivingMaxDamageReductionModifier - drivingBaseDamageReductionModifier, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return drivingBaseDamageReductionModifier + lerp;
        }
    }

    public static float DrivingTurnSpeedModifier
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Gathering ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, drivingMaxTurnSpeedModifier - drivingBaseTurnSpeedModifier, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return drivingBaseTurnSpeedModifier + lerp;
        }
    }

    public static float DrivingAccelerationModifier
    {
        get
        {
            float currentLevel = SkillManager.instance.GetSkill ( SkillType.Gathering ).currentLevel;
            float lerp = Mathf.Lerp ( 0.0f, drivingMaxAccelerationModifier - drivingBaseAccelerationModifier, (currentLevel - 1) / MAX_SKILL_LEVEL );
            return drivingBaseAccelerationModifier + lerp;
        }
    }

    public static float DrivingDamageReductionMultiplicationIncrease
    {
        get
        {
            return (drivingBaseDamageReductionModifier / DrivingDamageReduction);
        }
    }

    public static float DrivingTurnSpeedMultiplicationIncrease
    {
        get
        {
            return (DrivingTurnSpeedModifier / drivingBaseTurnSpeedModifier);
        }
    }

    public static float DrivingAccelerationMultiplicationIncrease
    {
        get
        {
            return (DrivingAccelerationModifier / drivingBaseAccelerationModifier);
        }
    }
}
