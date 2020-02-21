using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionOpenHorviksShop : Action
    {
        public override void DoAction ()
        {
            QuestActionHelper.instance.OpenHorviksToolStore ();
        }
    }
}
