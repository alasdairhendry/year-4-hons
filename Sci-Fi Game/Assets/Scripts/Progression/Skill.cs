using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public string skillDescription;

    public float currentXP = 0.0f;
    public int currentLevel = 1;

    public Skill (string skillName, string skillDescription)
    {
        this.skillName = skillName;
        this.skillDescription = skillDescription;      
    }

    public void AddXp (float amount)
    {
        currentXP += amount;
        Debug.Log ( "Added xp " + amount + " to " + skillName + " - Now have " + currentXP );

        if (currentXP >= SkillManager.instance.GetXPRequirementForLevel ( currentLevel ))
        {
            OnLevelUp ();
        }
    }

    public float GetXPRemaining ()
    {
        return SkillManager.instance.GetXPRequirementForLevel ( currentLevel ) - currentXP;
    }

    public float GetNextLevelTotalXP ()
    {
        return SkillManager.instance.GetXPRequirementForLevel ( currentLevel );
    }

    public void OnLevelUp ()
    {
        Debug.Log ( "Level up" );
        currentLevel++;
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
