using QuestFlow.QuestEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestCompleteCanvas : UIPanel
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private List<ItemStaticPanel> itemPanel = new List<ItemStaticPanel> ();
    [SerializeField] private GameObject textRewardsPanel;
    [SerializeField] private TextMeshProUGUI textRewards;
    [Space]
    [SerializeField] private float nonTextRewardsHeight;
    [SerializeField] private float textRewardsHeight;

    private void Start ()
    {
        Close ( true );
        QuestManager.instance.onQuestComplete += OnQuestComplete;    
    }

    private void OnQuestComplete (Quest obj)
    {
        Open ();
        headerText.text = "Quest Complete";
        bodyText.text = "You have completed " + obj.questName + ".\nYou have recieved these rewards.";

        for (int i = 0; i < itemPanel.Count; i++)
        {
            itemPanel[i].Disable ();
        }

        bool someSentToBank = false;
        bool allSentToBank = true;

        TalentManager.instance.AddTalentPoint ( obj.reward.talentPoints );

        for (int i = 0; i < obj.reward.xpRewards.Count; i++)
        {
            SkillManager.instance.AddXpToSkill ( obj.reward.xpRewards[i].skill, obj.reward.xpRewards[i].xp );
        }

        float mainPanelSizeX = mainPanel.GetComponent<RectTransform> ().sizeDelta.x;

        if (obj.reward.talentPoints > 0 || obj.reward.xpRewards.Count > 0 || obj.reward.annotatedRewards.Count > 0)
        {
            textRewardsPanel.SetActive ( true );
            mainPanel.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( mainPanelSizeX, textRewardsHeight );

            textRewards.text = "";

            if(obj.reward.talentPoints > 0)
            {
                textRewards.text += obj.reward.talentPoints.ToString ( "0" ) + " talent points\n\n";
            }

            for (int i = 0; i < obj.reward.xpRewards.Count; i++)
            {
                textRewards.text += obj.reward.xpRewards[i].xp.ToString ( "0" ) + " " + obj.reward.xpRewards[i].skill.ToString () + " experience\n";

                if (i == obj.reward.xpRewards.Count - 1)
                {
                    textRewards.text += "\n";
                }
            }

            for (int i = 0; i < obj.reward.annotatedRewards.Count; i++)
            {
                textRewards.text += obj.reward.annotatedRewards[i] + "\n\n";
            }
        }
        else
        {
            textRewardsPanel.SetActive ( false );
            mainPanel.GetComponent<RectTransform> ().sizeDelta = new Vector2 ( mainPanelSizeX, nonTextRewardsHeight );
        }

        for (int i = 0; i < obj.reward.mandatoryReward.reward.Count; i++)
        {
            if(i >= itemPanel.Count)
            {
                Debug.LogError ( "Too many items given from quest " + obj.questName );
                return;
            }

            ItemAmountPair reward = obj.reward.mandatoryReward.reward[i];
            itemPanel[i].SetContent ( reward.itemID, reward.amount );

            if (EntityManager.instance.PlayerInventory.CheckCanRecieveItem ( reward.itemID, reward.amount ))
            {
                int returned = EntityManager.instance.PlayerInventory.AddItem ( reward.itemID, reward.amount );
                allSentToBank = false;
            }
            else
            {
                EntityManager.instance.PlayerBankInventory.AddItem ( reward.itemID, reward.amount );
                someSentToBank = true;
            }
        }

        if (allSentToBank)
        {
            MessageBox.AddMessage ( "Your rewards have been sent to your bank" );
        }
        else if (someSentToBank)
        {
            MessageBox.AddMessage ( "Some of your rewards have been sent to your bank" );
        }
    }

    public override void Open ()
    {
        base.Open ();
        mainPanel.SetActive ( true );
        isOpened = true;
        UIPanelController.instance.OnPanelOpened ( this );
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        base.Close ();
        mainPanel.SetActive ( false );
        isOpened = false;
        UIPanelController.instance.OnPanelClosed ( this );
    }
}
