using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public enum SkillType { Shooting, Combat, Driving, Speech, Gathering, Crafting }

    public GameObject uiPrefab;
    public CanvasGroup canvasGroup;
    public GameObject canvasBody;

    private List<Skill> skills = new List<Skill> ();
    private List<float> xpRatesByLevel = new List<float> ();
    private Dictionary<SkillType, XPTrackerUIPrefab> skillUIs = new Dictionary<SkillType, XPTrackerUIPrefab> ();
    private Dictionary<SkillType, Skill> skillDictionary = new Dictionary<SkillType, Skill> ();

    private List<XPToAdd> xpToAddQueue = new List<XPToAdd> ();
    private float xpRefreshRate = 1.0f;
    private float xpRefreshCounter = 0.0f;

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
        for (int i = 0; i < 100; i++)
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
        GenerateSkill ( SkillType.Combat, "Determines your accuracy, speed and damage output whilst using melee weapons" );
        GenerateSkill ( SkillType.Driving, "Determines handling, acceleration and max speed whilst driving a vehicle" );
        GenerateSkill ( SkillType.Speech, "Provides betters prices whilst trading and increases persuasion success rates" );
        GenerateSkill ( SkillType.Gathering, "Increases yield from crops and increases drop rates whilst looting" );
        GenerateSkill ( SkillType.Crafting, "Increases chance of success whilst crafting and allows better items to be made" );
    }

    private void GenerateSkill(SkillType skillType, string description)
    {
        Skill skill = new Skill ( skillType.ToString ().ToProperCase (), description );
        skillDictionary.Add ( skillType, skill );
        skills.Add ( skill );
        GenerateUI ( skillType );
    }

    private void GenerateUI (SkillType type)
    {
        GameObject go = Instantiate ( uiPrefab );
        go.transform.SetParent ( canvasBody.transform );
        skillUIs.Add ( type, go.GetComponent<XPTrackerUIPrefab> ().Initialise ( type ) );
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

        string[] skillTypes = Enum.GetNames ( typeof ( SkillType ) );

        foreach (KeyValuePair<SkillType, float> item in cumulitiveXPToAdd)
        {
            GetSkill ( item.Key ).AddXp ( item.Value );
            skillUIs[item.Key].OnXpAdded ();
        }
    }

    public void AddXpToSkill (SkillType skill, float amount) {
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
        public SkillType skillType = SkillType.Combat;
        public float amount;

        public XPToAdd (SkillType skillType, float amount)
        {
            this.skillType = skillType;
            this.amount = amount;
        }
    }
}
