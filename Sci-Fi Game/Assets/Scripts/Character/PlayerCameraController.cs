using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Character character;
    [Space]
    [SerializeField] private Vector3 globalOffset = new Vector3 ();
    [Space]
    [SerializeField] private Vector3 localOffset = new Vector3 ();
    [SerializeField] private Vector3 localEuler = new Vector3 ();
    [Space]    
    [SerializeField] private float followDamp = 5.0f;
    [SerializeField] private float smoothVelocityX;
    [SerializeField] private float smoothVelocityY;
    [SerializeField] private float turnSmooth;
    [SerializeField] private float rotationSpeedX;
    [SerializeField] private float rotationSpeedY;
    [Space]
    [SerializeField] private float xClampMin = -50.0f;
    [SerializeField] private float xClampMax = 50.0f;
    [Space]
    [SerializeField] private float fovChangeDamp = 3.5f;
    [SerializeField] private float targetFOV = 60.0f;
    [SerializeField] private bool DEBUG_LOCK_Y_ROT = false;
    [SerializeField] private bool DEBUG_LOCK_X_ROT = false;
    private new Camera camera;
    private float yLookAngle;
    private float xLookAngle;

    private float yRecoilAngle;
    private float xRecoilAngle;

    private float smoothX;
    private float smoothY;

    private float mouseX;
    private float mouseY;
    private float currentFOV = 60.0f;

    public Transform LookTransform { get { return yRotationRoot; } }

    public Transform yRotationRoot { get; protected set; }
    public Transform xRotationRoot { get; protected set; }
    public Vector3 initialCameraPosition { get; protected set; }
    public Quaternion initialCameraRotation { get; protected set; }
    public Transform cameraTransform { get; protected set; }
    public Transform CinematicComboView { get; protected set; }

    private void Awake ()
    {
        camera = GetComponentInChildren<Camera> ();
        yRotationRoot = transform.GetChild ( 0 );
        xRotationRoot = yRotationRoot.GetChild ( 0 );
        cameraTransform = xRotationRoot.GetChild ( 0 ).GetChild ( 0 );
        initialCameraPosition = cameraTransform.localPosition;
        initialCameraRotation = cameraTransform.localRotation;
    }    

    public PlayerCameraController SetTarget(Transform t)
    {
        this.target = t;
        this.character = t.gameObject.GetComponent<Character> ();
        return this;
    }

    //public void SwitchTarget (Transform target, Vector3 offset)
    //{
    //    //this.target = target;
    //    //this.character = target.GetComponent<Character> ();
    //    //this.globalOffset = offset;
    //    //transform.GetChild ( 0 ).transform.localPosition = offset;
    //}

    private bool shouldRecordMovement = false;

    private void Update ()
    {
        if (CinematicComboView != null)
        {
            // In cinema view
            mouseX = Mathf.Lerp ( mouseX, 0.0f, Time.deltaTime * 10 );
            mouseY = Mathf.Lerp ( mouseY, 0.0f, Time.deltaTime * 10 );
            return;
        }

        if (Mouse.Down ( 0, false ) || !character.cWeapon.isHolstered)
        {
            shouldRecordMovement = true;
        }
        else if (!Mouse.DownRepeating ( 0 ) && character.cWeapon.isHolstered)
        {
            shouldRecordMovement = false;
        }

        if (shouldRecordMovement)
        {
            mouseX = Input.GetAxis ( "Mouse X" );
            mouseY = Input.GetAxis ( "Mouse Y" );
        }
        else
        {
            mouseX = Mathf.Lerp ( mouseX, 0.0f, Time.deltaTime * 10 );
            mouseY = Mathf.Lerp ( mouseY, 0.0f, Time.deltaTime * 10 );
        }
    }

    private void FixedUpdate ()
    {
        if (!target) return;
        
        if (target.gameObject.activeSelf == false) { return; }

        if (character)
        {
            targetFOV = character.IsAiming && character.cWeapon.currentWeaponData.weaponAttackType == WeaponAttackType.Gun ? 40.0f : 60.0f;
            currentFOV = Mathf.Lerp ( currentFOV, targetFOV, Time.deltaTime * fovChangeDamp );
            camera.fieldOfView = currentFOV;
        }

        transform.position = Vector3.Slerp ( transform.position, target.TransformPoint( globalOffset ), followDamp * Time.deltaTime );

        smoothX = Mathf.SmoothDamp ( smoothX, mouseX, ref smoothVelocityX, turnSmooth );
        smoothY = Mathf.SmoothDamp ( smoothY, mouseY, ref smoothVelocityY, turnSmooth );

        if(!DEBUG_LOCK_Y_ROT)
        yLookAngle += (smoothX * rotationSpeedY);
        if(!DEBUG_LOCK_X_ROT)
        xLookAngle -= (smoothY * rotationSpeedX);

        yLookAngle += yRecoilAngle;
        xLookAngle += xRecoilAngle;

        xLookAngle = Mathf.Clamp ( xLookAngle, xClampMin, xClampMax );
        //if (!Input.GetMouseButton ( 0 ))
        //{
        //    yLookAngle = 0.0f;
        //}

            Quaternion yRotation = Quaternion.Euler ( 0.0f, yLookAngle, 0.0f );
            yRotationRoot.localRotation = yRotation;
        

            Quaternion xRotation = Quaternion.Euler ( xLookAngle, 0.0f, 0.0f );
            xRotationRoot.localRotation = xRotation;
    }

    private void LateUpdate ()
    {
        if(CinematicComboView != null)
        {
            // In cinema view
            cameraTransform.position = Vector3.Slerp ( cameraTransform.position, CinematicComboView.position, Time.deltaTime * 25.0f );
            cameraTransform.rotation = Quaternion.Slerp ( cameraTransform.rotation, CinematicComboView.rotation, Time.deltaTime * 25.0f );
        }
        else
        {
            // Out of cinema view
            cameraTransform.localPosition = Vector3.Slerp ( cameraTransform.localPosition, initialCameraPosition, Time.deltaTime * 25.0f );
            cameraTransform.localRotation = Quaternion.Slerp ( cameraTransform.localRotation, initialCameraRotation, Time.deltaTime * 25.0f );
        }
    }

    public void SetXRecoil (float x)
    {
        xRecoilAngle = Time.deltaTime * x;
    }

    public void SetYRecoil (float x)
    {
        yRecoilAngle= Time.deltaTime * x;
    }

    public void SetCinematicComboView(Transform transform)
    {
        CinematicComboView = transform;
    }
}
