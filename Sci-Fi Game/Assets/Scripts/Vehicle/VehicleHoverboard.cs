using UnityEngine;

public class VehicleHoverboard : Vehicle
{
    public Hover hover { get; protected set; }

    [SerializeField] private float exitUpwardsForce = 10.0f;
    [SerializeField] private float rotationalSpeed = 10.0f;
    [SerializeField] private float pitchSpeed = 10.0f;
    [Space]
    [SerializeField] private float jumpBoostForce = 10.0f;
    [SerializeField] private float jumpBoostCooldown = 2.5f;
    private float currJumpBoostCooldown = 0.0f;
    [Space]
    [SerializeField] private float defaultRideHeight = 0.75f;
    [SerializeField] private float maxRideHeight = 5.0f;
    [SerializeField] private float timeToRideMax = 2.0f;
    [SerializeField] private float rideHeightDrainRate = 10.0f;
    [SerializeField] private float rideHeightRegenRate = 10.0f;
    [Space]
    [SerializeField] private float antiRollForce = 10.0f;
    [SerializeField] private float maxSqrMagnitude = 220.0f;
    private float currSqrMagnitude { get { return rigidbody.velocity.sqrMagnitude; } }
    public float NormalisedSpeed { get { return Mathf.Lerp ( 0.0f, 1.0f, currSqrMagnitude / maxSqrMagnitude ); } }

    private float currRideHeightEnergy = 1.0f;
    private float currRideHeight = 0.75f;

    [SerializeField] private Vector2 input = new Vector2 ();

    [SerializeField] private bool spinnyRotate = false;

    protected override void Awake ()
    {
        base.Awake ();
        hover = GetComponent<Hover> ();
    }

    public override void Enter (Character character)
    {
        base.Enter ( character );

        hover.hoverDirectionSpace = Space.World;
    }

    public override void Exit (Character character)
    {
        base.Exit ( character );
        character.rigidbody.AddRelativeForce ( Vector3.up * exitUpwardsForce, ForceMode.VelocityChange );
        hover.hoverDirectionSpace = Space.Self;
        hover.hoverHeight = 0.45f;
        input = Vector2.zero;
    }

    public override void OnUpdate ()
    {
        float f = NormalisedSpeed;
        CheckInput ();
        CheckRideHeight ();

        if(currJumpBoostCooldown > 0.0f)
        {
            currJumpBoostCooldown -= Time.deltaTime;
            if (currJumpBoostCooldown < 0.0f) currJumpBoostCooldown = 0.0f;
        }

        if (Input.GetKeyDown ( KeyCode.R ))
        {
            spinnyRotate = !spinnyRotate;
        }
    }

    public override void OnFixedUpdate ()
    {
        Rotation ();
        Power ();
        DoAntiRoll ();

        if (currJumpBoostCooldown <= 0.0f)
        {
            if (Input.GetKeyDown ( KeyCode.Space ))
            {
                currJumpBoostCooldown = jumpBoostCooldown;
                GetComponent<Rigidbody> ().AddForceAtPosition ( transform.up * jumpBoostForce, new Vector3 ( 0.0f, 0.0f, 3.0f ), ForceMode.VelocityChange );
            }
        }
    }

    private void CheckRideHeight ()
    {
        if (Input.GetKey ( KeyCode.LeftShift ) && currRideHeightEnergy > 0.0f)
        {
            currRideHeight = Mathf.SmoothStep ( currRideHeight, maxRideHeight, timeToRideMax * Time.deltaTime );
            currRideHeightEnergy -= rideHeightDrainRate * Time.deltaTime;
        }
        else
        {
            if (!Input.GetKey ( KeyCode.LeftShift ))
            {
                if (currRideHeightEnergy < 1.0f)
                    currRideHeightEnergy += rideHeightRegenRate * Time.deltaTime;

                if (currRideHeightEnergy > 1.0f)
                    currRideHeightEnergy = 1.0f;
            }

            currRideHeight = Mathf.SmoothStep ( currRideHeight, defaultRideHeight, timeToRideMax * Time.deltaTime );
        }

        GetComponent<Hover> ().hoverHeight = currRideHeight;
    }

    private void DoAntiRoll ()
    {
        DoAntiRollWith ( transform.position + (transform.forward * 1.5f), transform.position - (transform.forward * 1.5f) );
        DoAntiRollWith ( transform.position + (transform.right * 1.5f), transform.position - (transform.right * 1.5f) );
    }

    private void DoAntiRollWith(Vector3 front, Vector3 back)
    {        
        float dist = Mathf.Abs ( front.y - back.y );
        Vector3 forcePos = (front.y <= back.y) ? front : back;

        rigidbody.AddForceAtPosition ( transform.up * antiRollForce * dist * NormalisedSpeed, forcePos, ForceMode.Acceleration );
    }

    private void OnGUI ()
    {
        GUI.contentColor = Color.black;        
        GUI.Label ( new Rect ( 32, 32, 128, 128 ), "N Speed: " + (NormalisedSpeed * 100.0f).ToString ("00")  + "%");
        GUI.Label ( new Rect ( 32, 64, 128, 128 ), "Rot Speed: " + ((rotSpeed / rotationalSpeed) * 100.0f).ToString ( "00" ) + "%" );
        //GUI.Label ( new Rect ( 32, 128, 128, 128 ), "Mag: " + rigidbody.velocity.sqrMagnitude );
    }

    private void Power ()
    {
        GetComponent<Rigidbody> ().AddForceAtPosition ( transform.forward * input.magnitude * pitchSpeed, transform.position + transform.up * 1.5f, ForceMode.Acceleration );
    }

    private void CheckInput()
    {
        input = new Vector2 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Vertical" ) ).normalized;
    }
    private float rotSpeed = 1.0f;
    private void Rotation ()
    {
        Vector3 moveDirection = new Vector3 ();

        if (spinnyRotate)
        {
            moveDirection = currentDriver.cCameraController.LookTransform.forward * input.y;

            moveDirection += /*FindObjectOfType<CameraMovement> ().*/transform.right * input.x;
        }
        else
        {
            moveDirection = currentDriver.cCameraController.LookTransform.forward * input.y;
            moveDirection += currentDriver.cCameraController.LookTransform.right * input.x;
        }


        moveDirection.Normalize ();

        //moveDirection += transform.up * -input.y;
        //moveDirection += transform.forward * -input.x;

        if (moveDirection == Vector3.zero) return;

        Quaternion lookDirection = Quaternion.LookRotation ( moveDirection );

         rotSpeed = rotationalSpeed;

        //if(NormalisedSpeed <= 0.25f)
        //{
        //    rotSpeed = rotationalSpeed * 0.5f;
        //}

        rotSpeed = Mathf.Lerp ( rotationalSpeed * 0.4f, rotationalSpeed, NormalisedSpeed * 4.0f );

        //if (input.y > -0.9f && input.y < 0.9f)
        //    rotSpeed *= 0.2f;


        //float rotSpeed = Mathf.Lerp ( 0.2f, rotationalSpeed, Mathf.Abs ( input.y ) / 1.0f );

        Quaternion targetRotation = Quaternion.Slerp ( transform.rotation, lookDirection, rotSpeed * Time.deltaTime );
        transform.rotation = targetRotation;
    }
}
