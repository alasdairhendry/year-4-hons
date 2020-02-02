﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace QuestFlow.QuestEngine
{
    [CreateAssetMenu ( menuName = "Quest Flow/Quest" )]
    public class Quest : NodeGraph
    {
        public string questID;
        public string questName;
        public QuestReward reward;

        public override Node AddNode (Type type)
        {
            if (type != typeof ( Start ))
            {
                if (!ContainsStartNode ())
                {
                    return base.AddNode<Start> ();
                }
            }

            return base.AddNode ( type );
        }

        public bool ContainsStartNode ()
        {
            bool foundNode = false;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] as Start) { foundNode = true; break; }
            }

            return foundNode;
        }

        public Start GetStartNode ()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] as Start) { return nodes[i] as Start; }
            }

            return null;
        }

        public bool GetNextNodeFromCurrent(NodeBase current, out NodeBase nextNode)
        {
            if (current.ConditionsAreValid ())
            {
                NodePort port = current.GetOutputPort ( "output" );

                for (int i = 0; i < port.ConnectionCount; i++)
                {
                    NodePort connection = port.GetConnection ( i );
                    NodeBase connectedNode = connection.node as NodeBase;

                    if (connectedNode != null)
                    {
                        nextNode = connectedNode;
                        return true;
                    }
                }

                nextNode = current;
                return false;
            }
            else
            {
                nextNode = current;
                return false;
            }
        }

        public bool CanBeOffered ()
        {
            Start startNode = GetStartNode ();

            if(startNode == null)
            {
                Debug.LogError ( questID + " cannot be offered as it does not have a start node" );
                return false;
            }

            if (startNode.ConditionsAreValid ())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void RemoveNode (Node node)
        {
            if (node is State)
            {
                List<Condition> conditions = (node as NodeBase).conditions;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (conditions[i] != null)
                        AssetDatabase.RemoveObjectFromAsset ( conditions[i] );
                }
            }

            base.RemoveNode ( node );
        }
    }   
}