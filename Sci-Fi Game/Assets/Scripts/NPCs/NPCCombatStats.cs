using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NPCCombatStats
{
    public const int MAX_NPC_LEVEL = 99;

    private static float minHealth = 100.0f;
    private static float maxHealth = 1500.0f;

    private static float minMeleeDamageOutput = 15.0f;
    private static float maxMeleeDamageOutput = 150.0f;

    private static float minMeleeHitChance = 0.40f;
    private static float maxMeleeHitChance = 0.95f;

    private static float minGunDamageOutput = 5.0f;
    private static float maxGunDamageOutput = 25.0f;

    private static float minGunHitChance = 0.20f;
    private static float maxGunHitChance = 0.80f;

    public static float GetMaxHealth (NPCData data)
    {
        return Mathf.Lerp ( minHealth, maxHealth, GetNormalisedCombatLevel ( data.CombatLevel - 1 ) ) * data.BaseMaxHealthModifier;
    }

    public static float GetMeleeDamageOutput (NPCData data)
    {
        return Mathf.Lerp ( minMeleeDamageOutput, maxMeleeDamageOutput, GetNormalisedCombatLevel ( data.CombatLevel - 1 ) ) * data.BaseDamageModifier;
    }

    public static float GetMeleeHitChance (NPCData data)
    {
        return Mathf.Lerp ( minMeleeHitChance, maxMeleeHitChance, GetNormalisedCombatLevel ( data.CombatLevel - 1 ) ) * data.BaseHitChanceModifier;
    }

    public static float GetGunDamageOutput (NPCData data)
    {
        return Mathf.Lerp ( minGunDamageOutput, maxGunDamageOutput, GetNormalisedCombatLevel ( data.CombatLevel - 1 ) ) * data.BaseDamageModifier;
    }

    public static float GetGunHitChance (NPCData data)
    {
        return Mathf.Lerp ( minGunHitChance, maxGunHitChance, GetNormalisedCombatLevel ( data.CombatLevel - 1 ) ) * data.BaseHitChanceModifier;
    }

    private static float GetNormalisedCombatLevel(int combatLevel)
    {
        return (float)combatLevel / 99.0f;
    }
}
