using UnityEngine;

namespace QuestFlow.DialogueEngine
{
    [System.Serializable]
    [NodeWidth ( 256 )]
    [NodeTint ( "#98DBFF" )]
    public class Answer : DialogueEntry
    {
        [TextArea] public string dialogue = "";

        public override bool ConditionsAreValid ()
        {
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
    }
}