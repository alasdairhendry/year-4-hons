using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFaction : MonoBehaviour
{
    private Faction currentFaction = null;

    [SerializeField] private bool allowFactionToBeSetExternally = true;
    [SerializeField] private bool overrideDefaultFaction = false;
    [NaughtyAttributes.EnableIf ( nameof ( overrideDefaultFaction ) )] [SerializeField] private Faction overrideFaction;

    public Faction CurrentFaction
    {
        get
        {
            if (currentFaction == null)
            {
                CheckDefaultFaction ();
            }

            if (currentFaction == null)
            {
                Debug.LogError ( "This needs fixed." );
            }

            return currentFaction;
        }
        set
        {
            currentFaction = value;
        }
    }

    private void Awake ()
    {
        if(currentFaction == null)
        {
            CheckDefaultFaction ();
        }
    }

    private void CheckDefaultFaction ()
    {
        if (overrideDefaultFaction)
        {
            if (overrideFaction != null)
            {
                currentFaction = overrideFaction;
            }
        }
        else
        {
            if (GetComponent<NPC> () != null)
            {
                currentFaction = GetComponent<NPC> ().NpcData.Faction;
            }
        }
    }

    public void SetFaction (Faction faction)
    {
        if (allowFactionToBeSetExternally)
            currentFaction = faction;
    }
}
