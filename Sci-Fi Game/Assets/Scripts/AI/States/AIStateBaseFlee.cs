using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "AI/New Flee State Data" )]
public class AIStateBaseFlee : AIStateBase
{
    public override void OnEnter (NPC npc)
    {
        npc.NPCNavMesh.ClearCurrentPath ();
        npc.Character.cWeapon.SetHolsterState ( true );
    }

    public override void OnUpdate (NPC npc)
    {
        if((npc.transform.position - EntityManager.instance.PlayerCharacter.transform.position).sqrMagnitude > 400)
        {
            npc.DieImmediately ();
            return;
        }

        if (npc.NPCNavMesh.currentPath.corners.Length <= 0)
        {
            npc.NPCNavMesh.SetDestinationAwayFromEnemy ( 25, false, true );
            npc.Character.isRunning = true;
        }
        else
        {
            if (Random.value <= 0.0025f)
            {
                npc.NPCNavMesh.SetDestinationAwayFromEnemy ( 2, false, false );
                npc.Character.isRunning = true;
            }
        }
    }

    public override void OnExit (NPC npc)
    {
        npc.NPCNavMesh.ClearCurrentPath ();
        npc.Character.isRunning = false;
    }
}
