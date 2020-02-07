using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavMesh : MonoBehaviour
{
    [SerializeField] private LayerMask terrianLayerMask;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform pathSeeker;
    [SerializeField] private NPC npc;

    public NavMeshPath currentPath { get; protected set; }
    public int currentPathCornerIndex { get; protected set; } = 0;

    private Character character;
    private int walkableAreaMask = 0;

    NavMeshHit navHit;

    public NavMeshPathStatus status;

    private void Awake ()
    {
        character = GetComponent<Character> ();
        currentPath = new NavMeshPath ();
    }

    private void Start ()
    {
        walkableAreaMask = 1 << NavMesh.GetAreaFromName ( "Walkable" );
    }

    private void Update ()
    {
        MoveAlongPath ();

        if (currentPath == null) status = NavMeshPathStatus.PathInvalid;
        if (currentPath.corners.Length <= 0) status = NavMeshPathStatus.PathInvalid;
        status = currentPath.status;
    }

    private void LateUpdate ()
    {
        if (currentPath.corners.Length > 0)
            pathSeeker.transform.position = currentPath.corners[currentPathCornerIndex];
    }

    private void MoveAlongPath ()
    {
        if(currentPath.corners.Length > 0)
        {
            if (character.CanMove == false) return;

            character.cInput.SetInput ( 0.0f, 1.0f );

            Vector3 dir = (pathSeeker.position - transform.position);
            dir.y = 0;

            if(dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.LookRotation ( dir ), Time.deltaTime * 5.0f );

            if(Vector3.Distance(transform.position, pathSeeker.position) < 0.25f)
            {
                currentPathCornerIndex++;
            }

            if(currentPathCornerIndex >= currentPath.corners.Length)
            {
                currentPathCornerIndex = 0;
                currentPath = new NavMeshPath ();
                character.cInput.SetInput ( Vector2.zero );
            }
        }
    }

    public void FindDestinationWithinMobZone ()
    {
        Vector2 circle = Random.insideUnitCircle * npc.MobSpawner.Radius;
        Vector3 randomPosition = npc.MobSpawner.transform.position + new Vector3 ( circle.x, 1.0f, circle.y );

        if (NavMesh.SamplePosition ( randomPosition, out navHit, navMeshAgent.height * 4.0f, walkableAreaMask ))
        {
            if (navMeshAgent.CalculatePath ( navHit.position, currentPath ))
            {
                currentPathCornerIndex = 0;
            }
            else
            {
                Debug.LogError ( "Could not calculate path", this.gameObject );
            }
        }
        else
        {
            Debug.LogError ( "Sample position failed", this.gameObject );
        }
    }

    public void FindDestinationAwayFromEnemy (float range)
    {
        Vector3 randomPosition = EntityManager.instance.PlayerCharacter.transform.position + (EntityManager.instance.PlayerCharacter.transform.forward * range);

        if(Random.value <= 0.33f)
        {
            randomPosition = EntityManager.instance.PlayerCharacter.transform.position - (EntityManager.instance.PlayerCharacter.transform.right * range);
        }
        else if (Random.value <= 0.66f)
        {
            randomPosition = EntityManager.instance.PlayerCharacter.transform.position + (EntityManager.instance.PlayerCharacter.transform.right * range);
        }

        if (NavMesh.SamplePosition ( randomPosition, out navHit, navMeshAgent.height * 2.0f, walkableAreaMask ))
        {
            if (navMeshAgent.CalculatePath ( navHit.position, currentPath ))
            {
                currentPathCornerIndex = 0;
            }
            else
            {
                Debug.LogError ( "Could not calculate path", this.gameObject );
            }
        }
        else
        {
            Debug.LogError ( "Sample position failed", this.gameObject );
        }
    }

    public void ResetCurrentPath ()
    {
        currentPath = new NavMeshPath ();
        if (character == null) character = GetComponent<Character> ();
        if (character == null) Debug.Log ( "What1" );
        if (character.cInput == null) Debug.Log ( "What2" );
        character.cInput.SetInput ( 0.0f, 0.0f );
    }

    private void OnDrawGizmos ()
    {
        if (Application.isPlaying == false) return;
        for (int i = 0; i < currentPath.corners.Length; i++)
        {
            Gizmos.DrawCube ( currentPath.corners[i] + Vector3.up, Vector3.one * 0.25f );

            if (i > 0)
            {
                Gizmos.DrawLine ( currentPath.corners[i - 1] + Vector3.up, currentPath.corners[i] + Vector3.up);
            }
        }
    }
}