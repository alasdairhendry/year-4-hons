using QuestFlow.DialogueEngine;
using QuestFlow.QuestEngine;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class Actor : MonoBehaviour
    {
        public ActorData actor;
        public Conversation defaultConversation;
        public List<Quest> questsGiven = new List<Quest> ();
        private WorldMapObject worldMapObject;
        private NPC npc;

        private void Awake ()
        {
            npc = GetComponent<NPC> ();
        }

        private void Start ()
        {
            SkinnedMeshRenderer smr = GetComponentInChildren<SkinnedMeshRenderer> ( false );

            if (smr == null)
            {
                Debug.Log ( "ehh.." );
            }
            else
            {
                actor.sprite = Resources.Load<Sprite> ( string.Format ( "ActorSprites/{0} ({1})", smr.sharedMesh.name, smr.sharedMaterial.name ) );
            }

            if (questsGiven.Count > 0)
            {
                worldMapObject = GetComponent<WorldMapObject> ();
                if (worldMapObject == null)
                {
                    Debug.Log ( "Quest giver does not have a world map object" );
                }
                else
                {
                    CheckQuestConditions ();
                    QuestManager.instance.onQuestStateChanged += OnQuestStateChanged;
                    QuestManager.instance.onQuestSubStateChanged += OnQuestSubStateChanged;
                }
            }
        }


        private void OnQuestStateChanged (Quest arg1, QuestState arg2)
        {
            CheckQuestConditions ();
        }

        private void OnQuestSubStateChanged (Quest obj)
        {
            CheckQuestConditions ();
        }

        private void CheckQuestConditions ()
        {
            if (worldMapObject == null)
            {
                Debug.Log ( "Quest giver does not have a world map object" );
                return;
            }

            for (int i = 0; i < questsGiven.Count; i++)
            {
                if (QuestManager.instance.QuestCanBeOffered ( questsGiven[i] ))
                {
                    worldMapObject.Register ();
                    return;
                }
            }

            worldMapObject.Unregister ();
        }

        public void StartDialogue ()
        {
            DialogueManager.instance.StartConversation ( defaultConversation );

            if (npc != null)
                npc.isInConversation = true;
        }

        private void Update ()
        {
            if (npc != null && npc.isInConversation)
            {
                Quaternion lookDir = Quaternion.LookRotation ( (EntityManager.instance.PlayerCharacter.transform.position - transform.position).normalized );
                transform.rotation = Quaternion.Slerp ( transform.rotation, lookDir, Time.deltaTime * 5.0f );
            }
        }
    }
}