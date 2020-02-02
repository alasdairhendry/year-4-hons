using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestFlow
{
    public static class Comparison
    {
        public static bool GetResult (int a, int b, ComparisonOperator comparison)
        {
            switch (comparison)
            {
                case ComparisonOperator.Equals:
                    return a == b;
                case ComparisonOperator.GreaterThan:
                    return a > b;
                case ComparisonOperator.LessThan:
                    return a < b;
                case ComparisonOperator.GreaterEquals:
                    return a >= b;
                case ComparisonOperator.LessEquals:
                    return a <= b;
                default:
                    Debug.LogError ( "Comparison does not exist" );
                    return false;
            }
        }
    }
}