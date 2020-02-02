using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AudioClipObject : ScriptableObject
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip> ();
    public List<AudioClip> AudioClips { get => audioClips; set => audioClips = value; }

    public AudioClip GetRandom ()
    {
        return audioClips.GetRandom ();
    }
}
