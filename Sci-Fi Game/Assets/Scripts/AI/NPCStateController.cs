using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateController : MonoBehaviour
{
    [SerializeField] private NPC npc;
    [SerializeField] private NPCStateMachine stateMachine;
    [Space]
    [SerializeField] private AIStateControllerData aiStateControllerData;

    public AIStateControllerData AiStateControllerData { get => aiStateControllerData; set => aiStateControllerData = value; }
    public WeaponData ChosenWeaponData { get; protected set; } = null;

    protected void Start ()
    {
        if (aiStateControllerData.WeaponChoices.Count > 0)
        {
            ChosenWeaponData = aiStateControllerData.WeaponChoices.GetRandom ();

            if (ChosenWeaponData != null)
            {
                if (aiStateControllerData.WeaponAlwaysEquipped) npc.Character.cWeapon.Equip ( ChosenWeaponData );
                if (aiStateControllerData.WeaponAlwaysUnholstered) npc.Character.cWeapon.SetHolsterState ( false );
                else npc.Character.cWeapon.SetHolsterState ( true );
            }
        }

        SwitchToIdleBehaviour ();

        npc.Health.onHealthRemoved += (amount, damageType) =>
        {
            if (npc.Health.healthNormalised <= aiStateControllerData.FleesBelowHealthPercent)
            {
                SwitchToFleeBehaviour ();
                return;
            }
            else
            {
                SwitchToCombatBehaviour ();
                return;
            }
        };
    }

    protected void Update () { }
    protected void LateUpdate () { }

    protected virtual void SwitchStates(AIStateBase state)
    {
        if (stateMachine.currentState == state) return;
        if (state == null) return;
        stateMachine.SetState ( state );
    }

    public virtual void SwitchToIdleBehaviour ()
    {
        GetComponent<WorldMapObject> ().Unregister ();
        SwitchStates ( aiStateControllerData.IdleBehaviour );
    }

    public virtual void SwitchToNearMissBehaviour ()
    {
        GetComponent<WorldMapObject> ().Unregister ();
        SwitchStates ( aiStateControllerData.NearMissBehaviour );
    }

    public virtual void SwitchToFleeBehaviour ()
    {
        GetComponent<WorldMapObject> ().Register ();
        SwitchStates ( aiStateControllerData.FleeBehaviour );
    }

    public virtual void SwitchToCombatBehaviour ()
    {
        GetComponent<WorldMapObject> ().Register ();
        SwitchStates ( aiStateControllerData.CombatBehaviour );
    }
}
