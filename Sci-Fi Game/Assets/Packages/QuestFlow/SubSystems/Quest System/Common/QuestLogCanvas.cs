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
        [SerializeField] private TMP_Dropdown filterDropdown;

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
            CreateQuests ();
            RefreshQuestsFiltered ();
            filterDropdown.onValueChanged.AddListener ( (value) => { RefreshQuestsFiltered (); } );
            QuestManager.instance.onQuestStateChanged += OnQuestStateChanged;
            QuestManager.instance.onQuestSubStateChanged += OnQuestSubStateChanged;
            Close ( true );
        }

        private void CreateQuests ()
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
                RefreshSpecificQuestColour ( item.GetComponentInChildren<TextMeshProUGUI> (), quest );
                item.GetComponentInChildren<Button> ().onClick.AddListener ( () => { OpenQuestDisplayPanel ( quest ); } );
            }

            if (questsOrdered.Count <= 0)
            {
                questLogItem.SetActive ( false );
            }
        }

        private void OnQuestStateChanged (Quest quest, QuestState questState)
        {
            RefreshQuestColours (quest);
            RefreshQuestsFiltered ();
        }

        private void OnQuestSubStateChanged (Quest quest)
        {
            updatedPanelAnimator.SetTrigger ( "trigger" );
            questIDToLogDataDictionary[quest.questID] = QuestManager.instance.GetQuestLogEntry ( quest, questIDToLogDataDictionary[quest.questID] );
            RefreshQuestsFiltered ();
            RefreshQuestLogInformation ( quest );
        }

        public override void Open ()
        {
            base.Open ();
            mainPanel.SetActive ( true );
            isOpened = true;
            HideQuestDisplayPanel ();
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

        private void OpenQuestDisplayPanel (Quest quest)
        {
            questDisplayPanel.SetActive ( true );
            questLogPanel.SetActive ( false );
            currentDisplayedQuest = quest;
            questDisplayTitle.text = quest.questName;

            RefreshQuestLogInformation ( quest );

            closeButton.onClick.RemoveAllListeners ();
            closeButton.onClick.AddListener ( () => { HideQuestDisplayPanel (); } );
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

        private void RefreshQuestsFiltered ()
        {
            int currentFilter = filterDropdown.value;

            List<Quest> filteredQuests = new List<Quest> ();

            if (currentFilter == 0)
            {
                filteredQuests = QuestManager.instance.GetQuests;
            }
            else if (currentFilter == 1)
            {
                filteredQuests = QuestManager.instance.GetQuestDatas.Where ( x => x.currentQuestState == QuestState.NotStarted ).Select ( x => x.quest ).Where ( x => x.CanBeOffered () ).ToList ();
            }
            else if (currentFilter == 2)
            {
                filteredQuests = QuestManager.instance.GetQuestDatas.Where ( x => x.currentQuestState == QuestState.InProgress ).Select ( x => x.quest ).ToList ();
            }
            else if (currentFilter == 3)
            {
                filteredQuests = QuestManager.instance.GetQuestDatas.Where ( x => x.currentQuestState == QuestState.Completed ).Select ( x => x.quest ).ToList ();
            }
            else if (currentFilter == 4)
            {
                filteredQuests = QuestManager.instance.GetQuestDatas.Where ( x => x.currentQuestState == QuestState.NotStarted ).Select ( x => x.quest ).Where ( x => x.CanBeOffered () == false ).ToList ();
            }

            for (int i = 0; i < QuestManager.instance.GetQuests.Count; i++)
            {
                questIDToLogItemDictionary[QuestManager.instance.GetQuests[i].questID].SetActive ( false );
            }

            for (int i = 0; i < filteredQuests.Count; i++)
            {
                questIDToLogItemDictionary[filteredQuests[i].questID].SetActive ( true );
            }
        }

        private void RefreshQuestColours (Quest quest)
        {
            RefreshSpecificQuestColour ( questIDToLogItemDictionary[quest.questID].GetComponentInChildren<TextMeshProUGUI> (), quest );
        }

        private void RefreshSpecificQuestColour (TextMeshProUGUI textMesh, Quest quest)
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

        private void RefreshQuestLogInformation (Quest quest)
        {         
            if (currentDisplayedQuest == quest)
            {
                if (quest.CanBeOffered ())
                {
                    questDisplayBody.text = questIDToLogDataDictionary[quest.questID];
                }
                else
                {
                    List<KeyValuePair<Quest, QuestState>> conditions = quest.GetPrerequisiteQuests ();

                    if (conditions.Count > 0)
                    {
                        List<KeyValuePair<Quest, QuestState>> conditionsInProgress = conditions.Where ( x => x.Value == QuestState.InProgress ).ToList ();
                        List<KeyValuePair<Quest, QuestState>> conditionsCompleted = conditions.Where ( x => x.Value == QuestState.Completed ).ToList ();
                        questDisplayBody.text = "<align=center>To start this quest I need to have\n\n";

                        if (conditionsInProgress.Count > 0)
                        {
                            questDisplayBody.text += "<b>--- Started ---</b>";
                            for (int i = 0; i < conditionsInProgress.Count; i++)
                            {
                                questDisplayBody.text += "\n" + QuestManager.instance.GetQuestDataByID ( conditionsInProgress[i].Key.questID ).quest.questName;
                            }

                            questDisplayBody.text += "\n\n";
                        }

                        if (conditionsCompleted.Count > 0)
                        {
                            questDisplayBody.text += "<b>--- Completed ---</b>";
                            for (int i = 0; i < conditionsCompleted.Count; i++)
                            {
                                questDisplayBody.text += "\n" + QuestManager.instance.GetQuestDataByID ( conditionsCompleted[i].Key.questID ).quest.questName;
                            }
                        }
                    }
                    else
                    {
                        questDisplayBody.text = questIDToLogDataDictionary[quest.questID];
                    }
                }

                StartCoroutine ( SetNormalisedVerticalScrollPositionAfterFrame () );
            }
        }

        private IEnumerator SetNormalisedVerticalScrollPositionAfterFrame ()
        {
            yield return new WaitForEndOfFrame ();
            questDisplayBodyScrollRect.verticalNormalizedPosition = 0.0f;
        }
    }
}
