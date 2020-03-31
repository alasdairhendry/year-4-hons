using UnityEngine;

[RequireComponent ( typeof ( Rigidbody ) )]
[RequireComponent ( typeof ( CharacterMovement ) )]
public class Character : MonoBehaviour
{
    public enum State { Standing, Falling, Driving }
    public State currentState { get; protected set; } = State.Standing;

    [SerializeField] private float standingDrag = 6.0f;
    [SerializeField] private float fallingDrag = 0.0f;

    private bool canAim = true;
    private bool isAiming = false;

    public bool IsAiming
    {
        get
        {
            if (!canAim)
            {
                if (isAiming)
                {
                    isAiming = false;
                    OnAimChanged ();
                }
            }
            else
            {
                if (cInput.isAimInput || cWeapon.ShouldAim)
                {
                    if (!isAiming)
                    {
                        isAiming = true;
                        OnAimChanged ();
                    }
                }
                else if (!cInput.isAimInput && !cWeapon.ShouldAim)
                {
                    if (isAiming)
                    {
                        isAiming = false;
                        OnAimChanged ();
                    }
                }
            }

            return isAiming;
        }
    }

    public bool isCrouching
    {
        get
        {
            if (isCrouchingInput)
            {
                return true;
            }

            if (!isCrouchingInput)
            {
                return true;
            }

            return false;
        }
    }
    public float runPercentNormalised { get; set; } = 1.0f;
    public bool isRunning { get; set; }
    public bool shouldJump { get; set; }

    public bool isCrouchingInput { get; set; }

    public System.Action OnAimChanged;

    public new Rigidbody rigidbody { get; protected set; }
    public CharacterMovement cMovement { get; protected set; }
    public CharacterIK cIK { get; protected set; }
    public CharacterPhysics cPhysics { get; protected set; }
    public CharacterInput cInput { get; protected set; }
    public CharacterWeapon cWeapon { get; protected set; }
    public CharacterGear cGear { get; protected set; }
    public CharacterInteraction cInteraction { get; protected set; }
    public CharacterAnimator cAnimator { get; protected set; }
    public CharacterFaction cFaction { get; protected set; }
    public PlayerCameraController cCameraController { get; protected set; }
    public Health Health { get; protected set; }
    public FloatingTextIndicator FloatingTextIndicator { get; protected set; }
    public Animator Animator { get; protected set; }

    public bool isGrounded { get; set; } = false;

    public new Collider collider;

    [SerializeField] private LayerMask walkableLayers;
    [SerializeField] private float stepHeight = 0.6f;
    [SerializeField] private float standHeight = 0.6f;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private AudioClipObject deathAudioClips;

    public bool IsAI { get; protected set; }
    public Vehicle currentVehicle { get; protected set; } = null;
    public bool CanMove { get; protected set; } = true;
    [NaughtyAttributes.ShowNativeProperty] public bool isDead { get; protected set; }
    private NPC npc;

    private void Awake ()
    {
        if (GetComponent<NPC> () != null) IsAI = true;

        if (!IsAI)
        {
            EntityManager.instance.SetPlayerCharacter ( this );
        }
        else
        {
            npc = GetComponent<NPC> ();
        }

        rigidbody = GetComponent<Rigidbody> ();
        cMovement = GetComponent<CharacterMovement> ();
        cPhysics = GetComponent<CharacterPhysics> ();
        cIK = GetComponent<CharacterIK> ();
        cInput = GetComponent<CharacterInput> ();
        cWeapon = GetComponent<CharacterWeapon> ();
        cAnimator = GetComponent<CharacterAnimator> ();
        cGear = GetComponent<CharacterGear> ();
        cInteraction = GetComponent<CharacterInteraction> ();
        cFaction = GetComponent<CharacterFaction> ();
        Health = GetComponent<Health> ();
        FloatingTextIndicator = GetComponent<FloatingTextIndicator> ();
        Animator = GetComponent<Animator> ();

        if (IsAI) return;

        if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Sylas)
            EntityManager.instance.PlayerCharacter.Health.SetMaxHealth ( SkillModifiers.GetMaxHealth () * 1.35f, true );
        else
            EntityManager.instance.PlayerCharacter.Health.SetMaxHealth ( SkillModifiers.GetMaxHealth (), true );

