using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrefabAsset { LootableNPC }

[CreateAssetMenu]
public class PrefabAssetDatabase : ScriptableObject
{
    [SerializeField] private List<Asset> assets = new List<Asset> ();
    private Dictionary<PrefabAsset, Asset> assetsDictionary = new Dictionary<PrefabAsset, Asset> ();

    public List<Asset> Assets { get => assets; }

    public void CreateDictionary ()
    {
        for (int i = 0; i < assets.Count; i++)
        {
            if (assets[i].asset == null)
            {
                Debug.LogError ( "Asset type " + assets[i].asset.GetType () + " with name " + assets[i].assetNameString + " is null" );
                continue;
            }
            assetsDictionary.Add ( assets[i].assetType, assets[i] );
        }
    }

    public GameObject GetAsset (PrefabAsset assetName)
    {
        return assetsDictionary[assetName].asset;
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
        List<PrefabAsset> assetNames = Enum.GetNames ( typeof ( PrefabAsset ) ).Select ( x => (PrefabAsset)Enum.Parse ( typeof ( PrefabAsset ), x ) ).ToList ();

        for (int i = 0; i < assetNames.Count; i++)
        {
            if (!assets.Exists ( x => x.assetType == assetNames[i] ))
            {
                assets.Add ( new Asset ( assetNames[i], null ) );
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
        public PrefabAsset assetType;
        public GameObject asset;

        public Asset (PrefabAsset assetName, GameObject asset)
        {
            this.assetType = assetName;
            this.assetNameString = assetName.ToString ();
            this.asset = asset;
        }
    }
}
