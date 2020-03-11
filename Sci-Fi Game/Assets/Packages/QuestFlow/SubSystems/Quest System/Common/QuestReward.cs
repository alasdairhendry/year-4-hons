using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow.QuestEngine
{
    [System.Serializable]
    public class QuestReward
    {
        public ItemReward mandatoryReward = new ItemReward ();
        //public List<ItemReward> decisionRewards = new List<ItemReward> ();
        public int talentPoints = 0;
        public List<XPReward> xpRewards = new List<XPReward> ();
        public List<string> annotatedRewards = new List<string> ();
    }

    [System.Serializable]
    public class ItemReward
    {
        public List<ItemAmountPair> reward = new List<ItemAmountPair> ();
    }

    [System.Serializable]
    public class XPReward
    {
        public SkillType skill = SkillType.Shooting;
        public float xp = 0;
    }
}
