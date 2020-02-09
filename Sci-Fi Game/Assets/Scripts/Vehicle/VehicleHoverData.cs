using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vehicles/Hover Vehicle Data")]
public class VehicleHoverData : ScriptableObject
{
    public float maxSqrMagnitude = 220.0f;
    public Vector3 cameraOffset = new Vector3 ();
    [Space]
    public float enginePower = 10000.0f;
    public float enginePowerAppreciation = 1000.0f;
    public float enginePowerDepreciation = 1000.0f;
    public float baseHoverHeight = 2.0f;
    public float maxHoverHeight = 20.0f;
    public float brakeForce = 3500.0f;
    [Space]
    public float drivingDrag = 0.0f;
    public float drivingAngularDrag = 5.0f;
    [Space]
    public float idleDrag = 1.0f;
    public float idleAngularDrag = 5.0f;
    [Space]
    public float noDriverDrag = 10.0f;
    public float noDriverAngularDrag = 10.0f;
    [Space]
    public float turnSpeed = 50.0f;
    public float cameraXClampMin = -10;
    public float cameraXClampMax = 10.0f;
}
