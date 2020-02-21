using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SkillType { Shooting, Melee, Driving, Speech, Gathering, Crafting }

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public GameObject uiPrefab;
    public CanvasGroup canvasGroup;
    public GameObject canvasBody;

    [SerializeField] private List<Skill> skills = new List<Skill> ();
    [SerializeField] private List<float> xpRatesByLevel = new List<float> ();
    [SerializeField] private float xpRefreshRate = 1.0f;
    private Dictionary<SkillType, Skill> skillDictionary = new Dictionary<SkillType, Skill> ();

    private List<XPToAdd> xpToAddQueue = new List<XPToAdd> ();
    private float xpRefreshCounter = 0.0f;

    public float CharacterLevel { get; protected set; } = 1.0f;
    public float CombatLevel { get; protected set; } = 1.0f;
    public List<Skill> Skills { get => skills; protected set => skills = value; }
    public bool DEBUG_LEVEL_TO_991 { get => DEBUG_LEVEL_TO_99; set => DEBUG_LEVEL_TO_99 = value; }
    public List<float> XpRatesByLevel { get => xpRatesByLevel; protected set => xpRatesByLevel = value; }

    [SerializeField] private bool DEBUG_LEVEL_TO_99;

    /// <summary>
    /// Called when the character level changes, even by 0.1f
    /// </summary>
    public System.Action OnCharacterLevelIncreased;

    /// <summary>
    /// Called when the character level changes by a solid decimal
    /// </summary>
    public System.Action OnCharacterLevelIncremented;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        DetermineXPRatesByLevel ();
        GenerateSkills ();
    }

    private void Update ()
    {
        MonitorXPQueue ();
    }

    private void DetermineXPRatesByLevel ()
    {
        float previousLevel = 0;
        for (int i = 0; i < 101; i++)
        {
            if (i == 0)
            {
                xpRatesByLevel.Add ( 0 );
                continue;
            }

            xpRatesByLevel.Add ( DetermineXPRequirementForLevel ( i, previousLevel ) );
            previousLevel = xpRatesByLevel[i];
        }
    }

    private float DetermineXPRequirementForLevel (int targetLevel, float previousLevelXP)
    {
        return Mathf.Floor ( ((targetLevel * targetLevel) * 0.7f) + 100.0f ) + previousLevelXP;
    }

    private void GenerateSkills ()
    {
        GenerateSkill ( SkillType.Shooting, "Determines your accuracy, speed and damage output whilst shooting" );
        GenerateSkill ( SkillType.Melee, "Determines your accuracy, speed and damage output whilst using melee weapons" );
        GenerateSkill ( SkillType.Driving, "Determines handling, acceleration and max speed whilst driving a vehicle" );
        GenerateSkill ( SkillType.Speech, "Provides betters prices whilst trading and increases persuasion success rates" );
        GenerateSkill ( SkillType.Gathering, "Increases yield from crops and increases drop rates whilst looting" );
        GenerateSkill ( SkillType.Crafting, "Increases chance of success whilst crafting and allows better items to be made" );

        CheckCharacterLevel ();
        CheckCombatLevel ();
    }

    private void GenerateSkill(SkillType skillType, string description)
    {
        Skill skill = new Skill ( skillType.ToString ().ToProperCase (), description, skillType );
        skillDictionary.Add ( skillType, skill );
        skills.Add ( skill );
        skill.onLevelUp += OnSkillLeveledUp;   
    }

    private void OnSkillLeveledUp (int newLevel, SkillType skillType)
    {
        CheckCharacterLevel ();
        CheckCombatLevel ();

        if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Sylas)
            EntityManager.instance.PlayerCharacter.Health.SetMaxHealth ( SkillModifiers.GetMaxHealth () * 1.35f, false );
        else
            EntityManager.instance.PlayerCharacter.Health.SetMaxHealth ( SkillModifiers.GetMaxHealth (), false );

        MessageBox.AddMessage ( "Congratulations. You have just reached level " + newLevel.ToString ( "0" ) + " in " + GetSkill ( skillType ).skillName ); ;
    }

    private void CheckCharacterLevel ()
    {
        float resultingCharacterLevel = 0;
        float currentCharacterLevel = CharacterLevel;

        for (int i = 0; i < skills.Count; i++)
        {
            resultingCharacterLevel += skills[i].currentLevel;
        }

        resultingCharacterLevel /= skills.Count;
        CharacterLevel = resultingCharacterLevel;

        if(resultingCharacterLevel > currentCharacterLevel)
        {
            OnCharacterLevelIncreased?.Invoke ();
        }

        if (Mathf.FloorToInt ( resultingCharacterLevel ) > Mathf.FloorToInt ( currentCharacterLevel ))
        {
            OnCharacterLevelIncremented?.Invoke ();
            MessageBox.AddMessage ( "Congratulations. You have just reached Character Level " + Mathf.FloorToInt ( CharacterLevel ).ToString ( "00" ) );
        }
    }

    private void CheckCombatLevel ()
    {
        CombatLevel = ((float)GetSkill ( SkillType.Melee ).currentLevel + (float)GetSkill ( SkillType.Shooting ).currentLevel) / 2.0f;
    }

    private void MonitorXPQueue ()
    {
        if (xpToAddQueue.Count > 0)
            xpRefreshCounter += Time.deltaTime;

        if(xpRefreshCounter >= xpRefreshRate)
        {
            xpRefreshCounter = 0.0f;
            RefreshXPQueue ();
        }
    }

    private void RefreshXPQueue ()
    {
        Dictionary<SkillType, float> cumulitiveXPToAdd = new Dictionary<SkillType, float> ();

        for (int i = 0; i < xpToAddQueue.Count; i++)
        {
            XPToAdd currentXPToAdd = xpToAddQueue[i];

            if (cumulitiveXPToAdd.ContainsKey ( currentXPToAdd.skillType ))
            {
                cumulitiveXPToAdd[currentXPToAdd.skillType] += currentXPToAdd.amount;
            }
            else
            {
                cumulitiveXPToAdd.Add ( currentXPToAdd.skillType, currentXPToAdd.amount );
            }
        }

        xpToAddQueue.Clear ();

        foreach (KeyValuePair<SkillType, float> item in cumulitiveXPToAdd)
        {
            GetSkill ( item.Key ).AddXp ( item.Value );
        }
    }

    public void AddXpToSkill (SkillType skill, float amount)
    {
        if(EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Graesen)
        {
            amount *= 1.025f;
        }

        xpToAddQueue.Add ( new XPToAdd ( skill, amount ) );
    }

    public Skill GetSkill(SkillType skillType)
    {
        return skillDictionary[skillType];
    }

    public float GetXPRequirementForLevel(int targetLevel)
    {
        return xpRatesByLevel[targetLevel];
    }

    private class XPToAdd
    {
        public SkillType skillType = SkillType.Melee;
        public float amount;

        public XPToAdd (SkillType skillType, float amount)
        {
            this.skillType = skillType;
            this.amount = amount;
        }
    }
}
