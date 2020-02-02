using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;

namespace QuestFlow.DialogueEngine
{
    [CreateAssetMenu ( menuName = "Quest Flow/Conversation" )]
    public class Conversation : NodeGraph
    {
        public ActorData defaultActor;

        public void BeginConversation ()
        {
            Start startNode = (Start)nodes.First ( x => x is Start );
            GetNextNodeFromStatement ( startNode );
        }

        public void EndConversation ()
        {
            OnConversationEnd ();
        }

        private void GetNextNodeFromStatement (Node node)
        {
            if (node == null)
            {
                Debug.Log ( "Null node" );
                OnConversationEnd ();
                return;
            }

            NodePort port = node.GetOutputPort ( "output" );

            for (int i = 0; i < port.ConnectionCount; i++)
            {
                NodePort connection = port.GetConnection ( i );

                if (connection.node is Dialogue)
                {
                    Dialogue dialogue = (connection.node as Dialogue);

                    if (dialogue.ConditionsAreValid ())
                    {
                        // The first valid item we found was dialogue, so we'll display that to the user
                        OnStatementNodeBegin ( dialogue );
                        return;
                    }
                }
                else if(connection.node is Group)
                {
                    Group group = (connection.node as Group);

                    if (group.ConditionsAreValid ())
                    {
                        // The first valid item we found was dialogue, so we'll display that to the user
                        OnGroupNodeBegin ( group );
                        return;
                    }
                }
                else if (connection.node is Answer)
                {
                    Answer answer = (connection.node as Answer);

                    if (answer.ConditionsAreValid ())
                    {
                        // The first valid item we found is an answer, so we'll compile all answers to display to the user
                        List<Answer> validAnswerConnections = new List<Answer> ();

                        for (int x = 0; x < port.ConnectionCount; x++)
                        {
                            // Nested loop through each connection again to find all valid answers

                            NodePort answerConnection = port.GetConnection ( x );

                            if (answerConnection.node is Answer)
                            {
                                if ((answerConnection.node as Answer).ConditionsAreValid ())
                                {
                                    validAnswerConnections.Add ( (answerConnection.node as Answer) );
                                }
                            }
                        }

                        OnAnswerNodeBegin ( validAnswerConnections );
                        return;
                    }
                }
            }

            // No valid connections found. End the conversation.
            OnConversationEnd ();
        }

        private void OnStatementNodeBegin (Dialogue node)
        {
            DialogueManager.instance.OnDialogueNodeBegin ( node );
        }

        private void OnAnswerNodeBegin (List<Answer> responses)
        {
            DialogueManager.instance.OnAnswerNodeBegin ( responses );
        }

        private void OnGroupNodeBegin(Group node)
        {
            DialogueManager.instance.OnGroupNodeBegin ( node );
            GetNextNodeFromStatement ( node );
        }

        private void OnConversationEnd ()
        {
            //activeNode = null;
            DialogueManager.instance.OnConversationEnd ();
        }

        public void ContinueDialogue (Dialogue node)
        {
            node.DoActions ();
            GetNextNodeFromStatement ( node );
        }

        public void OnSelectAnswer (Answer node)
        {
            node.DoActions ();
            GetNextNodeFromStatement ( node );
        }

        public override Node AddNode (Type type)
        {
            if (type != typeof ( Start ))
            {
                if (!ContainsStartNode ())
                {
                    return base.AddNode<Start> ();
                }

                if (defaultActor == null)
                {
                    Debug.LogError ( "Please assign a default actor before adding nodes." );
                    return null;
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

        public override void RemoveNode (Node node)
        {
            if (node is DialogueEntry)
            {
                List<Condition> conditions = (node as DialogueEntry).conditions;
                for (int i = 0; i < conditions.Count; i++)
                {
                    if (conditions[i] != null)
                        AssetDatabase.RemoveObjectFromAsset ( conditions[i] );
                }
            }

            base.RemoveNode ( node );
        }

        public override Node CopyNode (Node original)
        {
            if (original is DialogueEntry)
            {
                if ((original as DialogueEntry).conditions.Count > 0)
                {
                    Debug.LogError ( "Duplicating a node that contains conditions will result in shared condition assets. This is not advised." );
                }

                if ((original as DialogueEntry).actions.Count > 0)
                {
                    Debug.LogError ( "Duplicating a node that actions conditions will result in shared action asset. This is not advised." );

                }
            }

            return base.CopyNode ( original );
        }

        public static List<T> GetAllInstances<T> () where T : ScriptableObject
        {
            string[] guids = AssetDatabase.FindAssets ( "t:" + typeof ( T ).Name );  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath ( guids[i] );
                a[i] = AssetDatabase.LoadAssetAtPath<T> ( path );
            }

            return a.ToList (); ;

        }
    }
}