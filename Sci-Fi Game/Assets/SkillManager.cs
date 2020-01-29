using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public enum SkillType { Shooting }

    public GameObject uiPrefab;
    public CanvasGroup canvasGroup;
    public GameObject canvasBody;
    public Skill s;

    private Dictionary<SkillType, XPTrackerUIPrefab> skillUIs = new Dictionary<SkillType, XPTrackerUIPrefab> ();
    private Dictionary<SkillType, Skill> skillDictionary = new Dictionary<SkillType, Skill> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy ( this.gameObject );

        GenerateSkills ();
        GenerateUI ();
    }

    private void GenerateSkills ()
    {
        Skill.SkillUnlock skillUnlock = new Skill.SkillUnlock ( "Increased Accuracy", "", 1, 1 );
        skillUnlock.SetUnlockAction ( () => { Debug.Log ( "Unlocked skill unlocked 01" ); } );

        Skill skill = new Skill ( "Shooting", "da shooting", skillUnlock );
        skillDictionary.Add ( SkillType.Shooting, skill );
        this.s = skill;
    }

    private void GenerateUI ()
    {
        GameObject go = Instantiate ( uiPrefab );
        go.transform.SetParent ( canvasBody.transform );
        skillUIs.Add ( SkillType.Shooting, go.GetComponent<XPTrackerUIPrefab> ().Initialise ( SkillType.Shooting ) );
    }

    public void AddXp (SkillType skill, float amount) {
        GetSkill ( skill ).AddXp ( amount );
        skillUIs[skill].OnXpAdded ();
    }

    public Skill GetSkill(SkillType skillType)
    {
        return skillDictionary[skillType];
    }
}
