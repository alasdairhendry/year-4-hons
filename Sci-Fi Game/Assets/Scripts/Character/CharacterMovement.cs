using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour
{
    public Character character { get; protected set; }

    [SerializeField] private CharacterMovementData movementData;

    private Vector3 lastMousePosition = new Vector3 ();

    public float GetCurrentSpeed
    {
        get
        {
            return 0.0f;
            if (character.currentState == Character.State.Standing)
                return character.isRunning ? movementData.walkSpeed * movementData.runModifier : movementData.walkSpeed;

            return 0.0f;
        }
    }

    private void Awake ()
    {
        character = GetComponent<Character> ();
        lastMousePosition = Input.mousePosition;
    }

    public void OnUpdate ()
    {
        if (character.IsAI) return;
    }

    public void OnFixedUpdate ()
    {
        if (character.IsAI) return;
        Rotation ();
    }

    private void Rotation ()
    {
        if (character.IsAI) return;

        Vector3 moveDirection = new Vector3 ( 0.0f, 0.0f, 0.0f );

        if (character.IsAiming)
            moveDirection += character.cCameraController.LookTransform.forward;
        else
            moveDirection += character.cCameraController.LookTransform.forward * character.cInput.rawInput.y;

        if (!character.IsAiming)
            moveDirection += character.cCameraController.LookTransform.right * character.cInput.rawInput.x;

        moveDirection.Normalize ();

        if (moveDirection == Vector3.zero) return;

        Quaternion lookDirection = Quaternion.LookRotation ( moveDirection );
        Quaternion targetRotation = Quaternion.Slerp ( transform.rotation, lookDirection, movementData.turnSpeed * Time.deltaTime );
        transform.rotation = targetRotation;
    }

    public void SnapCharacterRotationToCamera ()
    {
        Vector3 moveDirection = new Vector3 ( 0.0f, 0.0f, 0.0f );
        moveDirection = character.cCameraController.LookTransform.forward;
        moveDirection.Normalize ();

        Quaternion lookDirection = Quaternion.LookRotation ( moveDirection );
        transform.rotation = lookDirection;
    }
}

public static class Extensions
{
    public static float SquaredDistance(Vector3 from, Vector3 to)
    {
        return (from - to).sqrMagnitude;
    }

    public static bool IsEpsilon(this Vector3 value, Vector3 target, float epsilon )
    {
        if(Vector3.SqrMagnitude(value - target) < epsilon)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static float Map (this float value, float inputFrom, float inputTo, float outputFrom, float outputTo)
    {
        return (value - inputFrom) / (inputTo - inputFrom) * (outputTo - outputFrom) + outputFrom;
    }

    public static float Map (this int value, int inputFrom, int inputTo, float outputFrom, float outputTo)
    {
        return ((float)value - (float)inputFrom) / ((float)inputTo - (float)inputFrom) * (outputTo - outputFrom) + outputFrom;
    }

}
