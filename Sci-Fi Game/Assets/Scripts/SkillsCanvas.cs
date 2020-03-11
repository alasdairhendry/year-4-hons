using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillsCanvas : UIPanel
{
    public static SkillsCanvas instance;

    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject skillUIPrefab;
    [SerializeField] private Transform skillUIParent;
    [SerializeField] private TextMeshProUGUI characterLevelText;
    [SerializeField] private TextMeshProUGUI combatLevelText;
    [SerializeField] private TextMeshProUGUI factionNameText;
    [SerializeField] private TextMeshProUGUI factionSpecText;

    private Dictionary<SkillType, SkillEntryUIPanel> panels = new Dictionary<SkillType, SkillEntryUIPanel> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        SkillManager.instance.OnCharacterLevelIncreased += OnCharacterLevelIncreased;

        for (int i = 0; i < SkillManager.instance.Skills.Count; i++)
        {
            int x = i;

            GameObject go = Instantiate ( skillUIPrefab );
            go.transform.SetParent ( skillUIParent );

            SkillEntryUIPanel panel = go.GetComponent<SkillEntryUIPanel> ();

            SkillManager.instance.Skills[i].onXPGained += OnXPGained;
            SkillManager.instance.Skills[i].onLevelUp += OnSkillLevelUp;
            panels.Add ( SkillManager.instance.Skills[i].skillType, panel );
            panel.Initialise ( SkillManager.instance.Skills[i].skillType );
            UpdateSkillUI ( SkillManager.instance.Skills[i].skillType );
        }

        Close ( true );
    }

    public override void Open ()
    {
        base.Open ();
        isOpened = true;
        mainPanel.SetActive ( true );
        UIPanelController.instance.OnPanelOpened ( this );

        factionNameText.text = EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionName;
        factionSpecText.text = EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.specialisationDescription;
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        isOpened = false;
        mainPanel.SetActive ( false );
        UIPanelController.instance.OnPanelClosed ( this );
    }

    private void OnXPGained (float xpGained, SkillType skillType)
    {
        UpdateSkillUI ( skillType );
    }

    private void OnSkillLevelUp (int newLevel, SkillType skillType)
    {
        UpdateSkillUI ( skillType );
    }

    private void OnCharacterLevelIncreased ()
    {
        characterLevelText.text = Mathf.FloorToInt ( SkillManager.instance.CharacterLevel ).ToString ( "00" );
    }

    private void UpdateSkillUI (SkillType skillType)
    {
        combatLevelText.text = Mathf.FloorToInt ( SkillManager.instance.CombatLevel ).ToString ( "00" );
        panels[skillType].skillNameText.text = SkillManager.instance.GetSkill ( skillType ).skillName;
        panels[skillType].skillLevelText.text = "Lv. " + SkillManager.instance.GetSkill ( skillType ).currentLevel.ToString ( "00" );

        if (SkillManager.instance.GetSkill ( skillType ).currentLevel >= SkillModifiers.MAX_SKILL_LEVEL)
        {
            panels[skillType].skillXPText.text = "MAX LEVEL";
            panels[skillType].skillProgressBar.SetValue ( 1.0f );
        }
        else
        {
            panels[skillType].skillXPText.text = string.Format ( "{0:0.#}", SkillManager.instance.GetSkill ( skillType ).GetRelativeCurrentXP () ) + " / " + string.Format ( "{0:0.##}", SkillManager.instance.GetSkill ( skillType ).GetNextLevelRelativeXPRequirement () ) + " xp";
            panels[skillType].skillProgressBar.SetValue ( SkillManager.instance.GetSkill ( skillType ).GetProgressToNextLevelNormalised () );
        }
    }
}
