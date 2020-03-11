using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public enum AudioMixerGroup { Master, Music, SFX, Ambient, UI }

public class AudioMixerManager : MonoBehaviour
{
    public static AudioMixerManager instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup masterGroup;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup musicGroup;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup sfxGroup;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup ambientGroup;
    [SerializeField] private UnityEngine.Audio.AudioMixerGroup uiGroup;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }

        DontDestroyOnLoad ( this.gameObject );
        SceneManager.sceneLoaded += (scene, loadMode) => {
            audioMixer.SetFloat ( "volume-master", -80 );
            audioMixer.DOSetFloat ( "volume-master", 0, 1 );
        };
    }

    private void Start ()
    {
        audioMixer.SetFloat ( "volume-master", -80.0f );
    }

    public void SetVolume(AudioMixerGroup mixerGroup, float value)
    {
        float logValue = Mathf.Log10 ( value ) * 20.0f;

        switch (mixerGroup)
        {
            case AudioMixerGroup.Master:
                audioMixer.SetFloat ( "volume-master", logValue);
                break;
            case AudioMixerGroup.Music:
                audioMixer.SetFloat ( "volume-music", logValue);
                break;
            case AudioMixerGroup.SFX:
                audioMixer.SetFloat ( "volume-sfx", logValue );
                break;
            case AudioMixerGroup.Ambient:
                audioMixer.SetFloat ( "volume-ambient", logValue );
                break;
            case AudioMixerGroup.UI:
                audioMixer.SetFloat ( "volume-ui", logValue );
                break;
            default:
                break;
        }
    }

    public UnityEngine.Audio.AudioMixerGroup GetMixerGroup (AudioMixerGroup mixerGroup)
    {
        switch (mixerGroup)
        {
            case AudioMixerGroup.Master:
                return masterGroup;
            case AudioMixerGroup.Music:
                return musicGroup;
            case AudioMixerGroup.SFX:
                return sfxGroup;
            case AudioMixerGroup.Ambient:
                return ambientGroup;
            case AudioMixerGroup.UI:
                return uiGroup;
            default:
                return masterGroup;
        }
    }
}
