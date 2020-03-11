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
    }

    public void AddXp (float amount)
    {
        if (currentLevel >= SkillModifiers.MAX_SKILL_LEVEL) return;

        currentXP += amount;
        onXPGained?.Invoke ( amount, skillType );

        CheckLevelUp ();
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

    private void CheckLevelUp ()
    {
        if (currentLevel >= SkillModifiers.MAX_SKILL_LEVEL) return;

        for (int i = currentLevel; i < SkillManager.instance.XpRatesByLevel.Count; i++)
        {
            if (i == SkillManager.instance.XpRatesByLevel.Count - 1)
            {
                if (currentXP >= SkillManager.instance.XpRatesByLevel[i])
                {
                    currentLevel = 99;
                    onLevelUp?.Invoke ( currentLevel, skillType );
                    SoundEffectManager.Play ( AudioClipAsset.LevelUp, AudioMixerGroup.SFX );
                    break;
                }
            }
            else
            {
                if (currentXP >= SkillManager.instance.XpRatesByLevel[i] && currentXP < SkillManager.instance.XpRatesByLevel[i + 1])
                {
                    currentLevel = i + 1;
                    currentLevel = Mathf.Clamp ( currentLevel, 1, 99 );
                    onLevelUp?.Invoke ( currentLevel, skillType );
                    SoundEffectManager.Play ( AudioClipAsset.LevelUp, AudioMixerGroup.SFX );
                    break;
                }
            }

        }
    }
}
