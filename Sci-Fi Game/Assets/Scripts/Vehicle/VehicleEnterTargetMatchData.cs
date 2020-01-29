using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "New Vehicle Target Data" )]
public class VehicleTargetMatchData : ScriptableObject
{
    [Range ( 0.0f, 1.0f )] public float start;
    [Range ( 0.0f, 1.0f )] public float end;

    public Vector3 positionWeight = new Vector3 ();
    [Range ( 0.0f, 1.0f)] public float rotationWeight = 0.0f;

    public AvatarTarget target;
}
