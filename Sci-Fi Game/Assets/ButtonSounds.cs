using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
[RequireComponent ( typeof ( EventTrigger ) )]
public class ButtonSounds : MonoBehaviour
{
    [SerializeField] private EventTrigger eventTrigger;

    private void Awake ()
    {
        if (eventTrigger != null)
            SetListeners ();
    }

    private void OnValidate ()
    {
        Validate ();
    }

    private void Validate ()
    {
        eventTrigger = GetComponent<EventTrigger> ();

        if (eventTrigger == null)
            eventTrigger = gameObject.AddComponent<EventTrigger> ();
    }

    private void SetListeners ()
    {
        EventTrigger.Entry enter = new EventTrigger.Entry ();
        enter.eventID = EventTriggerType.PointerEnter;
        enter.callback.AddListener ( (baseEventData) =>
        {
            if (Input.GetMouseButton ( 0 ) || Input.GetMouseButton ( 1 ) || Input.GetMouseButton ( 2 )) return;
            SoundEffectManager.Play ( AudioClipAsset.UIButtonHover, AudioMixerGroup.UI );
        } );

        EventTrigger.Entry exit = new EventTrigger.Entry ();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener ( (baseEventData) =>
        {
            if (Input.GetMouseButton ( 0 ) || Input.GetMouseButton ( 1 ) || Input.GetMouseButton ( 2 )) return;
            SoundEffectManager.Play ( AudioClipAsset.UIButtonHoverExit, AudioMixerGroup.UI );
        } );

        EventTrigger.Entry click = new EventTrigger.Entry ();
        click.eventID = EventTriggerType.PointerClick;
        click.callback.AddListener ( (baseEventData) =>
        {
            SoundEffectManager.Play ( AudioClipAsset.UIButtonClick, AudioMixerGroup.UI );
        } );

        eventTrigger.triggers.Add ( enter );
        eventTrigger.triggers.Add ( exit );
        eventTrigger.triggers.Add ( click );
    }
}
