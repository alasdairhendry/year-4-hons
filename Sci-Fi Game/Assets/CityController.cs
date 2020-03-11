using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityController : MonoBehaviour
{
    [SerializeField] private float delayBeforeDeath = 5.0f;
    [SerializeField] private float delayBeforeAlarms = 1.0f;
    private List<AudioSource> alarmAudioSources = new List<AudioSource> ();

    private bool isInCity = true;
    private bool alarmIsSounding = false;
    private bool hasKilled = false;

    private float deathDelayCounter = 0;

    private void Awake ()
    {
        alarmAudioSources = GetComponentsInChildren<AudioSource> ().ToList();
    }

    private void Start ()
    {
        EntityManager.instance.PlayerCharacter.OnRespawn += () =>
        {
            OnCharacterEnterCity ();
            hasKilled = false;
        };
    }

    private void Update ()
    {
        if (!isInCity)
        {
            deathDelayCounter += Time.deltaTime;

            if (alarmIsSounding == false)
            {
                if (deathDelayCounter >= delayBeforeAlarms)
                {
                    SoundAlarms ();
                }
            }

            if (hasKilled == false)
            {
                if (deathDelayCounter >= delayBeforeDeath)
                {
                    DestroyCharacter ();
                }
            }
        }
    }

    private void SoundAlarms ()
    {
        if (alarmIsSounding) return;
        alarmIsSounding = true;

        for (int i = 0; i < alarmAudioSources.Count; i++)
        {
            alarmAudioSources[i].loop = true;
            alarmAudioSources[i].Play ();
        }
    }

    private void StopAlarms ()
    {
        if (!alarmIsSounding) return;
        alarmIsSounding = false;

        for (int i = 0; i < alarmAudioSources.Count; i++)
        {
            alarmAudioSources[i].loop = false;
        }
    }

    private void DestroyCharacter ()
    {
        if (hasKilled) return;
        hasKilled = true;

        if (EntityManager.instance.PlayerCharacter.currentVehicle != null)
        {
            EntityManager.instance.PlayerCharacter.currentVehicle.health.RemoveHealth ( float.MaxValue, DamageType.FireDamage );
        }

        EntityManager.instance.PlayerCharacter.Health.RemoveHealth ( float.MaxValue, DamageType.FireDamage );
    }

    public void OnCharacterEnterCity ()
    {
        if (isInCity) return;
        StopAlarms ();
        isInCity = true;
        deathDelayCounter = 0;
        hasKilled = false;
    }

    public void OnCharacterLeaveCity ()
    {
        if (!isInCity) return;
        isInCity = false;
    }
}
