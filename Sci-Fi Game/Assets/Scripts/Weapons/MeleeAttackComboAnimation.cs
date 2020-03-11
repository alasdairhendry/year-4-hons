using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Melee Attack Combo Animation")]
public class MeleeAttackComboAnimation : ScriptableObject
{
    public AnimationClip clip;
    public float clipLength;
    public float attackDelay;
    public List<Data> data = new List<Data> ();

    [NaughtyAttributes.Button]
    private void SetClipLength ()
    {
        clipLength = clip.length * 60.0f;
    }

    [System.Serializable]
    public class Data
    {
        public float swingAudioDelay;
        public float resultAudioDelay;
    }
}
