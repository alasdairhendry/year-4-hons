using UnityEngine;

public class NPCStateMachine : MonoBehaviour
{
    [SerializeField] private NPC npc;
    public AIStateBase currentState { get; protected set; }

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
