using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace QuestFlow.QuestEngine
{
    [CreateNodeMenu ( "" )]
    [System.Serializable]
    public class NodeBase : Node
    {
        [HideInInspector] [Input] public State input;
        [HideInInspector] [Output ( connectionType = ConnectionType.Override )] public State output;

        [HideInInspector] public bool conditionsFoldout = false;
        [HideInInspector] public bool actionsFoldout = false;
        [HideInInspector] public bool outputFoldout = false;

        [TextArea(5, 20)] public string questLog = "";
        [HideInInspector] public List<Condition> conditions = new List<Condition> ();
        [HideInInspector] public List<Action> actions = new List<Action> ();
        [HideInInspector] public ConditionRequirement conditionRequirementType = ConditionRequirement.All;

        public void OnEnterNode ()
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                conditions[i].OnEnter ();
            }
        }

        public void OnExitNode ()
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                conditions[i].OnExit ();
            }
        }

        public bool ConditionsAreValid ()
        {
            if (conditions.Count <= 0) return true;

            switch (conditionRequirementType)
            {
                case ConditionRequirement.All:

                    for (int i = 0; i < conditions.Count; i++)
                    {
                        if (conditions[i] == null)
                        {
                            Debug.Log ( "Quest Entry has null conditions", this );
                            continue;
                        }

                        if (conditions[i].GetResult () == false)
                        {
                            return false;
                        }
                    }

                    return true;

                case ConditionRequirement.Any:

                    for (int i = 0; i < conditions.Count; i++)
                    {
                        if (conditions[i] == null)
                        {
                            Debug.Log ( "Quest Entry has null conditions", this );
                            continue;
                        }

                        if (conditions[i].GetResult () == true)
                        {
                            return true;
                        }
                    }

                    return false;
            }

            return true;
        }

        public void DoActions ()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i] != null)
                {
                    actions[i].DoAction ();
                }
            }
        }
    }
}