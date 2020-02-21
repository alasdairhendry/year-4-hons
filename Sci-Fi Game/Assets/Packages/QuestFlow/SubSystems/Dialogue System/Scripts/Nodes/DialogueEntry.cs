using QuestFlow;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace QuestFlow.DialogueEngine
{
    [System.Serializable]
    public abstract class DialogueEntry : NodeBase
    {
        public enum ConditionRequirementType { All, Any }

        [Input] public DialogueEntry input;
        [Output] public DialogueEntry output;

        [HideInInspector] public bool conditionsFoldout = false;
        [HideInInspector] public bool actionsFoldout = false;
        [HideInInspector] public bool outputFoldout = false;

        [HideInInspector] public ConditionRequirementType conditionRequirementType = ConditionRequirementType.All;

        public List<Condition> conditions = new List<Condition> ();
        [HideInInspector] public List<Action> actions = new List<Action> ();

        public virtual bool ConditionsAreValid ()
        {
            //Debug.Log ( "Checking conditions" );

            if (conditions.Count <= 0)
            {
                //Debug.Log ( "No conditions available" );
                return true;
            }

            switch (conditionRequirementType)
            {
                case ConditionRequirementType.All:

                    //Debug.Log ( "Condition Type - All" );

                    for (int i = 0; i < conditions.Count; i++)
                    {
                        //Debug.Log ( "Condition Check - " + i );
                        if (conditions[i] == null)
                        {
                            //Debug.Log ( "Condition is null" );
                            Debug.Log ( "Dialogue Entry has null conditions", this );
                            continue;
                        }

                        if (conditions[i].GetResult () == false)
                        {
                            //Debug.Log ( "Condition result was false" );
                            return false;
                        }
                    }

                    //Debug.Log ( "Condition result was true" );
                    return true;

                case ConditionRequirementType.Any:

                    for (int i = 0; i < conditions.Count; i++)
                    {
                        if (conditions[i] == null)
                        {
                            Debug.Log ( "Dialogue Entry has null conditions", this );
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

        // Use this for initialization
        protected override void Init ()
        {
            base.Init ();
        }

        // Return the correct value of an output port when requested
        public override object GetValue (NodePort port)
        {
            if (port.fieldName == "output") return GetInputValue<DialogueEntry> ( "input", input );
            return null; // Replace this
        }
    }
}