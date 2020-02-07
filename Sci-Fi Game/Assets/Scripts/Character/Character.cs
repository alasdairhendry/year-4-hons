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
                else if(!cInput.isAimInput && !cWeapon.ShouldAim)
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

            if (!isCrouchingInput && !canStand)
            {
                return true;
            }

            return false;
        }
    }
    public bool isRunning { get; set; }
    public bool shouldJump { get; set; }

    public bool canStand { get; set; }
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
   [NaughtyAttributes.ShowNativeProperty]  public bool isDead { get; protected set; }

    private void Awake ()
    {
        rigidbody = GetComponent<Rigidbody> ();
        cMovement = GetComponent<CharacterMovement> ();
        cPhysics = GetComponent<CharacterPhysics> ();
        cIK = GetComponent<CharacterIK> ();
        cInput = GetComponent<CharacterInput> ();
        cWeapon = GetComponent<CharacterWeapon> ();
        cGear = GetComponent<CharacterGear> ();
        cInteraction = GetComponent<CharacterInteraction> ();
        Health = GetComponent<Health> ();
        FloatingTextIndicator = GetComponent<FloatingTextIndicator> ();
        Animator = GetComponent<Animator> ();

        if (GetComponent<NPC> () != null) IsAI = true;

        if (IsAI) return;

        Health.onDeath += OnPlayerDeath;

        if (FindObjectOfType<PlayerCameraController> () == null)
            cCameraController = Instantiate ( cameraPrefab ).GetComponent<PlayerCameraController> ().SetTarget ( this.transform );
        else cCameraController = FindObjectOfType<PlayerCameraController> ().SetTarget ( this.transform );

        EntityManager.instance.SetPlayerCharacter ( this );
    }

    private void Update ()
    {
        CheckStateUpdate ();

        if (IsAI) return;

        //if (IsAiming)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}
        //else
        //{
        //    if (Input.GetKeyDown ( KeyCode.L ))
        //    {
        //        if (Cursor.lockState == CursorLockMode.Locked)
        //        {
        //            Cursor.lockState = CursorLockMode.None;
        //            Cursor.visible = true;
        //        }
        //        else
        //        {
        //            Cursor.lockState = CursorLockMode.Locked;
        //            Cursor.visible = false;
        //        }
        //    }
        //}
        

        //if (Input.GetKeyDown ( KeyCode.E ))
        //{
        //    if (FindObjectOfType<Vehicle> ())
        //    {
        //        SetCurrentState ( State.Driving );
        //        FindObjectOfType<Vehicle> ().Enter ( this );
        //    }

        //}
        //if (Input.GetKeyDown ( KeyCode.F ))
        //{
        //    if (currentVehicle != null)
        //    {
        //        //currentState = State.Standing;
        //        FindObjectOfType<Vehicle> ().Exit ( this );
        //    }
        //}

        if (isCrouching)
        {
            GetComponent<CapsuleCollider> ().center = new Vector3 ( 0.0f, 0.75f, 0.0f );
            GetComponent<CapsuleCollider> ().height = 1.0f;
        }
        else
        {
            GetComponent<CapsuleCollider> ().center = new Vector3 ( 0.0f, 1.1f, 0.0f );
            GetComponent<CapsuleCollider> ().height = 1.4f;
        }
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
                CheckCanStand ();
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
            //case State.Crouching:
            //    break;
            //case State.Aiming:
            //    break;
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

        if(currentVehicle == null)
        {
            SetCurrentState ( State.Standing );
        }
        else
        {
            SetCurrentState ( State.Driving );
        }
    }

    //private void CheckInput ()
    //{
    //    input = new Vector2 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Vertical" ) ).normalized;
    //    rawInput = new Vector2 ( Input.GetAxisRaw ( "Horizontal" ), Input.GetAxisRaw ( "Vertical" ) ).normalized;
    //}    

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

    private bool CheckIsGrounded ()
    {
        Vector3 origin = transform.position;
        Vector3[] origins = new Vector3[5] { transform.position, transform.position + transform.forward * 0.25f, transform.position + transform.right * 0.25f, transform.position - transform.forward * 0.25f, transform.position - transform.right * 0.25f };
        origin.y += stepHeight;

        Vector3 dir = -Vector3.up;
        float dist = stepHeight + 0.1f;

        RaycastHit hit;

        for (int i = 0; i < origins.Length; i++)
        {
            origins[i].y += stepHeight;
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

    private bool CheckCanStand ()
    {
        Vector3 origin = transform.position;

        Vector2 inputDir = cInput.rawInput;

        //origin += transform.forward * inputDir.y;
        //origin += transform.right * inputDir.x;

        if (canStand)
        {

        }
        else
        {
            //origin -= transform.forward * inputDir.y;
            //origin -= transform.right * inputDir.x;
        }

        origin.y += 0.5f;

        Vector3 dir = Vector3.up;

        Ray ray = new Ray ( origin, dir );
        RaycastHit hit;

        if (Physics.Raycast ( ray, out hit, standHeight - 0.5f, walkableLayers ))
        {
            if (canStand)
            {
                //isCrouching = true;
            }

            if (hit.distance > 1.5f - 0.5f)
                canStand = false;
            else
            {
                canStand = true;
            }
        }
        else
        {
            if (!canStand)
            {
                //isCrouching = false;
                //Debug.Log ( "hello" );
            }

            canStand = true;
        }

        return canStand;
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
        SoundEffect.Play3D ( deathAudioClips.GetRandom (), this.transform.position, 2, 10 );

        if (Random.value > 0.65f)
            cWeapon.Unequip ( true );

        GetComponent<Animator> ().SetTrigger ( "die" );
        CanMove = false;
        isDead = true;
    }
}
