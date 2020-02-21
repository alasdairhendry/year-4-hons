using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestActionHelper : MonoBehaviour
{
    public static QuestActionHelper instance;
    [SerializeField] private List<QuestInteractable> interactables = new List<QuestInteractable> ();
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
}
