using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHover : Vehicle
{
    [Header ( "Base Vehicle Hover" )]
    [SerializeField] private VehicleHoverData vehicleData;
    [SerializeField] private Hover hover;

    public float currentEngineForce = 0.0f;

    public VehicleHoverData VehicleData { get => vehicleData; protected set => vehicleData = value; }

    protected override void Awake ()
    {
        base.Awake ();
        base.CurrentVehicleMode = VehicleMode.Hover;
    }

    public override void OnUpdate ()
    {
        base.OnUpdate ();

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
            base.rigidbody.drag = vehicleData.idleDrag;
            base.rigidbody.angularDrag = vehicleData.idleAngularDrag;
        }
        else
        {
            base.rigidbody.drag = vehicleData.drivingDrag;
            base.rigidbody.angularDrag = vehicleData.drivingAngularDrag;
        }

        if (NormalisedSpeed ( vehicleData.maxSqrMagnitude ) >= 1)
        {
            base.rigidbody.drag = vehicleData.idleDrag;
        }

        if (Input.GetAxisRaw ( "Vertical" ) > 0)
        {
            if (currentEngineForce < VehicleData.enginePower)
            {
                currentEngineForce += Time.deltaTime * VehicleData.enginePowerAppreciation;

                if (currentEngineForce > VehicleData.enginePower)
                    currentEngineForce = VehicleData.enginePower;
            }
        }
        else
        {
            if (currentEngineForce > 0)
            {
                currentEngineForce -= Time.deltaTime * VehicleData.enginePowerDepreciation;

                if (currentEngineForce < 0)
                    currentEngineForce = 0.0f;
            }
        }
        
        //if (Input.GetAxisRaw("Vertical") > 0)
        //{
        //    if (NormalisedSpeed ( vehicleData.maxSqrMagnitude ) < 1)
        //        currentEngineForce = /*vehicleData.engineForceByNormalisedSpeed.Evaluate ( NormalisedSpeed ( vehicleData.maxSqrMagnitude ) ) **/ vehicleData.enginePower;
        //    else currentEngineForce = 0.0f;
        //}
        //else
        //{
        //    if(currentEngineForce > 0)
        //    {
        //        currentEngineForce -= Time.deltaTime * VehicleData.enginePowerDepreciation;

        //        if (currentEngineForce < 0)
        //            currentEngineForce = 0.0f;
        //    }
        //}

    }

    public override void OnFixedUpdate ()
    {
        if (!engineIsOn) return;

        transform.rotation = Quaternion.Slerp ( transform.rotation, EntityManager.instance.MainCamera.transform.rotation, VehicleData.turnSpeed * Time.deltaTime );

        if (NormalisedSpeed ( VehicleData.maxSqrMagnitude ) < 1)
            rigidbody.AddForce ( transform.forward * currentEngineForce, ForceMode.Force );
    }

    public override void Exit (Character character)
    {
        base.Exit ( character );
        base.rigidbody.drag = vehicleData.noDriverDrag;
        base.rigidbody.angularDrag = vehicleData.noDriverAngularDrag;
        this.rigidbody.useGravity = true;
    }
}
