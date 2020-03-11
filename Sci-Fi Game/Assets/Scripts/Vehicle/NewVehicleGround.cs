using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using static NewVehicleGround;

public enum Gear { Reverse, Neutral, First, Second, Third, Fourth, Fifth, Sixth }

public class NewVehicleGround : Vehicle
{
    #region variables
    private Dictionary<Gear, VehicleGear> gears = new Dictionary<Gear, VehicleGear> ()
    {
        { Gear.Reverse, new VehicleGear ( Gear.Reverse, 0.200f, 0.2f, 1.0f ) },
        { Gear.Neutral, new VehicleGear ( Gear.Neutral, 0.100f, 0.0f, 500.0f ) },

        { Gear.First, new VehicleGear   ( Gear.First,   0.200f, 0.2f, 1.00f ) },
        { Gear.Second, new VehicleGear  ( Gear.Second,  0.185f, 0.36f, 1.00f ) },

        { Gear.Third, new VehicleGear   ( Gear.Third,   0.170f, 0.52f, 1.00f ) },
        { Gear.Fourth, new VehicleGear  ( Gear.Fourth,  0.155f, 0.68f, 1.00f ) },
        { Gear.Fifth, new VehicleGear   ( Gear.Fifth,   0.140f, 0.84f, 1.00f ) },
        { Gear.Sixth, new VehicleGear   ( Gear.Sixth,   0.125f, 1.00f, 1.00f ) }
    };
    
    public VehicleGear CurrentGear { get { return gears[CurrentGearIndex]; } }

    [Header ( "Base Vehicle Ground" )]
    [SerializeField] private VehicleGroundData vehicleData;
    public List<WheelAxle> wheelAxles = new List<WheelAxle> ();

    private float currentEngineTorque = 0.0f;    // The current torque BEING applied to the wheels due to gears etc
    private float currentRevs = 0.0f;
    private float targetRevs = 0.0f;
    private float currentTurningAngle = 0.0f;

    public Gear CurrentGearIndex { get; protected set; } = Gear.Neutral;
    public int torqueWheelCount { get; protected set; } = 0;
    public float GetNormalisedWheelTorque (float input) { return input / ((float)torqueWheelCount); }
    public float WheelTorque { get { return wheelAxles[1].rightCollider.motorTorque; } }
    public float BrakeTorque { get { return wheelAxles[0].rightCollider.brakeTorque; } }
    public float RPM { get { return wheelAxles[1].rightCollider.rpm; } }

    public VehicleGroundData VehicleData { get => vehicleData; protected set => vehicleData = value; }
    #endregion

    protected override void Awake ()
    {
        base.Awake ();
        base.CurrentVehicleMode = VehicleMode.Drive;
        GetTorqueWheelCount ();
    }

    private void Update ()
    {
        if (engineIsOn)
        {
            MonitorGearSwitchingInput ();
            MonitorDrivingInput ();
            MonitorRevs ();
            MonitorTurning ();
            SetEngineAudioPitch ();
            ApplyWheelTorque ();
            ApplyBrakeTorque ();
            MonitorAutomaticGearbox ();

            if (ACCELERATOR == 0.0f)
            {
                base.rigidbody.drag = vehicleData.idleDrag;
            }
            else
            {
                base.rigidbody.drag = vehicleData.drivingDrag;
            }

            if (CurrentGearIndex == Gear.Neutral)
            {
                base.rigidbody.drag = vehicleData.idleDrag;
            }

            if (NormalisedSpeed ( vehicleData.maxSqrMagnitude ) >= 1)
            {
                base.rigidbody.drag = vehicleData.idleDrag;
            }
        }
        else
        {
            base.rigidbody.drag = vehicleData.idleDrag;
            ApplyBrakeTorque ();
            MonitorRevs ();
            SetEngineAudioPitch ();
        }
    }

    private void LateUpdate ()
    {
        ApplyWheelGraphicsToCollider ();
    }

    public override void SetToSeatPosition ()
    {
        base.SetToSeatPosition ();

        ignitionSource.Play ();
    }

    public override void Exit (Character character)
    {
        base.Exit ( character );
    }

    private void GetTorqueWheelCount ()
    {
        for (int i = 0; i < wheelAxles.Count; i++)
        {
            Physics.IgnoreCollision ( collider, wheelAxles[i].leftCollider );
            Physics.IgnoreCollision ( collider, wheelAxles[i].rightCollider );

            if (wheelAxles[i].isTorqueWheel)
            {
                torqueWheelCount += 2;
            }
        }
    }

