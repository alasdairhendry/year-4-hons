using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    [SerializeField] private string vehicleName;
    public Character currentDriver { get; protected set; }
    public new Rigidbody rigidbody { get; protected set; }
    private FixedJoint currentFixedJoint;
    public Vector3 driverEnterPosition = new Vector3 ();
    public Vector3 driverEnterRotation = new Vector3 ();
    public Vector3 driverMatchTarget = new Vector3 ();
    public Vector3 driverLocalPosition = new Vector3 ();
    public Vector3 driverLocalRotation = new Vector3 (0.0f, 0.0f, 1.0f);
    public Vector3 driverExitLocalPosition = new Vector3 ();
    public Vector3 cameraOffset = new Vector3 ();
    public bool engineIsOn = false;
    public Transform doorIK;
    public Collider collider;

    public VehicleTargetMatchData enterTargetMatchData;
    public VehicleTargetMatchData exitTargetMatchData;

    [SerializeField] private float onEnterVehicleMass = 500.0f;
    [SerializeField] private float onExitVehicleMass = 500.0f;
    [SerializeField] private float onEnterDriverMass = 100.0f;
    [SerializeField] private bool disableCollider = false;

    protected virtual void Awake ()
    {
        rigidbody = GetComponent<Rigidbody> ();
    }

    public virtual void EnterPlayer ()
    {
        Character character = EntityManager.instance.PlayerCharacter;

        if (currentDriver) return;
        if (character.currentVehicle) return;

        currentDriver = character;

        character.SetCurrentVehicle ( this );

        character.GetComponent<Animator> ().SetBool ( "driving", true );
        GetComponentInChildren<Animator> ().SetTrigger ( "enter" );

        character.rigidbody.constraints = RigidbodyConstraints.None;
    }

    public virtual void Enter(Character character)
    {
        if (currentDriver) return;
        if (character.currentVehicle) return;

        currentDriver = character;        

        character.SetCurrentVehicle ( this );

        character.GetComponent<Animator> ().SetBool ( "driving", true );
        GetComponentInChildren<Animator> ().SetTrigger ( "enter" );

        character.rigidbody.constraints = RigidbodyConstraints.None;
    }

    public virtual void SetToSeatPosition ()
    {
        if (!currentDriver)
        {
            Debug.LogError ( "No current driver" );
            return;
        }

        currentDriver.rigidbody.useGravity = false;

        currentDriver.transform.position = transform.TransformPoint ( driverLocalPosition );
        currentDriver.transform.rotation = Quaternion.LookRotation ( transform.TransformDirection ( driverLocalRotation ) );
        currentFixedJoint = currentDriver.gameObject.AddComponent<FixedJoint> ();
        currentFixedJoint.connectedBody = GetComponent<Rigidbody> ();

        rigidbody.mass = onEnterVehicleMass;
        currentDriver.cPhysics.SetMass ( onEnterDriverMass );
        currentDriver.GetComponent<Animator> ().applyRootMotion = false;
    }

    public virtual void SetToExitPosition ()
    {
        currentDriver.transform.position = transform.TransformPoint ( driverExitLocalPosition );
        currentDriver.SetCurrentVehicle ( null );
        currentDriver.SetCurrentState ( Character.State.Standing );
        currentDriver.rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        currentDriver = null;
    }

    public virtual void OnUpdate ()
    {

    }

    public virtual void OnFixedUpdate ()
    {

    }

    public virtual void Exit(Character character)
    {
        if (currentFixedJoint)
            Destroy ( currentFixedJoint );

        if (!character) return;

        currentDriver.GetComponent<Animator> ().SetBool ( "driving", false );
        currentDriver.GetComponent<Animator> ().applyRootMotion = true;
        currentDriver.rigidbody.useGravity = true;

        GetComponentInChildren<Animator> ().SetTrigger ( "enter" );

        rigidbody.mass = onExitVehicleMass;
        currentDriver.cPhysics.ResetMass ();

        engineIsOn = false;
    }

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube ( transform.TransformPoint ( driverEnterPosition + (Vector3.up * 0.05f) ), Vector3.one * 0.1f );
        Gizmos.color = Color.blue;
        Gizmos.DrawCube ( transform.TransformPoint ( driverMatchTarget + (Vector3.up * 0.05f) ), Vector3.one * 0.1f );
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube ( transform.TransformPoint ( driverLocalPosition + (Vector3.up * 0.05f) ), Vector3.one * 0.1f );        
        Gizmos.color = Color.red;
        Gizmos.DrawCube ( transform.TransformPoint ( driverExitLocalPosition ), Vector3.one * 0.1f );
    }

    public class VehicleOccupant
    {
        public enum Seat { FrontDriver, FrontPassenger, RearDriver, RearPassenger, RearMiddle }
        public Character character;
        public Seat seat;
    }
}
