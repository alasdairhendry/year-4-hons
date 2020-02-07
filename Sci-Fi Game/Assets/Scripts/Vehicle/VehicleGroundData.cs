using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vehicles/Ground Vehicle Data")]
public class VehicleGroundData : ScriptableObject
{
    public float maxSqrMagnitude = 220.0f;
    public Vector3 cameraOffset = new Vector3 ();
    public float engineTorque = 3500.0f;        // The torque applied to the engine / How fast we accelerate / How much force the engine applies to the wheels (THIS IS MODIFIED BY GEARS ETC AND IS THE GENERIC OUTPUT IN NEUTRAL)
    public float brakeForce = 3500.0f;
    public float drivingDrag = 0.5f;
    public float drivingAngularDrag = 0.5f;
    public float idleDrag = 0.5f;
    public float idleAngularDrag = 0.5f;
    public float maxRevs = 7000.0f;
    public float maxTurningAngle = 50.0f;
    public bool automaticGearbox = false;
}
