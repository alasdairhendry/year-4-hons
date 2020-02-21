using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NPCData : ScriptableObject
{
    [SerializeField] private string npcName;
    [SerializeField] private NPCAttackOption defaultAttackOption;
    [SerializeField] private Faction faction;
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
    [SerializeField] private bool combatScalesWithPlayer = false;
    [SerializeField] private int combatLevel = 1;
    [SerializeField] private float baseHitChanceModifier = 1;
    [SerializeField] private float baseMaxHealthModifier = 1;
    [SerializeField] private float baseDamageModifier = 1;   

    public string NpcName { get => npcName; set => npcName = value; }
    public NPCAttackOption DefaultAttackOption { get => defaultAttackOption; set => defaultAttackOption = value; }
    public DropTable UniqueDropTable { get => uniqueDropTable; set => uniqueDropTable = value; }
    public bool AccessToCoinsDropTable { get => accessToCoinsDropTable; set => accessToCoinsDropTable = value; }
    public bool AccessToGlobalDropTable { get => accessToGlobalDropTable; set => accessToGlobalDropTable = value; }
    public bool AccessToRareDropTable { get => accessToRareDropTable; set => accessToRareDropTable = value; }
    public bool AccessToSuperRareDropTable { get => accessToSuperRareDropTable; set => accessToSuperRareDropTable = value; }
    public AudioClipObject DamageTakenAudioClips { get => damageTakenAudioClips; set => damageTakenAudioClips = value; }
    public AudioClipObject DeathAudioClips { get => deathAudioClips; set => deathAudioClips = value; }
    public int CombatLevel
    {
        get
        {
            if (combatScalesWithPlayer)
            {
                if(SkillManager.instance == null)
                {
                    return combatLevel;
                }

                return (int)SkillManager.instance.CombatLevel;
            }
            else
            {
                return combatLevel;
            }
        }
        protected set
        {
            combatLevel = value;
        }
    }
    public float BaseHitChanceModifier { get => baseHitChanceModifier; set => baseHitChanceModifier = value; }
    public float BaseMaxHealthModifier { get => baseMaxHealthModifier; set => baseMaxHealthModifier = value; }
    public float BaseDamageModifier { get => baseDamageModifier; set => baseDamageModifier = value; }
    public Faction Faction { get => faction; set => faction = value; }
}
