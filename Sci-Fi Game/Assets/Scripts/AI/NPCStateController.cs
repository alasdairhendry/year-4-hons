using System.Collections.Generic;
using UnityEngine;

public enum NPCAiState { Idle, NearMiss, Flee, Combat }

public class NPCStateController : MonoBehaviour
{
    [SerializeField] private NPC npc;
    [SerializeField] private NPCStateMachine stateMachine;
    [Space]
    [SerializeField] private AIStateControllerData aiStateControllerData;
    public NPCAiState NPCAiState { get; protected set; }

    public AIStateControllerData AiStateControllerData { get => aiStateControllerData; protected set => aiStateControllerData = value; }
    public NPCStateMachine StateMachine { get => stateMachine; protected set => stateMachine = value; }
    public WeaponData ChosenWeaponData { get; protected set; } = null;

    private float lastAttackedByPlayerCounter = 0.0f;
    private WorldMapObject worldMapObject;
    public System.Action<NPCAiState> onStateSwitch;

    private void Awake ()
    {
        worldMapObject = GetComponent<WorldMapObject> ();
    }

    protected void Start ()
    {
        ChooseWeapon ();
        SetDefaultBehaviour ();
        SubscribeToEvents ();      
    }

    private void ChooseWeapon ()
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
    }

    private void SetDefaultBehaviour ()
    {
        SwitchToIdleBehaviour ();
    }

    private void SubscribeToEvents ()
    {
        npc.Health.onHealthRemoved += (amount, damageType) =>
        {
            if(damageType == DamageType.PlayerAttack)
            {
                lastAttackedByPlayerCounter = 0.0f;
            }

            if (aiStateControllerData.FleesBelowHealthPercent > 0 && npc.Health.healthNormalised <= aiStateControllerData.FleesBelowHealthPercent)
            {
                if (stateMachine.currentState != aiStateControllerData.FleeBehaviour)
                    SwitchToFleeBehaviour ();
                return;
            }
            else
            {
                if (stateMachine.currentState != aiStateControllerData.CombatBehaviour)
                    SwitchToCombatBehaviour ();
                return;
            }
        };
    }

    protected void Update ()
    {
        CheckLastAttackedCounter ();
        CheckEnemyDistance ();
    }

    protected void LateUpdate () { }

    private void CheckLastAttackedCounter ()
    {
        if (aiStateControllerData.StopsAttackingAfterLastAttackDelay > 0)
        {
            if (stateMachine.currentState == aiStateControllerData.CombatBehaviour)
            {
                lastAttackedByPlayerCounter += Time.deltaTime;

                if (lastAttackedByPlayerCounter >= aiStateControllerData.StopsAttackingAfterLastAttackDelay)
                {
                    SwitchToIdleBehaviour ();
                }
            }
        }
    }

    private void CheckEnemyDistance ()
    {
        if (stateMachine.currentState != aiStateControllerData.CombatBehaviour) return;

        if (aiStateControllerData.StopsAttackingSqrDistanceIsPlayerWeaponDistance)
        {
            if (EntityManager.instance.PlayerCharacter.cWeapon.currentWeaponData == null)
                return;

            float distance = 0.0f;

            if (EntityManager.instance.PlayerCharacter.cWeapon.currentWeaponData.weaponAttackType == WeaponAttackType.Gun) distance = EntityManager.instance.PlayerCharacter.cWeapon.currentWeaponGunData.maxDistance;
            else distance = EntityManager.instance.PlayerCharacter.cWeapon.MeleeStrikeDistance;

            if (Vector3.Distance ( transform.position, EntityManager.instance.PlayerCharacter.transform.position ) > distance)
            {
                SwitchToIdleBehaviour ();
            }
        }
        else
        {
            if (aiStateControllerData.StopsAttackingOnEnemySqrDistanceDelay > 0)
            {
                if ((transform.position - EntityManager.instance.PlayerCharacter.transform.position).sqrMagnitude >= aiStateControllerData.StopsAttackingOnEnemySqrDistanceDelay)
                {
                    SwitchToIdleBehaviour ();
                }
            }
        }
    }

    protected virtual void SwitchStates (AIStateBase state, NPCAiState aiState)
    {
        if (stateMachine.currentState == state && state != null) return;
        stateMachine.SetState ( state );
        NPCAiState = aiState;
        onStateSwitch?.Invoke ( aiState );
    }

    public virtual void SwitchToIdleBehaviour ()
    {
        if (worldMapObject != null && worldMapObject.MapBlipType == MapBlipType.Enemy)
            worldMapObject.Unregister ();
        SwitchStates ( aiStateControllerData.IdleBehaviour, NPCAiState.Idle );
        npc.IsInCombat = false;
    }

    public virtual void SwitchToNearMissBehaviour ()
    {
        if (worldMapObject != null && worldMapObject.MapBlipType == MapBlipType.Enemy)
            worldMapObject.Unregister ();
        SwitchStates ( aiStateControllerData.NearMissBehaviour, NPCAiState.NearMiss );
    }

    public virtual void SwitchToFleeBehaviour ()
    {
        if (worldMapObject != null && worldMapObject.MapBlipType == MapBlipType.Enemy)
            worldMapObject.Register();

        SwitchStates ( aiStateControllerData.FleeBehaviour, NPCAiState.Flee );
        MessageBox.AddMessage ( npc.NpcData.NpcName + " tries to flee", MessageBox.Type.Warning );
        npc.IsInCombat = true;
    }

    public virtual void SwitchToCombatBehaviour ()
    {
        if (worldMapObject != null && worldMapObject.MapBlipType == MapBlipType.Enemy)
            worldMapObject.Register ();

        SwitchStates ( aiStateControllerData.CombatBehaviour, NPCAiState.Combat );
        npc.IsInCombat = true;
    }
}
