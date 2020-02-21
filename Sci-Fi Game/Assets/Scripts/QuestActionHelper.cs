using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestActionHelper : MonoBehaviour
{
    public static QuestActionHelper instance;
    [SerializeField] private List<QuestInteractable> interactables = new List<QuestInteractable> ();
    [SerializeField] private List<QuestMobSpawner> mobSpawners = new List<QuestMobSpawner> ();
    [SerializeField] private NPCShopkeeper horvik;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
    }

    public void SetQuestInteractableState (QuestInteractable.InteractableID id, bool state)
    {
        QuestInteractable qi = interactables.First ( x => x.id == id );
        if (qi == null)
        {
            Debug.LogError ( "Quest interactable " + id.ToString () + " does not exist" );
            return;
        }

        qi.interactable.IsInteractable = state;
    }

    public void SpawnMobSpawner (QuestMobSpawner.MobSpawnerID mobSpawnerID, int amount)
    {
        QuestMobSpawner qms = mobSpawners.First ( x => x.id == mobSpawnerID );

        if (qms == null)
        {
            Debug.LogError ( "Quest mob spawner " + mobSpawnerID.ToString () + " does not exist" );
            return;
        }

        qms.mobSpawner.SpawnSetAmount ( amount );
    }

    public void DespawnAllMobSpawner (QuestMobSpawner.MobSpawnerID mobSpawnerID)
    {
        QuestMobSpawner qms = mobSpawners.First ( x => x.id == mobSpawnerID );

        if (qms == null)
        {
            Debug.LogError ( "Quest mob spawner " + mobSpawnerID.ToString () + " does not exist" );
            return;
        }

        qms.mobSpawner.DespawnAll ();
    }

    public void OpenHorviksToolStore ()
    {
        horvik.OpenShop ();
    }

    [System.Serializable]
    public class QuestInteractable
    {
        public enum InteractableID { AggiesDoor, MedicalStationSign }
        public InteractableID id;
        public Interactable interactable;
    }

    [System.Serializable]
    public class QuestMobSpawner
    {
        public enum MobSpawnerID { EvilCivilian }
        public MobSpawnerID id;
        public MobSpawner mobSpawner;
    }
}
