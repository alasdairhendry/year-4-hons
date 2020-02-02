using System.Collections.Generic;
using UnityEngine;

public enum MeleeDamageType { Crush, Slash, Stab }

[CreateAssetMenu ( menuName = "Weapons/New Melee Data" )]
public class WeaponMeleeData : WeaponData
{
    [Header ( "Melee Data" )]
    public MeleeDamageType meleeDamageType;

    public AudioClipObject swingSoundData;
    public AudioClipObject hitSoundData;
    public AudioClipObject missSoundData;
    [UnityEngine.Serialization.FormerlySerializedAs( "animationClips" )]
    public List<MeleeAttackAnimation> genericAnimationClips = new List<MeleeAttackAnimation> ();
    public List<MeleeAttackAnimation> specialAnimationClips = new List<MeleeAttackAnimation> ();
    public List<MeleeAttackComboAnimation> comboAnimationClips = new List<MeleeAttackComboAnimation> ();
}
