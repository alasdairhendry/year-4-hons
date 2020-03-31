using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private float stopMovementWhenDistanceIs = 0.1f;
    [SerializeField] private float followDamp = 5.0f;
    [SerializeField] private float smoothVelocityX;
    [SerializeField] private float smoothVelocityY;
    [SerializeField] private float turnSmooth;
    [Space]
    [SerializeField] private float rotationSensitivityX;
    [SerializeField] private float rotationSensitivityY;
    [Space]
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
    private bool shouldRecordMovement = false;

    public Transform LookTransform { get { return yRotationRoot; } }

    public Transform yRotationRoot { get; protected set; }
    public Transform xRotationRoot { get; protected set; }
    public Vector3 initialCameraPosition { get; protected set; }
    public Quaternion initialCameraRotation { get; protected set; }
    public Transform cameraTransform { get; protected set; }
    public Transform cameraRoot { get; protected set; }
    public Transform CinematicComboView { get; protected set; }

    [Header("Obstruction")]
    [SerializeField] private float cameraObsDistanceDeduction = 0.25f;
    [SerializeField] private float cameraObsForwardDistance = 0.1f;
    [SerializeField] private LayerMask obstructionLayerMask;
    Vector3 obstructionPosition = new Vector3 ();

    private void Awake ()
    {
        camera = GetComponentInChildren<Camera> ();
        yRotationRoot = transform.GetChild ( 0 );
        xRotationRoot = yRotationRoot.GetChild ( 0 );
        cameraRoot = xRotationRoot.GetChild ( 0 );
        cameraTransform = cameraRoot.GetChild ( 0 );
        initialCameraPosition = cameraTransform.localPosition;
        initialCameraRotation = cameraTransform.localRotation;
    }

    private void Update ()
    {
        CheckBackfaceCulling ();
        CheckObstructions ();

        if(cameraIsBackfaced || cameraIsObstructed)
        {
            camera.useOcclusionCulling = false;
        }
        else
        {
            camera.useOcclusionCulling = true;
        }

        if (character.currentVehicle != null && character.currentVehicle.CurrentVehicleMode == VehicleMode.Hover)
        {
            mouseX = Input.GetAxis ( "Mouse X" ) * Time.deltaTime * rotationSensitivityX;
            mouseY = Input.GetAxis ( "Mouse Y" ) * Time.deltaTime * rotationSensitivityY;
        }
        else if (character.currentVehicle != null && character.currentVehicle.CurrentVehicleMode == VehicleMode.Drive)
        {
            mouseX = Mathf.Lerp ( mouseX, 0.0f, Time.deltaTime * 10 );
            mouseY = Mathf.Lerp ( mouseY, 0.0f, Time.deltaTime * 10 );
        }
        else
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
                mouseX = Input.GetAxis ( "Mouse X" ) * Time.deltaTime * rotationSensitivityX;
                mouseY = Input.GetAxis ( "Mouse Y" ) * Time.deltaTime * rotationSensitivityY;
            }
            else
            {
                mouseX = Mathf.Lerp ( mouseX, 0.0f, Time.deltaTime * 10 );
                mouseY = Mathf.Lerp ( mouseY, 0.0f, Time.deltaTime * 10 );
            }
        }
    }

    private void FixedUpdate ()
    {
        SetCameraPositionAndRotation ();
        CheckLocalCameraPosition ();
    }

    private void SetCameraPositionAndRotation ()
    {
        if (!target) return;
        if (target.gameObject.activeSelf == false) { return; }

        if (character.currentVehicle != null)
        {
            if (character.currentVehicle.CurrentVehicleMode == VehicleMode.Hover)
            {
                VehicleHover vehicleHover = character.currentVehicle as VehicleHover;
                VehicleBaseMulti vehicleMulti = character.currentVehicle as VehicleBaseMulti;
                VehicleHoverData vehicleData;
                if (vehicleHover != null)
                {
                    vehicleData = vehicleHover.VehicleData;
                }
                else
                {
                    vehicleData = vehicleMulti.VehicleHoverData;
                }

                transform.position = Vector3.Slerp ( transform.position, character.currentVehicle.transform.TransformPoint ( vehicleData.cameraOffset ), followDamp * 4 * Time.deltaTime );

                smoothX = Mathf.SmoothDamp ( smoothX, mouseX, ref smoothVelocityX, turnSmooth );
                smoothY = Mathf.SmoothDamp ( smoothY, mouseY, ref smoothVelocityY, turnSmooth );

                yLookAngle += (smoothX * rotationSpeedY);
                xLookAngle -= (smoothY * rotationSpeedX);
                xLookAngle = Mathf.Clamp ( xLookAngle, vehicleData.cameraXClampMin, vehicleData.cameraXClampMax );

                Quaternion yRotation = Quaternion.Euler ( 0.0f, yLookAngle, 0.0f );
                yRotationRoot.localRotation = yRotation;

                Quaternion xRotation = Quaternion.Euler ( xLookAngle, 0.0f, 0.0f );
                xRotationRoot.localRotation = xRotation;
            }
            else
            {
                NewVehicleGround vehicleGround = character.currentVehicle as NewVehicleGround;
                VehicleBaseMulti vehicleMulti = character.currentVehicle as VehicleBaseMulti;
                VehicleGroundData vehicleData;
                if (vehicleGround != null)
                {
                    vehicleData = vehicleGround.VehicleData;
                }
                else
                {
                    vehicleData = vehicleMulti.VehicleGroundData;
                }

                transform.rotation = Quaternion.identity;
                transform.position = Vector3.Slerp ( transform.position, character.currentVehicle.transform.TransformPoint ( vehicleData.cameraOffset ), followDamp * 2 * Time.deltaTime );

                yLookAngle = yRotationRoot.localEulerAngles.y;
                xLookAngle = yRotationRoot.localEulerAngles.x;

                Vector3 dir = character.currentVehicle.transform.position - yRotationRoot.transform.position;
                dir.y = 0;
                Quaternion yRotation = Quaternion.LookRotation ( dir );
                yRotationRoot.localRotation = Quaternion.Slerp ( yRotationRoot.localRotation, yRotation, followDamp * 8 * Time.deltaTime );
                Quaternion xRotation = Quaternion.identity;
                xRotationRoot.localRotation = Quaternion.Slerp ( xRotationRoot.localRotation, xRotation, followDamp * 8 * Time.deltaTime );
            }
        }
        else
        {
            targetFOV = 60.0f;

            if (character != null && character.cWeapon != null && character.cWeapon.isEquipped && !character.cWeapon.isHolstered && character.IsAiming && character.cWeapon.currentWeaponData.weaponAttackType == WeaponAttackType.Gun)
                targetFOV = 40.0f;

            currentFOV = Mathf.Lerp ( currentFOV, targetFOV, Time.deltaTime * fovChangeDamp );
            camera.fieldOfView = currentFOV;

            //if (character != null && character.cWeapon != null && character.cWeapon.isEquipped == true && character.cWeapon.isHolstered == false)
            //{
            //    targetFOV = character.IsAiming && character.cWeapon.currentWeaponData.weaponAttackType == WeaponAttackType.Gun ? 40.0f : 60.0f;
            //    currentFOV = Mathf.Lerp ( currentFOV, targetFOV, Time.deltaTime * fovChangeDamp );
            //    camera.fieldOfView = currentFOV;
            //}

            Vector3 localTargetPosition = target.TransformPoint ( globalOffset );

            if (Mathf.Abs ( (transform.position - localTargetPosition).sqrMagnitude ) > stopMovementWhenDistanceIs)
                transform.position = Vector3.Slerp ( transform.position, target.TransformPoint ( globalOffset ), followDamp * Time.deltaTime );

            smoothX = Mathf.SmoothDamp ( smoothX, mouseX, ref smoothVelocityX, turnSmooth * Time.deltaTime);
            smoothY = Mathf.SmoothDamp ( smoothY, mouseY, ref smoothVelocityY, turnSmooth * Time.deltaTime );

            if (!DEBUG_LOCK_Y_ROT)
                yLookAngle += (smoothX * rotationSpeedY);
            if (!DEBUG_LOCK_X_ROT)
                xLookAngle -= (smoothY * rotationSpeedX);

            yLookAngle += yRecoilAngle * Time.deltaTime;
            xLookAngle += xRecoilAngle * Time.deltaTime;

            xLookAngle = Mathf.Clamp ( xLookAngle, xClampMin, xClampMax );

            Quaternion yRotation = Quaternion.Euler ( 0.0f, yLookAngle, 0.0f );
            yRotationRoot.localRotation = yRotation;

            Quaternion xRotation = Quaternion.Euler ( xLookAngle, 0.0f, 0.0f );
            xRotationRoot.localRotation = xRotation;
        }
    }

    private void CheckLocalCameraPosition ()
    {
        if (CinematicComboView != null)
        {
            // In cinema view
            cameraTransform.position = Vector3.Slerp ( cameraTransform.position, CinematicComboView.position, Time.deltaTime * 5.0f );
            cameraTransform.rotation = Quaternion.Slerp ( cameraTransform.rotation, CinematicComboView.rotation, Time.deltaTime * 5.0f );
        }
        else
        {
            // Out of cinema view
            if (obstructionPosition == Vector3.zero)
            {
                cameraTransform.localPosition = Vector3.Slerp ( cameraTransform.localPosition, initialCameraPosition, Time.deltaTime * 25.0f );
                cameraTransform.localRotation = Quaternion.Slerp ( cameraTransform.localRotation, initialCameraRotation, Time.deltaTime * 25.0f );
            }
            else
            {
                cameraTransform.localPosition = Vector3.Slerp ( cameraTransform.localPosition, obstructionPosition, Time.deltaTime * 25.0f );
                cameraTransform.localRotation = Quaternion.Slerp ( cameraTransform.localRotation, initialCameraRotation, Time.deltaTime * 25.0f );
            }
        }
    }

    [Space] [SerializeField] private GameObject hitGo;
    [SerializeField] private float backfaceCullingOcclusionLimit = 0.5f;

    private bool cameraIsBackfaced = false;
    private bool cameraIsObstructed = false;

    private void CheckBackfaceCulling ()
    {
        Ray ray = new Ray ( cameraTransform.position, cameraTransform.forward );
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, backfaceCullingOcclusionLimit ))
        {
            cameraIsBackfaced = true;
        }
        else
        {
            cameraIsBackfaced = false;
        }
    }

    private void CheckObstructions ()
    {
        Ray ray = new Ray ( cameraRoot.transform.position, (EntityManager.instance.PlayerCharacter.Animator.GetBoneTransform ( HumanBodyBones.Head ).transform.position - cameraRoot.transform.position).normalized );
        RaycastHit hit;
        RaycastHit[] hits;

        float dist = (EntityManager.instance.PlayerCharacter.Animator.GetBoneTransform ( HumanBodyBones.Head ).transform.position - cameraRoot.transform.position).magnitude;
        dist -= cameraObsDistanceDeduction;

        hits = Physics.RaycastAll ( ray, dist, obstructionLayerMask, QueryTriggerInteraction.Ignore );
        if (hits.Length > 0)
        {
            hits = hits.OrderBy ( x => x.distance ).ToArray ();
            hit = hits[hits.Length - 1];
            hitGo = hit.collider.gameObject;
            obstructionPosition = cameraRoot.InverseTransformPoint ( hit.point ) + new Vector3 ( 0.0f, 0.0f, cameraObsForwardDistance );
            cameraIsObstructed = true;
        }
        else
        {
            hitGo = null;
            obstructionPosition = Vector3.zero;
            cameraIsObstructed = false;
        }

        Debug.DrawRay ( ray.origin, ray.direction * dist );

        return;
    }

    public void SnapToTargetPosition ()
    {
        transform.position = target.TransformPoint ( globalOffset );
    }

    public void SetCinematicComboView (Transform transform)
    {
        CinematicComboView = transform;
    }

    public PlayerCameraController SetTarget (Transform t)
    {
        this.target = t;
        this.character = t.gameObject.GetComponent<Character> ();
        return this;
    }

    public void SetXRecoil (float x)
    {
        xRecoilAngle = Time.deltaTime * x * SkillModifiers.ShootingRecoilModifier;
    }

    public void SetYRecoil (float x)
    {
        yRecoilAngle = Time.deltaTime * x * SkillModifiers.ShootingRecoilModifier;
    }
}
