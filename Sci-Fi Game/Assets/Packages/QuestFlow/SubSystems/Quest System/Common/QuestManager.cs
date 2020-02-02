using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow.QuestEngine
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager instance;

        [SerializeField] private List<Quest> quests = new List<Quest> ();
        public List<Quest> GetQuests { get => quests; }

        private Dictionary<string, QuestData> questDataByID = new Dictionary<string, QuestData> ();
        private List<QuestData> questData = new List<QuestData> ();

        public System.Action<Quest, QuestState> onQuestStateChanged;
        public System.Action<Quest> onQuestSubStateChanged;
        public System.Action<Quest> onQuestStarted;
        public System.Action<Quest> onQuestComplete;
        public System.Action<Quest> onQuestFailed;

        private void Awake ()
        {
            if (instance == null) instance = this;
            else if (instance != this) { Destroy ( this.gameObject ); return; }

            for (int i = 0; i < quests.Count; i++)
            {
                QuestData _questData = new QuestData ( quests[i], quests[i].GetStartNode () );
                _questData.currentNode.OnEnterNode ();
                questDataByID.Add ( quests[i].questID, _questData );
                questData.Add ( _questData );
            }

            StartQuest ( quests[0] );

            onQuestStateChanged += (q, qs) => { Debug.Log ( string.Format ( "Quest {0} has changed to state {1}", q.questID, qs ) ); };
            onQuestSubStateChanged += (q) => { Debug.Log ( string.Format ( "Quest {0} has changed to a new substate", q.questID ) ); };
            onQuestStarted += (q) => { Debug.Log ( string.Format ( "Quest {0} has been started", q.questID ) ); };
            onQuestComplete += (q) => { Debug.Log ( string.Format ( "Quest {0} has been complete", q.questID ) ); };
            onQuestFailed += (q) => { Debug.Log ( string.Format ( "Quest {0} has been failed", q.questID ) ); };
        }

        private void Update ()
        {
            for (int i = 0; i < questData.Count; i++)
            {
                if (questData[i].currentQuestState == QuestState.InProgress)
                {
                    if (questData[i].currentNode.conditions.Count > 0)
                    {
                        NodeBase nextNode = null;

                        if (questData[i].quest.GetNextNodeFromCurrent ( questData[i].currentNode, out nextNode ))
                        {
                            SetQuestSubstate ( questData[i].quest, nextNode );
                        }
                    }
                }
            }
        }

        public QuestData GetQuestDataByID(string id)
        {
            if (questDataByID.ContainsKey ( id ))
            {
                return questDataByID[id];
            }
            else
            {
                return null;
            }
        }

        public bool StartQuest (string questID)
        {
            Quest quest = GetQuestDataByID ( questID ).quest;

            if (quest == null)
            {
                Debug.LogError ( "Quest " + questID + " does not exist" );
                return false;
            }

            return StartQuest ( quest );
        }

        public bool StartQuest (Quest quest)
        {
            if (QuestCanBeOffered ( quest ))
            {
                SetQuestState ( quest, QuestState.InProgress );
                quest.GetStartNode ().DoActions ();

                NodeBase nextNode = null;

                if (questDataByID[quest.questID].quest.GetNextNodeFromCurrent ( questDataByID[quest.questID].currentNode, out nextNode ))
                {
                    SetQuestSubstate ( quest, nextNode );
                }

                onQuestStarted?.Invoke ( quest );
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool QuestStartConditionsMet(Quest quest)
        {
            return quest.CanBeOffered ();
        }

        public bool QuestCanBeOffered(Quest quest)
        {
            if (questDataByID[quest.questID].currentQuestState != QuestState.NotStarted) return false;
            return quest.CanBeOffered ();
        }

        private void CompleteQuest(Quest quest)
        {
            SetQuestState ( quest, QuestState.Completed );
            onQuestComplete?.Invoke ( quest );
        }

        private void FailQuest (Quest quest)
        {
            SetQuestState ( quest, QuestState.Failed );
            onQuestFailed?.Invoke ( quest );
        }

        public void SetQuestState (Quest quest, QuestState questState)
        {
            questDataByID[quest.questID].currentQuestState = questState;
            onQuestStateChanged?.Invoke ( quest, questState );
        }

        public void SetQuestSubstate (Quest quest, NodeBase subState)
        {
            Debug.Log ( "SetQuestSubstate" );

            if (quest == null)
            {
                Debug.LogError ( "aefaf" );
                return;
            }

            if (questDataByID[quest.questID].currentNode != null)
                questDataByID[quest.questID].currentNode.OnExitNode ();

            questDataByID[quest.questID].currentNode = subState;
            questDataByID[quest.questID].currentNode.OnEnterNode ();
            questDataByID[quest.questID].currentNode.DoActions ();
            onQuestSubStateChanged?.Invoke ( quest );

            if (subState is Success)
            {
                CompleteQuest ( quest );
            }
            else if (subState is Failed)
            {
                FailQuest ( quest );
            }
        }

        public string GetQuestLogEntry(string questID)
        {
            Quest quest = GetQuestDataByID ( questID ).quest;

            if (quest == null)
            {
                Debug.LogError ( "Quest " + questID + " does not exist" );
                return "";
            }

            return GetQuestLogEntry ( quest );
        }

        public string GetQuestLogEntry(Quest quest)
        {
            return questDataByID[quest.questID].currentNode.questLog;
        }

        private void OnDestroy ()
        {
            for (int i = 0; i < questData.Count; i++)
            {
                questData[i].currentQuestState = QuestState.NotStarted;

                for (int x = 0; x < questData[i].quest.nodes.Count; x++)
                {
                    if (questData[i].quest.nodes[x] == null) continue;
                    (questData[i].quest.nodes[x] as NodeBase).OnExitNode ();
                }
            }
        }

        [System.Serializable]
        public class QuestData
        {
            public Quest quest;
            public NodeBase currentNode;
            [HideInInspector] public QuestState currentQuestState;

            public QuestData (Quest quest, NodeBase currentNode)
            {
                this.quest = quest;
                this.currentNode = currentNode;
            }
        }
    }
}
