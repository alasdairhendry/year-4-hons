using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NPCData : ScriptableObject
{
    [SerializeField] private string npcName;
    [SerializeField] private NPCAttackOption defaultAttackOption;
    [SerializeField] private List<Faction> possibleFactions = new List<Faction> ();
    [Space]
    [SerializeField] private AudioClipObject damageTakenAudioClips;
    [SerializeField] private AudioClipObject deathAudioClips;
    [Space]
    [SerializeField] private DropTable uniqueDropTable;
    [SerializeField] private bool accessToCoinsDropTable = true;
    [SerializeField] private bool accessToIngredientsDropTable = true;
    [SerializeField] private bool accessToMeleeDropTable = true;
    [SerializeField] private bool accessToGunDropTable = true;
    [SerializeField] private bool accessToMaskTable = true;
    [SerializeField] private bool accessToPartyHatTable = true;
    [Space]
    [SerializeField] private bool combatScalesWithPlayer = false;
    [SerializeField] private int combatLevel = 1;
    [SerializeField] private float baseHitChanceModifier = 1;
    [SerializeField] private float baseMaxHealthModifier = 1;
    [SerializeField] private float baseDamageModifier = 1;
    [Space]
    [SerializeField] private List<Material> characterMaterials = new List<Material> ();

    public string NpcName { get => npcName; set => npcName = value; }
    public NPCAttackOption DefaultAttackOption { get => defaultAttackOption; set => defaultAttackOption = value; }

    public DropTable UniqueDropTable { get => uniqueDropTable; set => uniqueDropTable = value; }
    public bool AccessToCoinsDropTable { get => accessToCoinsDropTable; set => accessToCoinsDropTable = value; }
    public bool AccessToIngredientsDropTable { get => accessToIngredientsDropTable; set => accessToIngredientsDropTable = value; }
    public bool AccessToMeleeDropTable { get => accessToMeleeDropTable; set => accessToMeleeDropTable = value; }
    public bool AccessToGunDropTable { get => accessToGunDropTable; set => accessToGunDropTable = value; }
    public bool AccessToMaskTable { get => accessToMaskTable; set => accessToMaskTable = value; }
    public bool AccessToPartyHatTable { get => accessToPartyHatTable; set => accessToPartyHatTable = value; }

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
    public List<Faction> PossibleFactions { get => possibleFactions; set => possibleFactions = value; }
    public List<Material> CharacterMaterials { get => characterMaterials; set => characterMaterials = value; }
}
