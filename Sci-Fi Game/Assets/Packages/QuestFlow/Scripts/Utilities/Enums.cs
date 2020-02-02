using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public enum QuestState { NotStarted, InProgress, Completed, Failed }
    public enum ConditionRequirement { All, Any }
    public enum ComparisonOperator { Equals, GreaterThan, LessThan, GreaterEquals, LessEquals }
}
