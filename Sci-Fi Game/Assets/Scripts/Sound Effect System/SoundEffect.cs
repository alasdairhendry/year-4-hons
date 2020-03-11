using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour, IPoolable
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private SelfDestruct selfDestruct;

    public AudioSource AudioSource { get => audioSource; protected set => audioSource = value; }

    public void Initialise (AudioClip clip, float volume = 0.5f, bool selfDestruct = true, float delay = 0, bool play = true)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;

        if (selfDestruct)
            this.selfDestruct.Initialise ( (clip.length + delay) * 1.05f, true );

        if (play)
            audioSource.PlayDelayed ( delay );
    }

    public void Set3DProperties (Vector3 position, float minDistance = 1.0f, float maxDistance = 10.0f)
    {
        transform.position = position;
        audioSource.spatialBlend = 1.0f;
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
    }

    void IPoolable.OnInstantiated ()
    {
        audioSource.spatialBlend = 0.0f;
        audioSource.Stop ();
    }
}
