using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Melee Sound Data")]
public class WeaponMeleeSoundData : ScriptableObject
{
    public List<AudioClip> audioClipSwing;
    public List<AudioClip> audioClipHit;
    public List<AudioClip> audioClipMiss;
    public List<AudioClip> audioClipSheathe;
    public List<AudioClip> audioClipUnsheathe;
}
