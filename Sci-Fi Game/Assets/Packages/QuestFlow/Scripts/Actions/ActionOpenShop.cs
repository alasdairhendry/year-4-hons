using QuestFlow.QuestEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public class ActionOpenShop : Action
    {
        [SerializeField] private QuestActionHelper.Shop.ShopID shopID;

        public override void DoAction ()
        {
            QuestActionHelper.instance.OpenShop ( shopID );
        }
    }
}
