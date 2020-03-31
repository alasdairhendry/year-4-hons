using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VehicleMode { Drive, Hover }

[RequireComponent(typeof(Rigidbody))]
public class Vehicle : MonoBehaviour
{
    public VehicleMode CurrentVehicleMode { get; protected set; } = VehicleMode.Drive;

    [Header ( "Base Vehicle" )]
    public Vector3 driverLocalPosition = new Vector3 ();
    public Vector3 driverLocalRotation = new Vector3 (0.0f, 0.0f, 1.0f);
    public Vector3 driverExitLocalPosition = new Vector3 ();
    public AnimationClip idleClip;
    [Space]
    public bool engineIsOn = false;
    public new Collider collider;
    [Space]
    public AudioSource idleSource;
    public AudioSource ignitionSource;
    public Vector2 pitches = new Vector2 ();
    [Space]
    [SerializeField] private float onEnterVehicleMass = 500.0f;
    [SerializeField] private float onExitVehicleMass = 500.0f;
    [SerializeField] private float onEnterDriverMass = 100.0f;
    [Space]
    [SerializeField] private ParticleSystem smokeParticles;
    [SerializeField] private GameObject explosionParticles;

    public Character currentDriver { get; protected set; }
    public new Rigidbody rigidbody { get; protected set; }
    public float CurrSqrMagnitude { get { return rigidbody.velocity.sqrMagnitude; } }
    public float NormalisedSpeed(float maxSqrMagnitude)
    {
        return Mathf.Lerp ( 0.0f, 1.0f, CurrSqrMagnitude / maxSqrMagnitude );
    }

    public float ACCELERATOR
    {
        get
        {
            if (!Application.isPlaying) return 0.0f;
            if (!engineIsOn) return 0.0f;

            return Mathf.Clamp01 ( Input.GetAxis ( "Vertical" ) );
        }
    }
    public float BRAKE
    {
        get
        {
            if (!Application.isPlaying) return 0.0f;
            if (!engineIsOn) return 0.0f;

            return Mathf.Abs ( Mathf.Clamp ( 0.0f, -1.0f, Input.GetAxis ( "Vertical" ) ) );
        }
    }
    public float STEERING
    {
        get
        {
            if (!Application.isPlaying) return 0.0f;
            if (!engineIsOn) return 0.0f;

            return Input.GetAxis ( "Horizontal" );
        }
    }
    public Health health { get; protected set; }

    private FixedJoint currentFixedJoint;

    protected virtual void Awake ()
    {
        rigidbody = GetComponent<Rigidbody> ();
        health = GetComponent<Health> ();
        health.onHealthChanged += OnHealthRemoved;
        health.onDeath += OnDeath;       
    }

    public virtual void EnterPlayer ()
    {
        Enter ( EntityManager.instance.PlayerCharacter );        
    }

    public virtual void ExitPlayer ()
    {
        Exit ( EntityManager.instance.PlayerCharacter );
    }

    public virtual void Enter (Character character)
    {
        if (currentDriver) return;
        if (character.currentVehicle) return;
        character.cAnimator.OverrideDriveIdle ( idleClip );

        currentDriver = character;

        character.SetCurrentVehicle ( this );

        character.GetComponent<Animator> ().SetBool ( "driving", true );
        Physics.IgnoreCollision ( character.collider, collider );
        SetToSeatPosition ();

        character.rigidbody.constraints = RigidbodyConstraints.None;
        engineIsOn = true;
    }

    public virtual void Exit(Character character)
    {
        if (currentFixedJoint)
            Destroy ( currentFixedJoint );

        if (!character) return;

        currentDriver.GetComponent<Animator> ().SetBool ( "driving", false );
        currentDriver.GetComponent<Animator> ().applyRootMotion = true;
        currentDriver.rigidbody.useGravity = true;

        rigidbody.mass = onExitVehicleMass;
        currentDriver.cPhysics.ResetMass ();

        engineIsOn = false;

        SetToExitPosition ();
        Physics.IgnoreCollision ( character.collider, collider, false );
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

    public virtual void OnFixedUpdate () { }

    public void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube ( transform.TransformPoint ( driverLocalPosition + (Vector3.up * 0.05f) ), Vector3.one * 0.1f );        
        Gizmos.color = Color.red;
        Gizmos.DrawCube ( transform.TransformPoint ( driverExitLocalPosition ), Vector3.one * 0.1f );
    }

    private void OnHealthRemoved ()
    {
        if (health == null) return;
        ParticleSystem.EmissionModule mod = smokeParticles.emission;
        mod.rateOverTimeMultiplier = Mathf.Lerp ( 0.0f, 100.0f, Mathf.InverseLerp ( 0.65f, 0.25f, health.healthNormalised ) );
        smokeParticles.Play ();
    }

    private void OnCollisionEnter (Collision collision)
    {
        if (health == null) return;
        if (collision.gameObject.layer != LayerMask.NameToLayer ( "Environment" )) return;

        if(collision.relativeVelocity.sqrMagnitude > 25.0f)
        {
            float damage = 12.5f;

            if(currentDriver != null && currentDriver == EntityManager.instance.PlayerCharacter)
            {
                damage *= SkillModifiers.DrivingDamageReduction;
            }

            health.RemoveHealth ( damage, DamageType.BluntForce );
        }
    }

    private void OnDeath ()
    {
        if (isDead) return;

        if (currentDriver != null)
        {
            Exit ( currentDriver );
        }

        GameObject go = Instantiate ( explosionParticles );
        go.transform.position = transform.position;

        isDead = true;

        Destroy ( this.gameObject );
    }

    private bool isDead = false;

    public class VehicleOccupant
    {
        public enum Seat { FrontDriver, FrontPassenger, RearDriver, RearPassenger, RearMiddle }
        public Character character;
        public Seat seat;
    }
}
