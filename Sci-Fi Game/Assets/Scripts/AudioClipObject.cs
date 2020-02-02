using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioClipObject : ScriptableObject
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip> ();

    public AudioClip GetRandom ()
    {
        return audioClips.GetRandom ();
    }
}
