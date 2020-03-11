using System.Collections;
using System.Collections.Generic;
using QuestFlow.DialogueEngine;
using QuestFlow.QuestEngine;
using UnityEngine;

public class NPC_EvilCivilian : NPC
{
    [SerializeField] private Quest swordof100truthsQuest;
    [SerializeField] private QuestFlow.QuestEngine.NodeBase swordof100truthsTargetNode;
    [SerializeField] private QuestFlow.QuestEngine.NodeBase swordof100truthsSwitchToNode;

    protected override void OnConversationEnd (Conversation obj)
    {
        base.OnConversationEnd ( obj );
        if(QuestManager.instance.GetQuestDataByID(swordof100truthsQuest.questID).currentNode == swordof100truthsTargetNode)
        {
            overrideDefaultAttackOption = true;
            attackOptionOverride = NPCAttackOption.CanBeAttacked;
            GetComponentInChildren<Interactable> ().TextColour = ColourDescription.InteractionAttackableEnemy;
            NPCStateController.SwitchToCombatBehaviour ();
        }
    }

    protected override void OnDeathByDamage ()
    {
        base.OnDeathByDamage ();

        if (QuestManager.instance.GetQuestDataByID ( swordof100truthsQuest.questID ).currentNode == swordof100truthsTargetNode)
        {
            QuestManager.instance.SetQuestSubstate ( swordof100truthsQuest, swordof100truthsSwitchToNode );
        }
    }

    public override float OnBeforeDamagedByWeapon (float damage, WeaponData currentWeaponData)
    {
        if(currentWeaponData.weaponItemID != 74)
        {
            MessageBox.AddMessage ( "The evil civilian doesn't seem to be affected by attacks from this weapon.", MessageBox.Type.Error );
            return 0.0f;
        }
        return base.OnBeforeDamagedByWeapon ( damage, currentWeaponData );
    }
}
