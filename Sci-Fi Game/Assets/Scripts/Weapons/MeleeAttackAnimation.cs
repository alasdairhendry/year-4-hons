using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Melee Attack Animation")]
public class MeleeAttackAnimation : ScriptableObject
{
    public AnimationClip clip;
    public float clipLength;
    public float attackDelay;
    public float swingAudioDelay;
    public float resultAudioDelay;
}
