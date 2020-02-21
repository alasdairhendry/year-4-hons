using QuestFlow.QuestEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestFlow
{
    public class QuestLogCanvas : UIPanel
    {
        public static QuestLogCanvas instance;      

        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject questLogPanel;
        [SerializeField] private GameObject questLogItem;
        [Space]
        [SerializeField] private GameObject questDisplayPanel;
        [SerializeField] private TextMeshProUGUI questDisplayTitle;
        [SerializeField] private TextMeshProUGUI questDisplayBody;
        [SerializeField] private ScrollRect questDisplayBodyScrollRect;
        [SerializeField] private Button closeButton;

        private Dictionary<string, GameObject> questIDToLogItemDictionary = new Dictionary<string, GameObject> ();
        private Dictionary<string, string> questIDToLogDataDictionary = new Dictionary<string, string> ();
        private Quest currentDisplayedQuest;

        [SerializeField] private Animator updatedPanelAnimator;

        private void Awake ()
        {
            if (instance == null) instance = this;
            else if (instance != this) { Destroy ( this.gameObject ); return; }
        }

        private void Start ()
        {
            CreateQuestLogItems ();
            QuestManager.instance.onQuestStateChanged += UpdateQuestState;
            QuestManager.instance.onQuestSubStateChanged += UpdateQuestLog;
            Close ();
        }

        public override void Open ()
        {
            base.Open ();
            mainPanel.SetActive ( true );
            isOpened = true;
            HideQuestDisplayPanel ();
        }

        public override void Close ()
        {
            base.Close ();
            mainPanel.SetActive ( false );
            isOpened = false;
        }

        private void CreateQuestLogItems ()
        {
            List<Quest> questsOrdered = QuestManager.instance.GetQuests.OrderBy ( x => x.questName ).ToList ();

            for (int i = 0; i < questsOrdered.Count; i++)
            {
                Quest quest = questsOrdered[i];
                QuestManager.QuestData questData = QuestManager.instance.GetQuestDataByID ( quest.questID );
                GameObject item = questLogItem;

                if (i >= 1)
                {
                    item = Instantiate ( questLogItem, questLogItem.transform.parent );
                }

                questIDToLogItemDictionary.Add ( quest.questID, item );
                questIDToLogDataDictionary.Add ( quest.questID, QuestManager.instance.GetQuestLogEntry ( quest, "" ) );

                QuestState questState = questData.currentQuestState;
                SetQuestLogNameData ( item.GetComponentInChildren<TextMeshProUGUI> (), quest );
                item.GetComponentInChildren<Button> ().onClick.AddListener ( () => { OpenQuestDisplayPanel ( quest ); } );
            }

            if (questsOrdered.Count <= 0)
            {
                questLogItem.SetActive ( false );
            }
        }       

        private void UpdateQuestState (Quest quest, QuestState questState)
        {
            SetQuestLogNameData ( questIDToLogItemDictionary[quest.questID].GetComponentInChildren<TextMeshProUGUI> (), quest );
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

            textMesh.text = string.Format ( "<color={0}>{1}</color>", colour, quest.questName );
        }

        private void UpdateQuestLog (Quest quest)
        {
            updatedPanelAnimator.SetTrigger ( "trigger" );
            questIDToLogDataDictionary[quest.questID] = QuestManager.instance.GetQuestLogEntry ( quest, questIDToLogDataDictionary[quest.questID] );

            if (currentDisplayedQuest == quest)
            {
                questDisplayBody.text = questIDToLogDataDictionary[quest.questID];
                StartCoroutine ( SetNormalisedVerticalScrollPositionAfterFrame () );
            }
        }

        private void OpenQuestDisplayPanel(Quest quest)
        {
            questDisplayPanel.SetActive ( true );
            questLogPanel.SetActive ( false );
            currentDisplayedQuest = quest;
            questDisplayTitle.text = quest.questName;
            questDisplayBody.text = questIDToLogDataDictionary[quest.questID];
            StartCoroutine ( SetNormalisedVerticalScrollPositionAfterFrame () );
            closeButton.onClick.RemoveAllListeners ();
            closeButton.onClick.AddListener ( () => { HideQuestDisplayPanel (); } );
        }

        private IEnumerator SetNormalisedVerticalScrollPositionAfterFrame ()
        {
            yield return new WaitForEndOfFrame ();
            questDisplayBodyScrollRect.verticalNormalizedPosition = 0.0f;
        }

        public void HideQuestDisplayPanel ()
        {
            questLogPanel.SetActive ( true );
            questDisplayPanel.SetActive ( false );
            currentDisplayedQuest = null;
            questDisplayTitle.text = "Quest Log";
            questDisplayBody.text = "";
            closeButton.onClick.RemoveAllListeners ();
            closeButton.onClick.AddListener ( () => { Close (); } );
        }
    }
}
