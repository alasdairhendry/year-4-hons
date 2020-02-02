using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Sound Data")]
public class WeaponSoundData : ScriptableObject
{
    public List<AudioClip> audioClipFire;
    public List<AudioClip> audioClipSilencedFire;
    public List<AudioClip> audioClipReload;
    public List<AudioClip> audioClipReloadFinished;
    [Space]
    public List<AudioClip> audioClipEmptyFire;
    public AudioClip spinUpDelayLoop;
    public AudioClip spinDownDelayLoop;
}
