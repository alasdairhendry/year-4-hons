using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager instance;
    [SerializeField] private GameObject soundEffectPrefab;
    private Camera mainCamera = null;
    private Camera MainCamera
    {
        get
        {
            if (mainCamera == null) mainCamera = Camera.main;
            return mainCamera;
        }
    }

    private List<AudioClip> soundEffectsPlayedThisFrame = new List<AudioClip> ();

    private void Awake ()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }

        DontDestroyOnLoad ( this.gameObject );
        CreatePool ();
    }

    private void LateUpdate ()
    {
        soundEffectsPlayedThisFrame.Clear ();
    }

    private void CreatePool ()
    {
        List<PoolManager.PooledObject> pooledObjects = PoolManager.CreatePool ( soundEffectPrefab, 250, "Sound Effect Object", this.transform );

        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].gameObject.GetComponent<AudioSource> ().SetToRealisticRolloff ();
        }
    }

    public static SoundEffect Play (AudioClip clip, AudioMixerGroup mixerGroup, float volume = 0.5f, bool selfDestruct = true, float delay = 0, bool playOnAwake = true)
    {
        if (instance.soundEffectsPlayedThisFrame.Contains ( clip )) return null;

        GameObject go = PoolManager.Instantiate ( instance.soundEffectPrefab, Vector3.zero, Quaternion.identity );
        SoundEffect soundEffect = go.GetComponent<SoundEffect> ();
        soundEffect.AudioSource.outputAudioMixerGroup = AudioMixerManager.instance.GetMixerGroup ( mixerGroup );
        soundEffect.Initialise ( clip, volume, selfDestruct, delay, playOnAwake );
        instance.soundEffectsPlayedThisFrame.Add ( clip );

        return soundEffect;
    } 

    public static SoundEffect Play3D (AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position, float volume = 0.5f, bool selfDestruct = true, float minDistance = 1.0f, float maxDistance = 10.0f, float delay = 0)
    {
        // If the sound is greater than 75 units from the camera then just forget it
        if ((instance.MainCamera.transform.position - position).sqrMagnitude >= 5625) return null;

        SoundEffect soundEffect = Play ( clip, mixerGroup, volume, selfDestruct, delay, false );
        if (soundEffect == null) return null;
        soundEffect.AudioSource.outputAudioMixerGroup = AudioMixerManager.instance.GetMixerGroup ( mixerGroup );
        soundEffect.Set3DProperties ( position, minDistance, maxDistance );
        soundEffect.AudioSource.Play ();
        return soundEffect;
    }

    public static SoundEffect Play (AudioClipAsset audioClipAsset, AudioMixerGroup mixerGroup, float volume = 0.5f, bool selfDestruct = true, float delay = 0, bool playOnAwake = true)
    {
        return Play ( AssetManager.instance.GetAudioClip ( audioClipAsset ), mixerGroup, volume, selfDestruct, delay, playOnAwake );
    }

    public static SoundEffect Play3D (AudioClipAsset audioClipAsset, AudioMixerGroup mixerGroup, Vector3 position, float volume = 0.5f, bool selfDestruct = true, float minDistance = 1.0f, float maxDistance = 10.0f, float delay = 0)
    {
        return Play3D ( AssetManager.instance.GetAudioClip ( audioClipAsset ), mixerGroup, position, volume, selfDestruct, minDistance, maxDistance, delay );
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
