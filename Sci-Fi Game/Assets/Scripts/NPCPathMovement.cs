using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCPathMovementType { Random, Loop, PingPong }

public class NPCPathMovement : MonoBehaviour
{
    [SerializeField] private List<Vector3> predeterminedPathPositions = new List<Vector3> ();
    [SerializeField] private NPCPathMovementType movementType = NPCPathMovementType.Random;
    [SerializeField] private Vector2 movementDelayRange = new Vector2 ( 2.5f, 5.0f );
    private NPCStateController stateController;
    private NPCNavMesh npcNavMesh = null;

    private int index = 0;
    private bool isReverse = false;

    private void Awake ()
    {
        for (int i = 0; i < predeterminedPathPositions.Count; i++)
        {
            predeterminedPathPositions[i] = transform.TransformPoint ( predeterminedPathPositions[i] );
        }

        stateController = GetComponent<NPCStateController> ();
        npcNavMesh = GetComponent<NPCNavMesh> ();
        npcNavMesh.onPathComplete += OnPathComplete;
        stateController.onStateSwitch += OnStateSwitch;
    }

    private void Start ()
    {
        SetPathDelayed ();
    }

    private void OnStateSwitch (NPCAiState obj)
    {
        if(obj == NPCAiState.Idle)
        {
            SetPathDelayed ();
        }
    }

    private void OnPathComplete ()
    {
        SetPathDelayed ();
    }

    private void SetPathDelayed ()
    {
        if (stateController.NPCAiState != NPCAiState.Idle) return;
        StartCoroutine ( SetPathDelayedIE () );       
    }

    private IEnumerator SetPathDelayedIE ()
    {
        yield return new WaitForSeconds ( UnityEngine.Random.Range ( movementDelayRange.x, movementDelayRange.y ) );
        if (stateController.NPCAiState != NPCAiState.Idle) yield break;

        switch (movementType)
        {
            case NPCPathMovementType.Random:
                npcNavMesh.SetDestination ( predeterminedPathPositions.GetRandom (), false, true );
                break;
            case NPCPathMovementType.Loop:
                index++;
                if (index >= predeterminedPathPositions.Count) index = 0;
                npcNavMesh.SetDestination ( predeterminedPathPositions[index], false, true );
                break;
            case NPCPathMovementType.PingPong:

                if (isReverse)
                {
                    index--;
                    if (index < 0)
                    {
                        isReverse = false;
                        index = 0;
                        SetPathDelayed ();
                    }
                    npcNavMesh.SetDestination ( predeterminedPathPositions[index], false, true );
                }
                else
                {
                    index++;
                    if (index >= predeterminedPathPositions.Count)
                    {
                        isReverse = true;
                        index = predeterminedPathPositions.Count - 1;
                        SetPathDelayed ();
                    }
                    npcNavMesh.SetDestination ( predeterminedPathPositions[index], false, true );
                }

                break;
        }
    }

    private void OnDrawGizmosSelected ()
    {
        switch (movementType)
        {
            case NPCPathMovementType.Random:
                for (int i = 0; i < predeterminedPathPositions.Count; i++)
                {
                    Gizmos.DrawCube ( transform.TransformPoint( predeterminedPathPositions[i]), Vector3.one * 0.25f );

                    for (int x = 0; x < predeterminedPathPositions.Count; x++)
                    {
                        Gizmos.DrawLine ( transform.TransformPoint ( predeterminedPathPositions[i]), transform.TransformPoint ( predeterminedPathPositions[x]) );
                    }
                }
                break;
            case NPCPathMovementType.Loop:
                for (int i = 0; i < predeterminedPathPositions.Count; i++)
                {
                    Gizmos.DrawCube ( transform.TransformPoint ( predeterminedPathPositions[i]), Vector3.one * 0.25f );
                    if(i < predeterminedPathPositions.Count - 1)
                        Gizmos.DrawLine ( transform.TransformPoint ( predeterminedPathPositions[i]), transform.TransformPoint ( predeterminedPathPositions[i+1]) );
                    else
                        Gizmos.DrawLine ( transform.TransformPoint ( predeterminedPathPositions[i]), transform.TransformPoint ( predeterminedPathPositions[0]) );
                }
                break;
            case NPCPathMovementType.PingPong:
                for (int i = 0; i < predeterminedPathPositions.Count; i++)
                {
                    Gizmos.DrawCube ( transform.TransformPoint ( predeterminedPathPositions[i]), Vector3.one * 0.25f );
                    if (i < predeterminedPathPositions.Count - 1)
                        Gizmos.DrawLine ( transform.TransformPoint ( predeterminedPathPositions[i]), transform.TransformPoint ( predeterminedPathPositions[i + 1]) );
                }
                break;
        }



    }
}
