using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    public static AssetManager instance;

    [SerializeField] private AudioClipAssetDatabase audioClipManager;
    [SerializeField] private PrefabAssetDatabase prefabManager;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this)
        {
            Destroy ( this.gameObject );
            return;
        }

        DontDestroyOnLoad ( this.gameObject );

        audioClipManager.CreateDictionary ();
        prefabManager.CreateDictionary ();
    }

    public AudioClip GetAudioClip (AudioClipAsset assetName)
    {
        return audioClipManager.GetAsset ( assetName );
    }

    public GameObject GetPrefab(PrefabAsset assetName)
    {
        return prefabManager.GetAsset ( assetName );
    }
}
