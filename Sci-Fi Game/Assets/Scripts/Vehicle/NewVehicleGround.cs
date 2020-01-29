using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using static NewVehicleGround;

public class NewVehicleGround : Vehicle
{
    public enum Gear { Reverse, Neutral, First, Second, Third, Fourth, Fifth, Sixth }

    //public Rigidbody rb { get; set; }
    private Dictionary<Gear, VehicleGear> gears = new Dictionary<Gear, VehicleGear> ()
    {
        { Gear.Reverse, new VehicleGear ( Gear.Reverse, 0.200f, 0.0234f, 0.2f, 1.0f ) },
        { Gear.Neutral, new VehicleGear ( Gear.Neutral, 0.100f, 0.0f, 0.0f, 500.0f ) },

        { Gear.First, new VehicleGear   ( Gear.First,   0.200f, 0.0234f, 0.2f, 1.00f ) },
        { Gear.Second, new VehicleGear  ( Gear.Second,  0.185f, 0.0468f, 0.36f, 1.00f ) },

        { Gear.Third, new VehicleGear   ( Gear.Third,   0.170f, 0.0703f, 0.52f, 1.00f ) },
        { Gear.Fourth, new VehicleGear  ( Gear.Fourth,  0.155f, 0.0937f, 0.68f, 1.00f ) },
        { Gear.Fifth, new VehicleGear   ( Gear.Fifth,   0.140f, 0.1171f, 0.84f, 1.00f ) },
        { Gear.Sixth, new VehicleGear   ( Gear.Sixth,   0.125f, 0.1328f, 1.00f, 1.00f ) }
    };

    public Gear CurrentGearIndex = Gear.Neutral;
    public VehicleGear CurrentGear { get { return gears[CurrentGearIndex]; } }

    [NaughtyAttributes.ShowNativeProperty]
    public float ACCELERATOR
    {
        get
        {
            if (!Application.isPlaying) return 0.0f;
            if (!engineIsOn) return 0.0f;

            if (LogitechGSDK.LogiUpdate () && LogitechGSDK.LogiIsConnected ( 0 ))
                return InputWheel.Accelerator;
            else return Mathf.Clamp01 ( Input.GetAxis ( "Vertical" ) );
        }
    }

    [NaughtyAttributes.ShowNativeProperty]
    public float BRAKE
    {
        get
        {
            if (!Application.isPlaying) return 0.0f;
            if (!engineIsOn) return 0.0f;

            if (LogitechGSDK.LogiUpdate () && LogitechGSDK.LogiIsConnected ( 0 ))
                return InputWheel.Brake;
            else return Mathf.Abs ( Mathf.Clamp ( 0.0f, -1.0f, Input.GetAxis ( "Vertical" ) ) );
        }
    }

    [NaughtyAttributes.ShowNativeProperty]
    public float STEERING
    {
        get
        {
            if (!Application.isPlaying) return 0.0f;
            if (!engineIsOn) return 0.0f;

            if (LogitechGSDK.LogiUpdate () && LogitechGSDK.LogiIsConnected ( 0 ))
                return InputWheel.Steering;
            else return Input.GetAxis ( "Horizontal" );
        }
    }

