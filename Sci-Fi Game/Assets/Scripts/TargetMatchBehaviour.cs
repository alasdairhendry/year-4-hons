using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMatchBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterTargetMatch ctm = animator.GetComponent<CharacterTargetMatch> ();
        ctm.GetComponent<Collider> ().isTrigger = true;
        ctm.GetComponent<Rigidbody> ().useGravity = false;
        animator.GetComponent<Character> ().isWallClimbing = true;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterTargetMatch ctm = animator.GetComponent<CharacterTargetMatch> ();
        CharacterTargetMatch.MatchData data = ctm.CurrentData ( stateInfo.normalizedTime );
        if (data == null) return;
        animator.MatchTarget ( data.targetPosition, data.targetRotation, data.targetBodyPath, new MatchTargetWeightMask ( data.positionWeight, data.rotationWeight ), data.start, data.end );
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CharacterTargetMatch ctm = animator.GetComponent<CharacterTargetMatch> ();
        ctm.GetComponent<Collider> ().isTrigger = false;
        ctm.GetComponent<Rigidbody> ().useGravity = true;
        animator.GetComponent<Character> ().isWallClimbing = false;        
        //animator.transform.position = ctm.targetPosition;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
