using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterOnDriveBehaviour : StateMachineBehaviour
{
    //private Vehicle vehicle;
    //private VehicleTargetMatchData data;

    //public bool isExiting = false;

    //override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    vehicle = animator.GetComponent<Character> ().currentVehicle;

    //    if (!isExiting)
    //    {
    //        Physics.IgnoreCollision ( vehicle.collider, animator.GetComponent<Character>().collider, true );
            
    //        data = vehicle.enterTargetMatchData;
    //        animator.transform.position = vehicle.transform.TransformPoint ( vehicle.driverEnterPosition );
    //        animator.transform.rotation = Quaternion.LookRotation ( vehicle.transform.TransformDirection ( vehicle.driverEnterRotation ) );
    //        animator.GetComponent<Character> ().currentVehicle.rigidbody.velocity = Vector3.zero;
    //        animator.GetComponent<Character> ().currentVehicle.rigidbody.angularVelocity = Vector3.zero;
    //        animator.GetComponent<CharacterPhysics> ().SetKinematic ( true );
    //    }
    //    else
    //    {
    //        data = vehicle.exitTargetMatchData;
    //        vehicle.engineIsOn = false;
    //        animator.GetComponent<CharacterPhysics> ().SetKinematic ( true );
    //    }
    //}

    //public override void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (!isExiting)
    //    {
    //        Vector3 pos = vehicle.transform.TransformPoint ( vehicle.driverMatchTarget );
    //        Quaternion rot = Quaternion.LookRotation ( vehicle.transform.TransformDirection ( vehicle.driverLocalRotation ) );

    //        animator.GetComponent<CharacterIK> ().OpeningCarDoor ( true, vehicle.doorIK );
    //        animator.MatchTarget ( pos, rot, data.target, new MatchTargetWeightMask ( data.positionWeight, data.rotationWeight ), data.start, data.end );
    //    }
    //    else
    //    {
    //        Vector3 pos = vehicle.transform.TransformPoint ( vehicle.driverExitLocalPosition );
    //        Quaternion rot = Quaternion.LookRotation ( vehicle.transform.TransformDirection ( vehicle.driverEnterRotation ) );

    //        animator.GetComponent<CharacterIK> ().OpeningCarDoor ( true, vehicle.doorIK );
    //        animator.MatchTarget ( pos, rot, data.target, new MatchTargetWeightMask ( data.positionWeight, data.rotationWeight ), data.start, data.end );
    //    }
    //}

    //override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if(!isExiting)
    //    {
    //        animator.GetComponent<CharacterIK> ().OpeningCarDoor ( false, vehicle.doorIK );
    //        animator.GetComponent<CharacterPhysics> ().ResetKinematic ();
    //        vehicle.engineIsOn = true;
    //        vehicle.SetToSeatPosition ();
    //    }
    //    else
    //    {
    //        animator.GetComponent<CharacterIK> ().OpeningCarDoor ( false, vehicle.doorIK );
    //        animator.GetComponent<CharacterPhysics> ().ResetKinematic ();
    //        vehicle.SetToExitPosition ();
    //        Physics.IgnoreCollision ( vehicle.collider, animator.GetComponent<Character> ().collider, false );
    //    }
    //}
}