    private void MonitorAutomaticGearbox ()
    {
        if (vehicleData.automaticGearbox)
        {
            if (CurrentGearIndex != Gear.Neutral && CurrentGearIndex != Gear.Reverse)
            {
                if (CurrentGearIndex != Gear.Sixth)
                {
                    if (NormalisedSpeed ( vehicleData.maxSqrMagnitude ) > CurrentGear.maxMPHMultipler)
                    {
                        ShiftUp ();
                    }
                }

                if (CurrentGearIndex != Gear.First && CurrentGearIndex != Gear.Reverse && CurrentGearIndex != Gear.Neutral)
                {
                    if (NormalisedSpeed ( vehicleData.maxSqrMagnitude ) < CurrentGear.maxMPHMultipler * 0.8f)
                    {
                    
                    }

                    if (NormalisedSpeed ( vehicleData.maxSqrMagnitude ) < gears[(Gear)(int)CurrentGearIndex - 1].maxMPHMultipler * 0.8f)
                    {
                        ShiftDown ();
                    }
                }
            }
        }
    }

    private void MonitorTurning ()
    {
        currentTurningAngle = Mathf.Lerp ( -vehicleData.maxTurningAngle, vehicleData.maxTurningAngle, Mathf.InverseLerp ( -1.0f, 1.0f, STEERING ) );

        for (int i = 0; i < wheelAxles.Count; i++)
        {
            if (wheelAxles[i].isTurningWheel)
            {
                wheelAxles[i].leftCollider.steerAngle = currentTurningAngle;
                wheelAxles[i].rightCollider.steerAngle = currentTurningAngle;
            }
        }
    }

    private void ApplyWheelTorque ()
    {
        for (int i = 0; i < wheelAxles.Count; i++)
        {
            if (wheelAxles[i].isTorqueWheel)
            {
                if(CurrentGearIndex == Gear.Reverse)
                {
                    wheelAxles[i].leftCollider.motorTorque = GetNormalisedWheelTorque ( currentEngineTorque ) * -1.0f;
                    wheelAxles[i].rightCollider.motorTorque = GetNormalisedWheelTorque ( currentEngineTorque ) * -1.0f;
                }
                else
                {
                    wheelAxles[i].leftCollider.motorTorque = GetNormalisedWheelTorque ( currentEngineTorque );
                    wheelAxles[i].rightCollider.motorTorque = GetNormalisedWheelTorque ( currentEngineTorque );
                }
            }
        }
    }

    private void ApplyBrakeTorque ()
    {
        float pedal = 1.0f;
        float force = 0.0f;

        for (int i = 0; i < wheelAxles.Count; i++)
        {
            if (wheelAxles[i].isTorqueWheel)
            {
                if (engineIsOn)
                {
                    if (CurrentGearIndex != Gear.Reverse)
                    {
                        pedal = BRAKE;
                    }
                    else
                    {
                        pedal = ACCELERATOR;
                    }

                    force = GetNormalisedWheelTorque ( vehicleData.brakeForce ) * pedal * Mathf.Lerp ( 0.5f, 1.0f, NormalisedSpeed ( vehicleData.maxSqrMagnitude ) );
                }
                else
                {
                    pedal = 1.0f;
                    force = pedal * GetNormalisedWheelTorque ( vehicleData.brakeForce );
                }

                wheelAxles[i].leftCollider.brakeTorque = force;
                wheelAxles[i].rightCollider.brakeTorque = force;
            }
        }
    }    

    private void ApplyWheelGraphicsToCollider ()
    {
        for (int i = 0; i < wheelAxles.Count; i++)
        {
            ApplyWheelPose ( wheelAxles[i].leftCollider, wheelAxles[i].leftTransform, wheelAxles[i].isTurningWheel );
            ApplyWheelPose ( wheelAxles[i].rightCollider, wheelAxles[i].rightTransform, wheelAxles[i].isTurningWheel );
        }
    }

    private void ApplyWheelPose (WheelCollider collider, Transform transform, bool turning)
    {
        if (!transform) return;

        Vector3 position = new Vector3 ();
        Quaternion rotation = new Quaternion ();

        collider.GetWorldPose ( out position, out rotation );

        transform.position = position;
        transform.rotation = rotation;
    }

    private void SetEngineAudioPitch ()
    {
        if (engineIsOn)
        {
            idleSource.pitch = Mathf.Lerp ( pitches.x, pitches.y, currentRevs / vehicleData.maxRevs );
        }
        else
        {
            idleSource.pitch = Mathf.Lerp ( idleSource.pitch, 0.0f, Time.deltaTime * 3.0f );
        }
    }

    private void MonitorRevs ()
    {
        if(CurrentGearIndex == Gear.Neutral)
        {
            targetRevs = Mathf.Lerp ( 0.0f, vehicleData.maxRevs, ACCELERATOR );
        }
        else
        {
            targetRevs = Mathf.Lerp ( CurrentGear.revMinimum, vehicleData.maxRevs, NormalisedSpeed ( vehicleData.maxSqrMagnitude ) * CurrentGear.maxMPHMultipler );
        }

        currentRevs = Mathf.Lerp ( currentRevs, targetRevs, Time.deltaTime * 5.0f );
    }

