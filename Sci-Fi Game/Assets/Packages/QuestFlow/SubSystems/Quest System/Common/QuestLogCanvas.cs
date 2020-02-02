using QuestFlow.QuestEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestFlow
{
    public class QuestLogCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject questLogPanel;
        [SerializeField] private GameObject questLogItem;
        [Space]
        [SerializeField] private GameObject questDisplayPanel;
        [SerializeField] private TextMeshProUGUI questDisplayTitle;
        [SerializeField] private TextMeshProUGUI questDisplayBody;

        private Dictionary<string, GameObject> questIDToLogItemDictionary = new Dictionary<string, GameObject> ();
        private Quest currentDisplayedQuest;

        private void Start ()
        {
            CreateQuestLogItems ();
            HideQuestDisplayPanel ();
            QuestManager.instance.onQuestStateChanged += UpdateQuestState;
            QuestManager.instance.onQuestSubStateChanged += UpdateQuestLog;
        }

        private void CreateQuestLogItems ()
        {
            for (int i = 0; i < QuestManager.instance.GetQuests.Count; i++)
            {
                Quest quest = QuestManager.instance.GetQuests[i];
                QuestManager.QuestData questData = QuestManager.instance.GetQuestDataByID ( quest.questID );
                GameObject item = questLogItem;

                if (i >= 1)
                {
                    item = Instantiate ( questLogItem, questLogItem.transform.parent );
                }

                questIDToLogItemDictionary.Add ( quest.questID, item );

                QuestState questState = questData.currentQuestState;
                SetQuestLogNameData ( item.GetComponentInChildren<TextMeshProUGUI> (), quest );
                item.GetComponent<Button> ().onClick.AddListener ( () => { OpenQuestDisplayPanel ( quest ); } );
            }

            if (QuestManager.instance.GetQuests.Count <= 0)
            {
                questLogItem.SetActive ( false );
            }
        }       

        private void UpdateQuestState (Quest quest, QuestState questState)
        {
            SetQuestLogNameData ( questIDToLogItemDictionary[quest.questID].GetComponentInChildren<TextMeshProUGUI> (), quest );
            UpdateQuestLog ( quest );
        }

        private void SetQuestLogNameData (TextMeshProUGUI textMesh, Quest quest)
        {
            string colour = "";
            QuestManager.QuestData data = QuestManager.instance.GetQuestDataByID ( quest.questID );

            switch (QuestManager.instance.GetQuestDataByID ( quest.questID ).currentQuestState)
            {
                case QuestState.NotStarted:
                    colour = "red";
                    break;
                case QuestState.InProgress:
                    colour = "yellow";
                    break;
                case QuestState.Completed:
                    colour = "green";
                    break;
                case QuestState.Failed:
                    colour = "gray";
                    break;
                default:
                    break;
            }

            textMesh.text = string.Format ( "<color={0}>{1}</color>", colour, quest.questName + " - " + data.currentNode.name );
        }

        private void UpdateQuestLog (Quest quest)
        {
            if(currentDisplayedQuest == quest)
            {
                questDisplayBody.text = QuestManager.instance.GetQuestLogEntry ( quest );
            }
        }

        private void OpenQuestDisplayPanel(Quest quest)
        {
            questDisplayPanel.SetActive ( true );
            questLogPanel.SetActive ( false );
            currentDisplayedQuest = quest;
            questDisplayTitle.text = quest.questName;
            questDisplayBody.text = QuestManager.instance.GetQuestLogEntry ( quest );
        }

        public void HideQuestDisplayPanel ()
        {
            questLogPanel.SetActive ( true );
            questDisplayPanel.SetActive ( false );
            currentDisplayedQuest = null;
            questDisplayTitle.text = "";
            questDisplayBody.text = "";
        }
    }
}
