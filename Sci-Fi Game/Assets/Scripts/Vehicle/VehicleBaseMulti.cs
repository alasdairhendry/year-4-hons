using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleBaseMulti : Vehicle
{
    #region Ground    
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
    [SerializeField] private VehicleGroundData vehicleGroundData;
    public List<WheelAxle> wheelAxles = new List<WheelAxle> ();

    private float currentEngineDriveForce = 0.0f;    // The current torque BEING applied to the wheels due to gears etc
    private float currentRevs = 0.0f;
    private float targetRevs = 0.0f;
    private float currentTurningAngle = 0.0f;

    public Gear CurrentGearIndex { get; protected set; } = Gear.Neutral;
    public int torqueWheelCount { get; protected set; } = 0;
    public float GetNormalisedWheelTorque (float input) { return input / ((float)torqueWheelCount); }
    public float WheelTorque { get { return wheelAxles[1].rightCollider.motorTorque; } }
    public float BrakeTorque { get { return wheelAxles[0].rightCollider.brakeTorque; } }
    public float RPM { get { return wheelAxles[1].rightCollider.rpm; } }

    public VehicleGroundData VehicleGroundData { get => vehicleGroundData; protected set => vehicleGroundData = value; }    

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

    private void MonitorAutomaticGearbox ()
    {
        if (vehicleGroundData.automaticGearbox)
        {
            if (CurrentGearIndex != Gear.Neutral && CurrentGearIndex != Gear.Reverse)
            {
                if (CurrentGearIndex != Gear.Sixth)
                {
                    if (NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) > CurrentGear.maxMPHMultipler)
                    {
                        ShiftUp ();
                    }
                }

                if (CurrentGearIndex != Gear.First && CurrentGearIndex != Gear.Reverse && CurrentGearIndex != Gear.Neutral)
                {
                    if (NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) < CurrentGear.maxMPHMultipler * 0.8f)
                    {

                    }

                    if (NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) < gears[(Gear)(int)CurrentGearIndex - 1].maxMPHMultipler * 0.8f)
                    {
                        ShiftDown ();
                    }
                }
            }
        }
    }

    private void MonitorTurning ()
    {
        currentTurningAngle = Mathf.Lerp ( -vehicleGroundData.maxTurningAngle, vehicleGroundData.maxTurningAngle, Mathf.InverseLerp ( -1.0f, 1.0f, STEERING ) );

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
                if (CurrentGearIndex == Gear.Reverse)
                {
                    wheelAxles[i].leftCollider.motorTorque = GetNormalisedWheelTorque ( currentEngineDriveForce ) * -1.0f;
                    wheelAxles[i].rightCollider.motorTorque = GetNormalisedWheelTorque ( currentEngineDriveForce ) * -1.0f;
                }
                else
                {
                    wheelAxles[i].leftCollider.motorTorque = GetNormalisedWheelTorque ( currentEngineDriveForce );
                    wheelAxles[i].rightCollider.motorTorque = GetNormalisedWheelTorque ( currentEngineDriveForce );
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

                    force = GetNormalisedWheelTorque ( vehicleGroundData.brakeForce ) * pedal * Mathf.Lerp ( 0.5f, 1.0f, NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) );
                }
                else
                {
                    pedal = 1.0f;
                    force = pedal * GetNormalisedWheelTorque ( vehicleGroundData.brakeForce );
                }

                wheelAxles[i].leftCollider.brakeTorque = force;
                wheelAxles[i].rightCollider.brakeTorque = force;
            }
        }
    }

    private void ApplyWheelGraphicsToCollider ()
    {
        if(applyWheelVisualsDelay > 0)
        {
            applyWheelVisualsDelay -= Time.deltaTime;

            if(applyWheelVisualsDelay <= 0)
            {
                applyWheelVisualsDelay = 0.0f;
            }

            return;
        }

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
            idleSource.pitch = Mathf.Lerp ( pitches.x, pitches.y, currentRevs / vehicleGroundData.maxRevs );
        }
        else
        {
            idleSource.pitch = Mathf.Lerp ( idleSource.pitch, 0.0f, Time.deltaTime * 3.0f );
        }
    }

    private void MonitorRevs ()
    {
        if (CurrentGearIndex == Gear.Neutral)
        {
            targetRevs = Mathf.Lerp ( 0.0f, vehicleGroundData.maxRevs, ACCELERATOR );
        }
        else
        {
            targetRevs = Mathf.Lerp ( CurrentGear.revMinimum, vehicleGroundData.maxRevs, NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) * CurrentGear.maxMPHMultipler );
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
        currentEngineDriveForce = 0.0f;
    }

    private void MonitorGearReverse ()
    {
        if (BRAKE > 0)
        {
            if (NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) <= CurrentGear.maxMPHMultipler)
            {
                // TODO : FIND IF NORMALISED FORWARD INPUT IS LESS THAN NORMALISED ENGINE TORQUE TO PROVIDE FINE CONTROL FOR PEDALS
                currentEngineDriveForce = CurrentGear.maxTorqueMultiplier * vehicleGroundData.engineTorque * Time.deltaTime * BRAKE;
                //rb.AddForce ( -transform.forward * currentEngineTorque, ForceMode.Force );
                // TODO : APPLY THE TORQUE TO THE WHEELS
            }
            else
            {
                currentEngineDriveForce = 0.0f;
                // TODO : --DONT-- APPLY THE TORQUE TO THE WHEELS
            }
        }
        else
        {
            currentEngineDriveForce = 0.0f;
        }
    }

    private void MonitorGearDrive ()
    {
        if (ACCELERATOR > 0)
        {
            if (NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) <= CurrentGear.maxMPHMultipler)
            {
                // TODO : FIND IF NORMALISED FORWARD INPUT IS LESS THAN NORMALISED ENGINE TORQUE TO PROVIDE FINE CONTROL FOR PEDALS
                currentEngineDriveForce = CurrentGear.maxTorqueMultiplier * vehicleGroundData.engineTorque * Time.deltaTime * ACCELERATOR;
                //rb.AddForce ( transform.forward * currentEngineTorque, ForceMode.Force );
                // TODO : APPLY THE TORQUE TO THE WHEELS
            }
            else
            {
                currentEngineDriveForce = 0.0f;
                // TODO : --DONT-- APPLY THE TORQUE TO THE WHEELS
            }
        }
        else
        {
            currentEngineDriveForce = 0.0f;
        }
    }

    private void MonitorGearSwitchingInput ()
    {
        if (vehicleGroundData.automaticGearbox)
        {
            if (CurrentGearIndex == Gear.Reverse || CurrentGearIndex == Gear.Neutral)
            {
                if (ACCELERATOR > 0.0f)
                {
                    if (RPM > 0 || NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) < 0.1f)
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
                    if (RPM < 0 || NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) < 0.1f)
                    {
                        CurrentGearIndex = Gear.Reverse;
                        OnGearChange ();
                    }
                }
            }
        }
        else
        {
            //if (Input.GetKeyDown ( KeyCode.Q ))
            //{
            //    ShiftDown ();
            //}

            //if (Input.GetKeyDown ( KeyCode.E ))
            //{
            //    ShiftUp ();
            //}
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
    #endregion

    #region Hover

    [Header ( "Base Vehicle Hover" )]
    [SerializeField] private VehicleHoverData vehicleHoverData;
    [SerializeField] private Hover hover;
    [SerializeField] private bool isHovering = false;
    [SerializeField] private Animator wheelsAnimator;

    float currentEngineHoverForce = 0.0f;

    public VehicleHoverData VehicleHoverData { get => vehicleHoverData; protected set => vehicleHoverData = value; }
    #endregion

    [SerializeField] private List<ParticleSystem> flameParticles = new List<ParticleSystem> ();
    private float applyWheelVisualsDelay = 0.0f;

    protected override void Awake ()
    {
        base.Awake ();
        SetVehicleState ( isHovering );
        GetTorqueWheelCount ();
    }

    private void Update ()
    {
        if (CurrentVehicleMode == VehicleMode.Drive)
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
                    base.rigidbody.drag = vehicleGroundData.idleDrag;
                }
                else
                {
                    base.rigidbody.drag = vehicleGroundData.drivingDrag;
                    base.rigidbody.angularDrag = vehicleGroundData.drivingAngularDrag;
                }

                if (CurrentGearIndex == Gear.Neutral)
                {
                    base.rigidbody.drag = vehicleGroundData.idleDrag;
                    base.rigidbody.angularDrag = vehicleGroundData.idleAngularDrag;
                }

                if (NormalisedSpeed ( vehicleGroundData.maxSqrMagnitude ) >= 1)
                {
                    base.rigidbody.drag = vehicleGroundData.idleDrag;
                }
            }
            else
            {
                base.rigidbody.drag = vehicleGroundData.idleDrag;
                ApplyBrakeTorque ();
                MonitorRevs ();
                SetEngineAudioPitch ();
            }
        }
    }

    private void LateUpdate ()
    {
        if (CurrentVehicleMode == VehicleMode.Drive)
        {
            ApplyWheelGraphicsToCollider ();
        }
    }

    public override void OnUpdate ()
    {
        base.OnUpdate ();

        if (Input.GetKeyDown ( KeyCode.Space ))
        {
            SetVehicleState ( !isHovering );
        }

        if (Input.GetAxisRaw ( "Vertical" ) > 0)
        {
            if (currentEngineHoverForce < VehicleHoverData.enginePower)
            {
                currentEngineHoverForce += Time.deltaTime * VehicleHoverData.enginePowerAppreciation;

                if (currentEngineHoverForce > VehicleHoverData.enginePower)
                    currentEngineHoverForce = VehicleHoverData.enginePower;
            }
        }
        else
        {
            if (currentEngineHoverForce > 0)
            {
                currentEngineHoverForce -= Time.deltaTime * VehicleHoverData.enginePowerDepreciation;

                if (currentEngineHoverForce < 0)
                    currentEngineHoverForce = 0.0f;
            }
        }

        if (CurrentVehicleMode == VehicleMode.Hover)
        {
            if (hover.DetectedHoverableSurface)
            {
                rigidbody.useGravity = true;
            }
            else
            {
                rigidbody.useGravity = false;
            }

            if (ACCELERATOR == 0.0f)
            {
                base.rigidbody.drag = vehicleHoverData.idleDrag;
                base.rigidbody.angularDrag = vehicleHoverData.idleAngularDrag;
            }
            else
            {
                base.rigidbody.drag = vehicleHoverData.drivingDrag;
                base.rigidbody.angularDrag = vehicleHoverData.drivingAngularDrag;
            }

            if (NormalisedSpeed ( vehicleHoverData.maxSqrMagnitude ) >= 1)
            {
                base.rigidbody.drag = vehicleHoverData.idleDrag;
            }
        }
    }

    public override void OnFixedUpdate ()
    {
        if (!engineIsOn) return;

        if (CurrentVehicleMode == VehicleMode.Hover)
        {
            transform.rotation = Quaternion.Slerp ( transform.rotation, EntityManager.instance.MainCamera.transform.rotation, VehicleHoverData.turnSpeed * Time.deltaTime );

            if (NormalisedSpeed ( VehicleHoverData.maxSqrMagnitude ) < 1)
                rigidbody.AddForce ( transform.forward * currentEngineHoverForce, ForceMode.Force );
        }
    }

    public override void SetToSeatPosition ()
    {
        base.SetToSeatPosition ();

        ignitionSource.Play ();
    }

    public override void Exit (Character character)
    {
        base.Exit ( character );

        if(CurrentVehicleMode == VehicleMode.Hover)
        {
            base.rigidbody.drag = vehicleHoverData.noDriverDrag;
            base.rigidbody.angularDrag = vehicleHoverData.noDriverAngularDrag;
            this.rigidbody.useGravity = true;
        }
    }

    private void SetVehicleState(bool isHovering)
    {
        if (this.isHovering && !hover.DetectedHoverSurfaceDoubleDistance)
        {
            return;
        }

        this.isHovering = isHovering;

        if (this.isHovering)
        {
            for (int i = 0; i < wheelAxles.Count; i++)
            {
                wheelAxles[i].leftCollider.motorTorque = 0.0f;
                wheelAxles[i].rightCollider.motorTorque = 0.0f;
                wheelAxles[i].leftCollider.enabled = false;
                wheelAxles[i].rightCollider.enabled = false;
            }

            for (int i = 0; i < flameParticles.Count; i++)
            {
                flameParticles[i].Play ();
            }

            wheelsAnimator.SetBool ( "drive", false );
            hover.SetIsHovering ( true );
            base.CurrentVehicleMode = VehicleMode.Hover;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotationX;
        }
        else
        {
            for (int i = 0; i < wheelAxles.Count; i++)
            {
                wheelAxles[i].leftCollider.motorTorque = 0.0f;
                wheelAxles[i].rightCollider.motorTorque = 0.0f;
                wheelAxles[i].leftCollider.enabled = true;
                wheelAxles[i].rightCollider.enabled = true;
            }

            for (int i = 0; i < flameParticles.Count; i++)
            {
                flameParticles[i].Stop ();
            }

            applyWheelVisualsDelay = 1.5f;
            wheelsAnimator.SetBool ( "drive", true );
            hover.SetIsHovering ( false );
            base.CurrentVehicleMode = VehicleMode.Drive;
            transform.rotation = Quaternion.Euler ( 0.0f, transform.eulerAngles.y, 0.0f );
            rigidbody.constraints = RigidbodyConstraints.None;
            rigidbody.useGravity = true;
        }
    }


}
