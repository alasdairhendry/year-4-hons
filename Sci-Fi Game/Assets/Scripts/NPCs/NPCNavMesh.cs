using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCNavMesh : MonoBehaviour
{
    [SerializeField] private Transform pathSeeker;
    [SerializeField] private NPC npc;

    private Character character;
    private NavMeshHit navHit;
    public static int WalkableAreaMask
    {
        get
        {
            return 1 << NavMesh.GetAreaFromName ( "Walkable" );
        }
    }

    public bool HasPath { get; protected set; } = false;
    public Vector3 CurrentDestination { get; protected set; } = new Vector3 ();
    public NavMeshPath currentPath { get; protected set; }
    public int currentPathCornerIndex { get; protected set; } = 0;

    public System.Action onPathObtained;
    public System.Action onPathComplete;
    public System.Action onPathCleared;

    public bool currentPathIsMandatory { get; protected set; } = false;

    private void Awake ()
    {
        character = GetComponent<Character> ();
        currentPath = new NavMeshPath ();
    }

    private void Update ()
    {
        MoveAlongPath ();
    }

    private void LateUpdate ()
    {
        if (!HasPath) return;
        if (npc.MeshRenderer.isVisible == false) return;

        if (currentPath.corners.Length > 0)
            pathSeeker.transform.position = currentPath.corners[currentPathCornerIndex];
    }

    private void MoveAlongPath ()
    {
        if ((!npc.MeshRenderer.isVisible && !currentPathIsMandatory)|| npc.isInConversation || !HasPath)
        {
            character.cInput.SetInput ( 0.0f, 0.0f );
            return;
        }

        if (currentPath.corners.Length > 0)
        {
            if (character.CanMove == false) return;

            character.cInput.SetInput ( 0.0f, 1.0f );

            Vector3 dir = (pathSeeker.position - transform.position);
            dir.y = 0;

            if (dir != Vector3.zero)
                transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.LookRotation ( dir ), Time.deltaTime * 5.0f );

            if (Vector3.Distance ( transform.position, pathSeeker.position ) < 0.25f)
            {
                currentPathCornerIndex++;
            }

            if (currentPathCornerIndex >= currentPath.corners.Length)
            {
                OnPathComplete ();
                //ClearCurrentPath ();
            }
        }
    }

    private void CheckIsOnNavMesh ()
    {
        if (NavMesh.SamplePosition ( transform.position, out navHit, 8.0f, WalkableAreaMask ))
        {
                transform.position = navHit.position;
        }
    }

    public void SetDestination (Vector3 worldPosition, bool disableCurrentPathIfFails, bool mandatory)
    {
        if (mandatory == false)
        {
            if (npc.MeshRenderer.isVisible == false)
            {
                return;
            }
        }

        if (NavMesh.SamplePosition ( worldPosition, out navHit, 8.0f, WalkableAreaMask ))
        {
            NavMeshPath path = new NavMeshPath ();

            if (NavMesh.CalculatePath ( transform.position, navHit.position, WalkableAreaMask, path ))
            {
                CurrentDestination = navHit.position;
                currentPathCornerIndex = 0;
                currentPath = path;
                HasPath = true;
                OnPathObtained ();
                currentPathIsMandatory = mandatory;
            }
            else
            {
                if (disableCurrentPathIfFails)
                    ClearCurrentPath ();

                CheckIsOnNavMesh ();

                Debug.LogError ( "Could not calculate path", this.gameObject );
            }
        }
        else
        {
            if (disableCurrentPathIfFails)
                ClearCurrentPath ();

            Debug.LogError ( "Sample position failed", this.gameObject );
        }
    }

    public void SetDestinationWithinMobZone (bool disableCurrentPathIfFails, bool mandatory)
    {
        Vector2 circle = Random.insideUnitCircle * npc.MobSpawner.Radius;
        Vector3 randomPosition = npc.MobSpawner.transform.position + new Vector3 ( circle.x, 1.0f, circle.y );
        SetDestination ( randomPosition, disableCurrentPathIfFails, mandatory );
    }

    public void SetDestinationAwayFromEnemy (float range, bool disableCurrentPathIfFails, bool mandatory)
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

        SetDestination ( randomPosition, disableCurrentPathIfFails, mandatory );
    }

    public void ClearCurrentPath ()
    {
        currentPathIsMandatory = false;
        HasPath = false;
        currentPathCornerIndex = 0;
        currentPath = new NavMeshPath ();
        OnPathCleared ();

        if (character == null) character = GetComponent<Character> ();      
        character.cInput.SetInput ( 0.0f, 0.0f );
    }

    private void OnPathObtained ()
    {
        onPathObtained?.Invoke ();
    }

    private void OnPathComplete ()
    {
        currentPathIsMandatory = false;
        HasPath = false;
        currentPathCornerIndex = 0;
        currentPath = new NavMeshPath ();
        if (character == null) character = GetComponent<Character> ();
        character.cInput.SetInput ( 0.0f, 0.0f );
        onPathComplete?.Invoke ();
    }

    private void OnPathCleared ()
    {      
        onPathCleared?.Invoke ();
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