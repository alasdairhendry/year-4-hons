using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu ( menuName = "AI/New Idle State Data" )]
public class AIStateBaseIdle : AIStateBase
{
    public override void OnEnter (NPC npc)
    {
        npc.NPCNavMesh.ResetCurrentPath ();
    }

    public override void OnUpdate (NPC npc)
    {
        if(npc.NPCNavMesh.currentPath.corners.Length <= 0)
        {
            if(Random.value < 0.01f)
            {
                npc.NPCNavMesh.FindDestinationWithinMobZone ();
            }
        }
    }

    public override void OnExit (NPC npc)
    {
        npc.NPCNavMesh.ResetCurrentPath ();
    }
}
