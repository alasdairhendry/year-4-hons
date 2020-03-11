using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    public static TalentManager instance;

    [SerializeField] private List<TalentData> talentData = new List<TalentData> ();
    private Dictionary<TalentType, Talent> talentDictionary = new Dictionary<TalentType, Talent> ();

    public int CurrentTalentPoints { get; protected set; } = 0;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        CreateDictionary ();
    }

    private void CreateDictionary ()
    {
        for (int i = 0; i < talentData.Count; i++)
        {
            talentDictionary.Add ( talentData[i].talentType, new Talent ( talentData[i], 1 ) );
        }
    }

    public Talent GetTalent (TalentType type)
    {
        return talentDictionary[type];
    }

    public float GetTalentModifier (TalentType type)
    {
        int level = GetTalent ( type ).currentLevel;
        float baseModifier = 0.05f;
        float divisiveModifier = 0.5f;
        float currentModifier = 0;

        for (int i = 1; i < level; i++)
        {
            currentModifier += baseModifier;
            baseModifier *= divisiveModifier;
        }

        return currentModifier;
    }

    public void AddTalentPoint (int amount = 1)
    {
        if (amount <= 0) return;
        CurrentTalentPoints += amount;
        MessageBox.AddMessage ( "You've just recieved " + ColourHelper.TagColour ( amount.ToString (), ColourDescription.DarkYellowText ) + " talent " + amount.Pluralise ( "point", "points" ) + ". You have " + CurrentTalentPoints + " to spend" );
    }

    public void UpgradeTalent (TalentType type)
    {
        if (CurrentTalentPoints > 0)
        {
            if (GetTalent ( type ).currentLevel < GetTalent ( type ).talentData.maxLevels)
            {
                CurrentTalentPoints--;
                GetTalent ( type ).currentLevel++;
                MessageBox.AddMessage ( "Congratulations! Your " + GetTalent ( type ).talentData.talentName + " talent is now level " + GetTalent ( type ).currentLevel );
            }
            else
            {

                MessageBox.AddMessage ( "I can't upgrade that talent any further!", MessageBox.Type.Error );
            }
        }
        else
        {
            MessageBox.AddMessage ( "I need more talent points to perform this upgrade!", MessageBox.Type.Error );
        }
    }

    public void DowngradeTalent (TalentType type)
    {
        if (GetTalent ( type ).currentLevel > 1)
        {
            GetTalent ( type ).currentLevel--;
            CurrentTalentPoints++;
            MessageBox.AddMessage ( "You have downgraded your " + GetTalent ( type ).talentData.talentName + " talent" );
        }
        else
        {
            MessageBox.AddMessage ( "I can't downgrade that talent any further!", MessageBox.Type.Error );
        }
    }

    public class Talent
    {
        public Talent (TalentData talentData, int currentLevel)
        {
            this.talentData = talentData;
            this.currentLevel = currentLevel;
        }

        public TalentData talentData { get; protected set; }
        public int currentLevel { get; set; }
    }
}
