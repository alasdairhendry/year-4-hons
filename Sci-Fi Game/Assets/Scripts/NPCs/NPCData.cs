using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NPCData : ScriptableObject
{
    [SerializeField] private string npcName;
    [SerializeField] private float maxHealth;
    [SerializeField] private HostilityLevel defaultHostilityLevel;
    [Space]
    [SerializeField] private AudioClipObject damageTakenAudioClips;
    [SerializeField] private AudioClipObject deathAudioClips;
    [Space]
    [SerializeField] private DropTable uniqueDropTable;
    [SerializeField] private bool accessToCoinsDropTable = true;
    [SerializeField] private bool accessToGlobalDropTable = true;
    [SerializeField] private bool accessToRareDropTable = true;
    [SerializeField] private bool accessToSuperRareDropTable = true;
    [Space]
    [SerializeField] private int combatLevel = 1;
    [SerializeField] private float baseHitChanceModifier = 1;
    [SerializeField] private float baseDamageModifier = 1;

    public float GetBaseHitChance
    {
        get
        {
            float minHitChance = 0.20f;
            float maxHitChance = 0.90f;

            float maxNPCLevel = 100.0f;

            return Mathf.Lerp ( minHitChance, maxHitChance, Mathf.InverseLerp ( 1, maxNPCLevel, combatLevel ) ) * baseHitChanceModifier;
        }
    }

    public float GetBaseDamageOutput
    {
        get
        {
            float baseLineDamage = 100.0f;
            float maxNPCLevel = 100.0f;
            return ((baseLineDamage * Mathf.Pow ( (float)combatLevel, 1.25f )) / maxNPCLevel) * baseDamageModifier;
        }
    }

    public string NpcName { get => npcName; set => npcName = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public HostilityLevel DefaultHostilityLevel { get => defaultHostilityLevel; set => defaultHostilityLevel = value; }
    public DropTable UniqueDropTable { get => uniqueDropTable; set => uniqueDropTable = value; }
    public bool AccessToCoinsDropTable { get => accessToCoinsDropTable; set => accessToCoinsDropTable = value; }
    public bool AccessToGlobalDropTable { get => accessToGlobalDropTable; set => accessToGlobalDropTable = value; }
    public bool AccessToRareDropTable { get => accessToRareDropTable; set => accessToRareDropTable = value; }
    public bool AccessToSuperRareDropTable { get => accessToSuperRareDropTable; set => accessToSuperRareDropTable = value; }
    public AudioClipObject DamageTakenAudioClips { get => damageTakenAudioClips; set => damageTakenAudioClips = value; }
    public AudioClipObject DeathAudioClips { get => deathAudioClips; set => deathAudioClips = value; }
    public int CombatLevel { get => combatLevel; set => combatLevel = value; }
    public float BaseHitChanceModifier { get => baseHitChanceModifier; set => baseHitChanceModifier = value; }
    public float BaseDamageModifier { get => baseDamageModifier; set => baseDamageModifier = value; }
}
