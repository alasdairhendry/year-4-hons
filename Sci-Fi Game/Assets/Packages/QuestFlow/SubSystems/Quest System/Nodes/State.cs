using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace QuestFlow.QuestEngine
{
    [System.Serializable]
    [NodeWidth ( 256 )]
    [NodeTint ( "#98DBFF" )]
    public class State : NodeBase
    {   
        public override object GetValue (NodePort port)
        {
            if (port.fieldName == "output") return output;
            return null; // Replace this
        }
    }
}