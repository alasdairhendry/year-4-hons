using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "AI/New Flee State Data" )]
public class AIStateBaseFlee : AIStateBase
{
    public override void OnEnter (NPC npc)
    {
        npc.NPCNavMesh.ResetCurrentPath ();
    }

    public override void OnUpdate (NPC npc)
    {
        if((npc.transform.position - EntityManager.instance.PlayerCharacter.transform.position).sqrMagnitude > 400)
        {
            npc.Delete ();
            return;
        }

        if (npc.NPCNavMesh.currentPath.corners.Length <= 0)
        {
            npc.NPCNavMesh.FindDestinationAwayFromEnemy ( 25 );
            npc.Character.isRunning = true;
        }
        else
        {
            if (Random.value <= 0.005f)
            {
                npc.NPCNavMesh.FindDestinationAwayFromEnemy ( 25 );
                npc.Character.isRunning = true;
            }
        }
    }

    public override void OnExit (NPC npc)
    {
        npc.NPCNavMesh.ResetCurrentPath ();
        npc.Character.isRunning = false;
    }
}
