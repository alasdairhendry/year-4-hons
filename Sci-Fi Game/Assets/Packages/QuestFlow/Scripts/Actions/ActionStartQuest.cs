using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionStartQuest : Action
    {
        [SerializeField] public Quest quest;

        public override void DoAction ()
        {
            if(quest == null)
            {
                Debug.Log ( "IM FUCKING NULL" );
                return;
            }
            QuestManager.instance.StartQuest ( quest );
        }
    }
}