        Health.onDeath += OnPlayerDeath;
        Health.IsPlayer = true;
        cCameraController = FindObjectOfType<PlayerCameraController> ();
    }

    private void Update ()
    {
        CheckStateUpdate ();
    }

    private void FixedUpdate ()
    {
        CheckStateFixed ();
    }

    private void CheckStateUpdate ()
    {
        switch (currentState)
        {
            case State.Standing:
                cMovement.OnUpdate ();
                isGrounded = CheckIsGrounded ();
                rigidbody.drag = standingDrag;
                break;
            case State.Falling:
                cMovement.OnUpdate ();
                isGrounded = CheckIsGrounded ();
                rigidbody.drag = fallingDrag;
                break;
            case State.Driving:
                if (Input.GetKeyDown ( KeyCode.Escape ))
                {
                    if (currentVehicle != null)
                        currentVehicle.Exit ( this );
                }

                if (currentVehicle != null)
                    if (currentVehicle.engineIsOn)
                        currentVehicle.OnUpdate ();

                rigidbody.drag = fallingDrag;
                break;
            default:
                break;
        }
    }

    private void CheckStateFixed ()
    {
        switch (currentState)
        {
            case State.Standing:
                cMovement.OnFixedUpdate ();
                break;
            case State.Falling:
                cMovement.OnFixedUpdate ();
                break;
            case State.Driving:
                currentVehicle.OnFixedUpdate ();
                break;
            default:
                break;
        }
    }

    public void SetCurrentVehicle (Vehicle v)
    {
        currentVehicle = v;

        if (currentVehicle == null)
        {
            SetCurrentState ( State.Standing );
        }
        else
        {
            SetCurrentState ( State.Driving );
        }
    }

    public void SetCurrentState (State state)
    {
        currentState = state;
        OnStateChanged ();
    }

    private void OnStateChanged ()
    {
        switch (currentState)
        {
            case State.Standing:
                canAim = true;
                break;
            case State.Falling:
                canAim = false;
                break;
            case State.Driving:
                canAim = false;
                if (!cWeapon.isHolstered)
                    cWeapon.SetHolsterState ( true );
                break;
        }
    }

    [SerializeField] private float groundCheckRadialDistance = 0.25f;

    private void OnDrawGizmosSelected ()
    {
        Vector3[] origins = new Vector3[5] { transform.position, transform.position + transform.forward * groundCheckRadialDistance, transform.position + transform.right * groundCheckRadialDistance, transform.position - transform.forward * groundCheckRadialDistance, transform.position - transform.right * groundCheckRadialDistance };
        Vector3 dir = -Vector3.up;
        float dist = stepHeight + 0.2f;
        RaycastHit hit;

        for (int i = 0; i < origins.Length; i++)
        {
            origins[i].y += stepHeight + 0.1f;
            Debug.DrawRay ( origins[i], dir * dist );
        }
    }

    private bool CheckIsGrounded ()
    {
        if (IsAI && npc != null && npc.MeshRenderer.isVisible == false)
        {
            return true;
        }

        Vector3[] origins = new Vector3[5] { transform.position, transform.position + transform.forward * groundCheckRadialDistance, transform.position + transform.right * groundCheckRadialDistance, transform.position - transform.forward * groundCheckRadialDistance, transform.position - transform.right * groundCheckRadialDistance };

        Vector3 dir = -Vector3.up;
        float dist = stepHeight + 0.2f;

        RaycastHit hit;

        for (int i = 0; i < origins.Length; i++)
        {
            origins[i].y += stepHeight + 0.1f;
            Debug.DrawRay ( origins[i], dir * dist );
            if (Physics.Raycast ( origins[i], dir, out hit, dist, walkableLayers, QueryTriggerInteraction.Ignore ))
            {
                Vector3 targetPosition = hit.point;

                transform.position = new Vector3 ( transform.position.x, targetPosition.y, transform.position.z );

                SetCurrentState ( Character.State.Standing );
                GetComponent<Animator> ().SetBool ( "falling", false );
                return true;
            }
        }

        if (Physics.Raycast ( transform.position, dir, out hit, Mathf.Infinity, walkableLayers ))
        {
            float distanceToGround = Vector3.Distance ( transform.position, hit.point );

            if (distanceToGround > 1.0f)
            {
                if (!GetComponent<Animator> ().GetBool ( "falling" ))
                    GetComponent<Animator> ().SetBool ( "falling", true );
            }

            if (distanceToGround < 1.0f)
            {
                if (rigidbody.velocity.magnitude > 7.5f)
                    GetComponent<Animator> ().SetTrigger ( "softLand" );
            }
        }

        SetCurrentState ( Character.State.Falling );
        return false;
    }

    public void SetCanMove (bool canMove)
    {
        CanMove = canMove;
    }

    public void SetIsDead ()
    {
        isDead = true;
    }

    private void OnPlayerDeath ()
    {
        SoundEffectManager.Play3D ( deathAudioClips.GetRandom (), AudioMixerGroup.SFX, this.transform.position, minDistance: 2, maxDistance: 10 );
        SoundEffectManager.Play ( AudioClipAsset.PlayerDeath, AudioMixerGroup.SFX );

        cWeapon.SetHolsterState ( true, true );
        cWeapon.BreakCurrentWeapon ();

        MessageBox.AddMessage ( "Oh dear, you are dead..", MessageBox.Type.Error );

        GetComponent<Animator> ().SetBool ( "die", true );
        CanMove = false;
        isDead = true;

        Invoke ( nameof ( Revive ), 6.0f );
    }

    private void Revive ()
    {
        Health.Revive ();
        transform.position = EntityManager.instance.PlayerRespawnWorldPosition;
        transform.rotation = EntityManager.instance.PlayerRespawnRotation;
        EntityManager.instance.CameraController.SnapToTargetPosition ();

        GetComponent<Animator> ().SetBool ( "die", false );

        isDead = false;
        CanMove = true;
        OnRespawn?.Invoke ();
    }

    public System.Action OnRespawn;
}
