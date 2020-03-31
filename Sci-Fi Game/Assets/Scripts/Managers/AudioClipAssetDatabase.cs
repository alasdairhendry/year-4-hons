using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClipAsset { UIPanelOpen, UIPanelClose, UIButtonHover, UIButtonClick, BookOpen, BookClose, BookFlipPage, ChestOpen, ChestClose, QuestAccepted, QuestCompleted, QuestLogUpdated, LevelUp, CoinsRattle, InventoryUpdated, WeaponBreak, UseGem, PlayerDeath, Footstep, UIButtonHoverExit }

[CreateAssetMenu]
public class AudioClipAssetDatabase : ScriptableObject
{
    [SerializeField] private List<Asset> assets = new List<Asset> ();
    private Dictionary<AudioClipAsset, Asset> assetsDictionary = new Dictionary<AudioClipAsset, Asset> ();

    public List<Asset> Assets { get => assets; }

    public void CreateDictionary ()
    {
        for (int i = 0; i < assets.Count; i++)
        {
            if (assets[i].assets.Count <= 0)
            {
                Debug.LogError ( "Asset type " + assets[i].assets.GetType () + " with name " + assets[i].assetNameString + " is null" );
                continue;
            }
            assetsDictionary.Add ( assets[i].assetType, assets[i] );
        }
    }

    public AudioClip GetAsset (AudioClipAsset assetName)
    {
        return assetsDictionary[assetName].assets.GetRandom ();
    }

    private void OnEnable ()
    {
        Validate ();
    }

    private void OnValidate ()
    {
        Validate ();
    }

    private void Validate ()
    {
        List<AudioClipAsset> assetNames = Enum.GetNames ( typeof ( AudioClipAsset ) ).Select ( x => (AudioClipAsset)Enum.Parse ( typeof ( AudioClipAsset ), x ) ).ToList ();

        for (int i = 0; i < assetNames.Count; i++)
        {
            if (!assets.Exists ( x => x.assetType == assetNames[i] ))
            {
                assets.Add ( new Asset ( assetNames[i] ) );
            }
        }

        for (int i = 0; i < assets.Count; i++)
        {
            assets[i].assetNameString = assets[i].assetType.ToString ();
        }
    }

    [System.Serializable]
    public class Asset
    {
        public string assetNameString;
        public AudioClipAsset assetType;
        public List<AudioClip> assets = new List<AudioClip> ();
        public bool foldout = false;

        public Asset (AudioClipAsset assetName)
        {
            this.assetType = assetName;
            this.assetNameString = assetName.ToString ();
        }
    }
}
