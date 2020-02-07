using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateBase : ScriptableObject
{
    public abstract void OnEnter (NPC npc);
    public abstract void OnUpdate (NPC npc);
    public abstract void OnExit (NPC npc);
}
