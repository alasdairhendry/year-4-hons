using UnityEngine;
using XNode;

namespace QuestFlow.DialogueEngine
{
    [CreateNodeMenu ( "" )]
    [NodeTint ( "#7DEFB0" )]
    public class Start : NodeBase
    {
        [Output] public DialogueEntry output;
        [HideInInspector] public bool outputFoldout = false;

        private void Reset ()
        {
            name = "Start";
        }
    }
}