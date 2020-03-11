using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GearData : ScriptableObject
{
    [Header ( "Base Gear Data" )]
    [ItemID] public int itemtemID;
    public bool isBreakable = true;
    [NaughtyAttributes.ShowIf ( "isBreakable" )] [ItemID] public int brokenVariantItemID;
    [Space]
    public GameObject prefab;
    public HumanBodyBones equipBodyPart = HumanBodyBones.RightHand;
    [Space]
    public Vector3 offsetPosition;
    public Vector3 offsetRotation;
    public Vector3 localScale;
}
