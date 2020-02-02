using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{
    [Header ( "Base Weapon Data" )]
    public string weaponName;
    public WeaponAttackType weaponAttackType;
    [Space]
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public Vector3 localScale;
    [Space]
    public GameObject prefab;
    [Space]
    public float baseDamage;
    [Space]
    public HumanBodyBones activeBodyPart = HumanBodyBones.RightHand;
    public HumanBodyBones holsterBodyPart = HumanBodyBones.RightHand;
    public TransformData holsterData;
    [Space]
    public AudioClipObject sheatheSoundData;
    public AudioClipObject unsheatheSoundData;
}