    private void MonitorDrivingInput ()
    {
        if (CurrentGearIndex == Gear.Neutral)
        {
            MonitorGearNeutral ();
        }
        else if (CurrentGearIndex == Gear.Reverse)
        {
            MonitorGearReverse ();
        }
        else if (CurrentGearIndex != Gear.Neutral && CurrentGearIndex != Gear.Reverse)
        {
            MonitorGearDrive ();
        }
    }

    private void MonitorGearNeutral ()
    {
        currentEngineTorque = 0.0f;
    }

    private void MonitorGearReverse ()
    {
        if (BRAKE > 0)
        {
            if (NormalisedSpeed ( vehicleData.maxSqrMagnitude ) <= CurrentGear.maxMPHMultipler)
            {
                // TODO : FIND IF NORMALISED FORWARD INPUT IS LESS THAN NORMALISED ENGINE TORQUE TO PROVIDE FINE CONTROL FOR PEDALS
                currentEngineTorque = CurrentGear.maxTorqueMultiplier * vehicleData.engineTorque * Time.deltaTime * BRAKE;
                //rb.AddForce ( -transform.forward * currentEngineTorque, ForceMode.Force );
                // TODO : APPLY THE TORQUE TO THE WHEELS
            }
            else
            {
                currentEngineTorque = 0.0f;
                // TODO : --DONT-- APPLY THE TORQUE TO THE WHEELS
            }
        }
        else
        {
            currentEngineTorque = 0.0f;
        }
    }

    private void MonitorGearDrive ()
    {
        if (ACCELERATOR > 0)
        {
            if(NormalisedSpeed ( vehicleData.maxSqrMagnitude ) <= CurrentGear.maxMPHMultipler)
            {
                // TODO : FIND IF NORMALISED FORWARD INPUT IS LESS THAN NORMALISED ENGINE TORQUE TO PROVIDE FINE CONTROL FOR PEDALS
                currentEngineTorque = CurrentGear.maxTorqueMultiplier * vehicleData.engineTorque * Time.deltaTime * ACCELERATOR;
                //rb.AddForce ( transform.forward * currentEngineTorque, ForceMode.Force );
                // TODO : APPLY THE TORQUE TO THE WHEELS
            }
            else
            {
                currentEngineTorque = 0.0f;
                // TODO : --DONT-- APPLY THE TORQUE TO THE WHEELS
            }
        }
        else
        {
            currentEngineTorque = 0.0f;
        }
    }

    private void MonitorGearSwitchingInput ()
    {
        if (vehicleData.automaticGearbox)
        {
            if (CurrentGearIndex == Gear.Reverse || CurrentGearIndex == Gear.Neutral)
            {
                if(ACCELERATOR > 0.0f)
                {
                    if(RPM > 0 || NormalisedSpeed ( vehicleData.maxSqrMagnitude ) < 0.1f)
                    {
                        CurrentGearIndex = Gear.First;
                        OnGearChange ();
                    }
                }
            }

            if (CurrentGearIndex != Gear.Reverse)
            {
                if (BRAKE > 0.0f)
                {
                    if (RPM < 0 || NormalisedSpeed ( vehicleData.maxSqrMagnitude ) < 0.1f)
                    {
                        CurrentGearIndex = Gear.Reverse;
                        OnGearChange ();
                    }
                }
            }
        }
        else
        {
            if (Input.GetKeyDown ( KeyCode.Q ))
            {
                ShiftDown ();
            }

            if (Input.GetKeyDown ( KeyCode.E ))
            {
                ShiftUp ();
            }
        }
        
    }

    private void ShiftDown ()
    {
        if ((int)CurrentGearIndex == 0) return;
        CurrentGearIndex = gears[(Gear)(((int)CurrentGearIndex) - 1)].gear;
        OnGearChange ();
    }

    private void ShiftUp ()
    {
        if ((int)CurrentGearIndex == 7) return;
        CurrentGearIndex = gears[(Gear)(((int)CurrentGearIndex) + 1)].gear;
        OnGearChange ();
    }

    private void OnGearChange ()
    {

    }
}

[System.Serializable]
public class WheelAxle
{
    public WheelCollider leftCollider;
    public WheelCollider rightCollider;

    public Transform leftTransform;
    public Transform rightTransform;

    public bool isTorqueWheel = false;
    public bool isTurningWheel = false;
}

public class VehicleGear
{
    public Gear gear;
    public float revMinimum;
    public float maxTorqueMultiplier;
    public float minMPHMultiplier;
    public float maxMPHMultipler;

    public VehicleGear (Gear gear, float revMinimum, float maxMPHMultipler, float maxTorqueMultiplier)
    {
        this.gear = gear;
        this.revMinimum = revMinimum;
        this.maxTorqueMultiplier = maxTorqueMultiplier;
        this.maxMPHMultipler = maxMPHMultipler;
    }
}
