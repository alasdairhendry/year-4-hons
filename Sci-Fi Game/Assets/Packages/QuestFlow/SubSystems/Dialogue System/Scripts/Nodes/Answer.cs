using UnityEngine;

namespace QuestFlow.DialogueEngine
{
    [System.Serializable]
    [NodeWidth ( 256 )]
    [NodeTint ( "#98DBFF" )]
    public class Answer : DialogueEntry
    {
        [TextArea] public string dialogue = "";
    }
}