    public float maxMPH = 112.0f;
    public float currentMPH = 0.0f;
    [Space]
    public float engineTorque = 3500.0f;        // The torque applied to the engine / How fast we accelerate / How much force the engine applies to the wheels (THIS IS MODIFIED BY GEARS ETC AND IS THE GENERIC OUTPUT IN NEUTRAL)
    public float currentEngineTorque = 0.0f;    // The current torque BEING applied to the wheels due to gears etc
    [Space]
    public float brakeForce = 3500.0f;
    public float drivingDrag = 0.5f;
    public float airDrag = 0.5f;
    [Space]
    public float maxRevs = 7000.0f;
    public float currentRevs = 0.0f;
    public float targetRevs = 0.0f;
    [Space]
    public bool automaticGearbox = false;
    public float automaticGearboxMultiplier = 0.85f;
    [Space]
    public float maxTurningAngle = 50.0f;
    public float currentTurningAngle = 0.0f;
    [Space]
    public Transform revLineUITransform;
    public Transform speedLineUITransform;
    public TextMeshProUGUI mphText;
    public TextMeshProUGUI gearText;
    public TextMeshProUGUI timeText;
    public AudioSource idleSource;
    public AudioSource ignitionSource;
    public Vector2 pitches = new Vector2 ();
    private Stopwatch sw = new Stopwatch ();
    [Space]
    public List<WheelAxle> wheelAxles = new List<WheelAxle> ();
    public int torqueWheelCount = 0;
    public float GetNormalisedWheelTorque (float input) { return input / ((float)torqueWheelCount); }
    [NaughtyAttributes.ShowNativeProperty] public float WheelTorque { get { if (Application.isPlaying == false) return 0.0f; else return wheelAxles[1].rightCollider.motorTorque; } }
    [NaughtyAttributes.ShowNativeProperty] public float BrakeTorque { get { if (Application.isPlaying == false) return 0.0f; else return wheelAxles[0].rightCollider.brakeTorque; } }
    [NaughtyAttributes.ShowNativeProperty] public float RPM { get { if (Application.isPlaying == false) return 0.0f; else return wheelAxles[1].rightCollider.rpm; } }

    [SerializeField] private TextMeshProUGUI displayText;

    public override void SetToSeatPosition ()
    {
        base.SetToSeatPosition ();

        ignitionSource.Play ();
    }

    public override void Exit (Character character)
    {
        if (currentMPH > 10.0f) return;
        displayText.text = "";
        base.Exit ( character );
    }

    protected override void Awake ()
    {
        base.Awake ();
        GetTorqueWheelCount ();
        StartCoroutine ( CalculateMPH () );
    }

