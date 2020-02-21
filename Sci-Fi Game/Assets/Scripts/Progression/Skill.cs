using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public SkillType skillType;
    public string skillDescription;

    public float currentXP = 0.0f;
    public int currentLevel = 1;

    /// <summary>
    /// arg1 = amount of xp gained
    /// </summary>
    public System.Action<float, SkillType> onXPGained;

    /// <summary>
    /// arg1 = new level
    /// </summary>
    public System.Action<int, SkillType> onLevelUp;

    public Skill (string skillName, string skillDescription, SkillType skillType)
    {
        this.skillName = skillName;
        this.skillType = skillType;
        this.skillDescription = skillDescription;

        if (SkillManager.instance.DEBUG_LEVEL_TO_991)
        {
            currentXP = SkillManager.instance.XpRatesByLevel[98];
            currentLevel = 99;
        }
    }

    public void AddXp (float amount)
    {
        if (currentLevel >= SkillModifiers.MAX_SKILL_LEVEL) return;

        currentXP += amount;
        onXPGained?.Invoke ( amount, skillType );

        if (currentXP >= SkillManager.instance.GetXPRequirementForLevel ( currentLevel ))
        {
            OnLevelUp ();
        }
    }

    public float GetProgressToNextLevelNormalised ()
    {
        return Mathf.InverseLerp ( SkillManager.instance.GetXPRequirementForLevel ( currentLevel - 1 ), SkillManager.instance.GetXPRequirementForLevel ( currentLevel ), currentXP );
    }

    public float GetRelativeCurrentXP ()
    {
        return currentXP - SkillManager.instance.GetXPRequirementForLevel ( currentLevel - 1 );
    }

    public float GetXPRemainingUntilLevelUp ()
    {
        return SkillManager.instance.GetXPRequirementForLevel ( currentLevel ) - currentXP;
    }

    public float GetNextLevelRelativeXPRequirement ()
    {
        return SkillManager.instance.GetXPRequirementForLevel ( currentLevel ) - SkillManager.instance.GetXPRequirementForLevel ( currentLevel - 1 );
    }

    public float GetNextLevelTotalXPRequirement ()
    {
        return SkillManager.instance.GetXPRequirementForLevel ( currentLevel );
    }

    private void OnLevelUp ()
    {
        if (currentLevel >= SkillModifiers.MAX_SKILL_LEVEL) return;

        currentLevel++;        
        onLevelUp?.Invoke ( currentLevel, skillType );
    }

    //[System.Serializable]
    //public class SkillUnlock
    //{
    //    public string unlockName;
    //    public string unlockDescription;
    //    public int levelRestriction = -1;
    //    public int cost;

    //    public System.Action unlockAction;

    //    public List<SkillUnlock> requirements = new List<SkillUnlock> ();

    //    public SkillUnlock (string unlockName, string unlockDescription, int levelRestriction, int cost)
    //    {
    //        this.unlockName = unlockName;
    //        this.unlockDescription = unlockDescription;
    //        this.levelRestriction = levelRestriction;
    //        this.cost = cost;
    //    }

    //    public void SetUnlockAction(System.Action action)
    //    {
    //        unlockAction = action;
    //    }

    //    public void SetRequirements (params SkillUnlock[] requirements)
    //    {
    //        for (int i = 0; i < requirements.Length; i++)
    //        {
    //            this.requirements.Add ( requirements[i] );
    //        }
    //    }
    //}
}
