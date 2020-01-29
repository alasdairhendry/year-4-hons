using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public string skillDescription;
    public List<SkillUnlock> unlocks = new List<SkillUnlock> ();

    public float currentXP = 0.0f;
    public int currentLevel = 1;
    public int currentSkillPoints { get; protected set; }

    public List<float> xpRequirements = new List<float> ();

    public Skill (string skillName, string skillDescription, params SkillUnlock[] unlocks)
    {
        this.skillName = skillName;
        this.skillDescription = skillDescription;

        for (int i = 0; i < unlocks.Length; i++)
        {
            this.unlocks.Add ( unlocks[i] );
        }
    }

    public void AddXp (float amount)
    {
        currentXP += amount;

        if (currentXP >= XPRequiredForLevel ( currentLevel ))
        {           
            OnLevelUp ();
        }
    }

    public void UnlockIndex ()
    {
        currentSkillPoints--;
    }

    public void OnLevelUp ()
    {
        float xpReq = XPRequiredForLevel ( currentLevel );
        currentXP -= xpReq;
        currentLevel++;
        currentSkillPoints++;
    }

    public static float XPRequiredForLevel(int targetLevel)
    {
        if (targetLevel == 0) return 0.0f;

        float initial = 100.0f;

        for (int i = 1; i < targetLevel; i++)
        {
            initial *= 1.15f;
        }

        return initial;
    }

    [System.Serializable]
    public class SkillUnlock
    {
        public string unlockName;
        public string unlockDescription;
        public int levelRestriction = -1;
        public int cost;

        public System.Action unlockAction;

        public List<SkillUnlock> requirements = new List<SkillUnlock> ();

        public SkillUnlock (string unlockName, string unlockDescription, int levelRestriction, int cost)
        {
            this.unlockName = unlockName;
            this.unlockDescription = unlockDescription;
            this.levelRestriction = levelRestriction;
            this.cost = cost;
        }

        public void SetUnlockAction(System.Action action)
        {
            unlockAction = action;
        }

        public void SetRequirements (params SkillUnlock[] requirements)
        {
            for (int i = 0; i < requirements.Length; i++)
            {
                this.requirements.Add ( requirements[i] );
            }
        }
    }
}
