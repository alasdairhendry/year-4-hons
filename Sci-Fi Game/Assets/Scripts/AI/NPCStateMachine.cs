using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    private NPC npc;
    public AIStateBase currentState { get; protected set; }

    private void Awake ()
    {
        npc = GetComponent<NPC> ();
    }

    public void SetState (AIStateBase state)
    {
        if (currentState != null)
        {
            currentState.OnExit ( npc );
        }

        if (state != null)
        {
            currentState = state;
            state.OnEnter ( npc );
        }
    }

    private void Update ()
    {
        if (currentState != null)
        {
            currentState.OnUpdate ( npc );
        }
    }
}
