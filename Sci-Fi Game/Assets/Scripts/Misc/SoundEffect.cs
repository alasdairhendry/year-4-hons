using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEffect
{
    public static AudioSource Play (AudioClip clip, bool selfDestruct = true)
    {
        GameObject go = new GameObject ( "Audio Source - 2D" );
        AudioSource src = go.AddComponent<AudioSource> ();

        if (selfDestruct)
            go.AddComponent<SelfDestruct> ().Initialise ( clip.length, true );

        src.clip = clip;
        src.Play ();

        return src;
    }

    public static AudioSource Play3D (AudioClip clip, Vector3 position, float minDistance = 10.0f, float maxDistance = 100.0f, bool selfDestruct = true, float delay = 0)
    {
        GameObject go = new GameObject ("Audio Source - 3D");
        go.transform.position = position;
        AudioSource src = go.AddComponent<AudioSource> ();

        if (selfDestruct)
            go.AddComponent<SelfDestruct> ().Initialise ( clip.length + delay, true );

        src.spatialBlend = 1;
        src.minDistance = minDistance;
        src.maxDistance = maxDistance;

        src.clip = clip;

        if (delay > 0)
            src.PlayDelayed ( delay );
        else
            src.Play ();


        return src;
    }

    public static AudioSource Create (Object from, float spatialBlend, float minDistance = 10.0f, float maxDistance = 100.0f, Transform parent= null)
    {
        GameObject go = new GameObject ( "Audio Source - 3D [" + from.GetType ().Name + "]" );
        AudioSource src = go.AddComponent<AudioSource> ();

        src.spatialBlend = spatialBlend;
        src.minDistance = minDistance;
        src.maxDistance = maxDistance;

        if (parent != null)
        {
            go.transform.SetParent ( parent );
            go.transform.localPosition = Vector3.zero;
        }

        return src;
    }

}
