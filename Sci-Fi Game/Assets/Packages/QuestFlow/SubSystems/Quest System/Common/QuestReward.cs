using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow.QuestEngine
{
    [System.Serializable]
    public class QuestReward
    {
        public Reward mandatoryReward = new Reward ();
        public List<Reward> decisionRewards = new List<Reward> ();
    } 

    [System.Serializable]
    public class Reward
    {
        public List<ItemAmountPair> reward = new List<ItemAmountPair> ();
    }
}
