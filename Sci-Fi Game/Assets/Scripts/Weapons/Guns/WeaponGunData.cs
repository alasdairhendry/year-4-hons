using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu ( menuName = "Weapons/New Gun Data" )]
public class WeaponGunData : WeaponData
{
    public enum WeaponType { Pistol, Rifle }
    public enum AmmoType { Bullets, Energy, Plasma }
    public enum FireType { Single, Burst, Auto }

    //public string weaponName;
    [Header("Gun Data")]
    public WeaponType weaponType = WeaponType.Pistol;
    public AmmoType ammoType = AmmoType.Bullets;
    public List<FireType> fireTypes = new List<FireType> ();
    public int attachmentSlots = 1;
    //public HumanBodyBones activeBodyPart = HumanBodyBones.RightHand;
    public TransformData ikData;
    public TransformData inVehicleIkData;
    [Space]
    //public TransformData holsterData;
    //public HumanBodyBones holsterBodyPart = HumanBodyBones.RightHand;
    [Space]
    public float fireRate = 60.0f;  // Rounds per minute
    public float burstDelay = 60.0f;  // Delay Between burst fires. Calculated in Rounds per Minute to keep uniformity in calculations
    public float reloadTime = 1.2f;
    public float clipSize = 30;
    [Space]
    public float spinUpDelaySeconds = 0;
    public float spinDownDelaySeconds = 0;
    [Space]
    public float maxDistance;
    //public float baseDamage;
    public AnimationCurve damageByDistanceFalloff;
    [Space]
    public WeaponGunSoundData weaponSoundData;
    public RecoilData recoilData;
    [Space]
    //public GameObject prefab;
    public GameObject muzzlePrefab;
    //public Vector3 offsetPosition;
    //public Vector3 offsetRotation;
    //public Vector3 localScale;
    public Vector3 clipTextLocalPosition;
    public Vector3 clipTextHolsteredLocalPosition;
}
