using UnityEngine;

[CreateAssetMenu ( menuName = "AI/New Combat State Data" )]
public class AIStateBaseCombat : AIStateBase
{
    public override void OnEnter (NPC npc)
    {
        npc.NPCNavMesh.ResetCurrentPath ();

        if (npc.NPCStateController.ChosenWeaponData != null)
        {
            if (npc.Character.cWeapon.isEquipped == false)
                npc.Character.cWeapon.Equip ( npc.NPCStateController.ChosenWeaponData );

            npc.Character.cWeapon.SetHolsterState ( false );
        }
    }

    public override void OnUpdate (NPC npc)
    {
        Character playerCharacter = EntityManager.instance.PlayerCharacter;

        RotateTowardsPlayer ( npc, playerCharacter );
        HandleGunMechanics ( npc, playerCharacter );
        HandleMeleeMechanics ( npc );
    }

    private void RotateTowardsPlayer (NPC npc, Character playerCharacter)
    {
        Transform npcTransform = npc.transform;

        Vector3 dir = playerCharacter.transform.position - npcTransform.position;
        dir.y = 0;

        if (dir != Vector3.zero)
        {
            npcTransform.rotation = Quaternion.Slerp ( npcTransform.rotation, Quaternion.LookRotation ( dir ), Time.deltaTime * 10.0f );
        }
    }

    private void HandleGunMechanics (NPC npc, Character playerCharacter)
    {
        if (!npc.Character.cWeapon.isEquipped) return;
        if (npc.Character.cWeapon.currentWeaponData.weaponAttackType == WeaponAttackType.Melee) return;


        if (Random.value < npc.NPCStateController.AiStateControllerData.StrategicReloadChance)
        {
            npc.Character.cWeapon.Reload ();
            return;
        }        

        if (npc.Character.cWeapon.isFiring)
        {
            if (playerCharacter.isDead)
            {
                if (Random.value < npc.NPCStateController.AiStateControllerData.StopCombatAfterPlayerDeathChance)
                {
                    npc.Character.cWeapon.NPC_StopFire ();
                    npc.NPCStateController.SwitchToIdleBehaviour ();
                }

                return;
            }

            if (Random.value < npc.NPCStateController.AiStateControllerData.StopFiringChance)
            {
                npc.Character.cWeapon.NPC_StopFire ();
            }
        }
        else
        {
            if (Random.value < npc.NPCStateController.AiStateControllerData.FireChance)
            {
                if (npc.Character.cWeapon.NPC_TryFire () == false)
                {
                    if (Random.value < npc.NPCStateController.AiStateControllerData.EmptyClipReloadChance)
                    {
                        npc.Character.cWeapon.Reload ();
                    }
                }
            }
        }
    }

    private void HandleMeleeMechanics (NPC npc)
    {
        if (!npc.Character.cWeapon.isEquipped) return;
        if (npc.Character.cWeapon.currentWeaponData.weaponAttackType == WeaponAttackType.Gun) return;
    }

    public override void OnExit (NPC npc)
    {
        npc.NPCNavMesh.ResetCurrentPath ();
        npc.Character.cWeapon.NPC_StopFire ();

        if (npc.Character.isDead) return;

        if (npc.NPCStateController.AiStateControllerData.WeaponAlwaysEquipped == false)
            npc.Character.cWeapon.Unequip ();
        if (npc.NPCStateController.AiStateControllerData.WeaponAlwaysUnholstered == false)
            npc.Character.cWeapon.SetHolsterState ( true );
    }
}
