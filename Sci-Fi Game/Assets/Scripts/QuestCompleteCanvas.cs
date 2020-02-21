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

    private void Start ()
    {
        Close ();
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

        for (int i = 0; i < obj.reward.mandatoryReward.reward.Count; i++)
        {
            if(i >= itemPanel.Count)
            {
                Debug.LogError ( "Too many items given from quest " + obj.questName );
                return;
            }

            ItemAmountPair reward = obj.reward.mandatoryReward.reward[i];
            itemPanel[i].SetContent ( reward.itemID, reward.amount );
            EntityManager.instance.PlayerInventory.AddItem ( reward.itemID, reward.amount );
        }
    }

    public override void Open ()
    {
        base.Open ();
        mainPanel.SetActive ( true );
        isOpened = true;
    }

    public override void Close ()
    {
        base.Close ();
        mainPanel.SetActive ( false );
        isOpened = false;
    }
}