    private void GetTorqueWheelCount ()
    {
        for (int i = 0; i < wheelAxles.Count; i++)
        {
            if (wheelAxles[i].isTorqueWheel)
            {
                torqueWheelCount += 2;
            }
        }
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
            SetUIElements ();
            MonitorZeroToSixty ();
            ApplyWheelTorque ();
            ApplyBrakeTorque ();
            MonitorAutomaticGearbox ();

            displayText.text = currentMPH.ToString ( "00" ) + " mph";

            if (ACCELERATOR == 0.0f)
            {
                base.rigidbody.drag = airDrag;
            }
            else
            {
                base.rigidbody.drag = drivingDrag;
            }

            if (CurrentGearIndex == Gear.Neutral)
            {
                base.rigidbody.drag = airDrag;
            }

            if (currentMPH > maxMPH)
            {
                base.rigidbody.drag = airDrag;
            }
        }
        else
        {
            base.rigidbody.drag = airDrag;
            ApplyBrakeTorque ();
            MonitorRevs ();
            SetEngineAudioPitch ();
        }
    }

    private void MonitorAutomaticGearbox ()
    {
        if (automaticGearbox)
        {
            if (CurrentGearIndex != Gear.Neutral && CurrentGearIndex != Gear.Reverse)
            {
                if (CurrentGearIndex != Gear.Sixth)
                {
                    if (currentMPH > CurrentGear.maxMPHMultipler * maxMPH * automaticGearboxMultiplier)
                    {
                        ShiftUp ();
                    }
                }

                if (CurrentGearIndex != Gear.First)
                {
                    if (currentMPH < gears[(Gear)(int)CurrentGearIndex - 1].maxMPHMultipler * maxMPH * automaticGearboxMultiplier)
                    {
                        ShiftDown ();
                    }
                }
            }
        }

    }

    private void MonitorTurning ()
    {
        currentTurningAngle = Mathf.Lerp ( -maxTurningAngle, maxTurningAngle, Mathf.InverseLerp ( -1.0f, 1.0f, STEERING ) );

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

                    force = GetNormalisedWheelTorque ( brakeForce ) * pedal * Mathf.Lerp ( 0.5f, 1.0f, currentMPH / maxMPH );
                }
                else
                {
                    pedal = 1.0f;
                    force = pedal * GetNormalisedWheelTorque ( brakeForce );
                }

                wheelAxles[i].leftCollider.brakeTorque = force;
                wheelAxles[i].rightCollider.brakeTorque = force;
            }
        }
    }    

    private void LateUpdate ()
    {
        ApplyWheelGraphicsToCollider ();
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

    private void MonitorZeroToSixty ()
    {
        if(timeText == null)
        {
            timeText = GameObject.Find ( "time" )?.GetComponent<TextMeshProUGUI> ();
            return;
        }

        if (ACCELERATOR > 0.0f)
        {
            if (sw.ElapsedMilliseconds <= 0)
            {
                sw.Start ();
            }
        }

        if (sw.IsRunning)
        {
            if(currentMPH >= 60.0f)
            {
                sw.Stop ();
                float f = sw.ElapsedMilliseconds / 1000.0f;
                timeText.text = "0 - 60 in " + f.ToString ( "00.000" ) + " seconds";
            }
            else
            {
                float f = sw.ElapsedMilliseconds / 1000.0f;
                timeText.text = f.ToString ( "00.0" ) + " seconds";
            }
        }
    }

    private void SetEngineAudioPitch ()
    {
        if (engineIsOn)
        {
            idleSource.pitch = Mathf.Lerp ( pitches.x, pitches.y, currentRevs / maxRevs );
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
            targetRevs = Mathf.Lerp ( 0.0f, maxRevs, ACCELERATOR );
        }
        else
        {
            targetRevs = Mathf.Lerp ( CurrentGear.revMinimum, maxRevs, Mathf.InverseLerp ( 0.0f, maxMPH * CurrentGear.maxMPHMultipler, currentMPH ) );
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
            if (currentMPH <= maxMPH * CurrentGear.maxMPHMultipler)
            {
                // TODO : FIND IF NORMALISED FORWARD INPUT IS LESS THAN NORMALISED ENGINE TORQUE TO PROVIDE FINE CONTROL FOR PEDALS
                currentEngineTorque = CurrentGear.maxTorqueMultiplier * engineTorque * Time.deltaTime * BRAKE;
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
            if(currentMPH <= maxMPH * CurrentGear.maxMPHMultipler)
            {
                // TODO : FIND IF NORMALISED FORWARD INPUT IS LESS THAN NORMALISED ENGINE TORQUE TO PROVIDE FINE CONTROL FOR PEDALS
                currentEngineTorque = CurrentGear.maxTorqueMultiplier * engineTorque * Time.deltaTime * ACCELERATOR;
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

    private void SetUIElements ()
    {
        if (gearText)
            gearText.text = CurrentGearIndex.ToString ();
        if (mphText)
            mphText.text = currentMPH.ToString ( "00" ) + " mph";
    }

    private void MonitorGearSwitchingInput ()
    {
        if (automaticGearbox)
        {
            if (CurrentGearIndex == Gear.Reverse || CurrentGearIndex == Gear.Neutral)
            {
                if(ACCELERATOR > 0.0f)
                {
                    if(RPM > 0 || currentMPH < 1.0f)
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
                    if (RPM < 0 || currentMPH < 1.0f)
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

    private IEnumerator CalculateMPH ()
    {
        while (true)
        {
            yield return new WaitForSeconds ( 0.1f );
            currentMPH = GetComponent<Rigidbody> ().velocity.magnitude * 2.237f;
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

    public VehicleGear (Gear gear, float revMinimum, float minMPHMultiplier, float maxMPHMultipler, float maxTorqueMultiplier)
    {
        this.gear = gear;
        //this.revMinimum = revMinimum;
        this.maxTorqueMultiplier = maxTorqueMultiplier;
        this.minMPHMultiplier = minMPHMultiplier;
        this.maxMPHMultipler = maxMPHMultipler;
    }
}
