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

                    NodePort groupPort = group.GetOutputPort ( "output" );
                    Node outputNode;
                    GetRecursiveFirstGroupChildNode ( port, out outputNode );

                    if(outputNode != null)
                    {
                        if(outputNode is Answer)
                        {
                            GetAnswers ( port );
                            return;
                        }
                        else if(outputNode is Dialogue)
                        {
                            // The first valid item we found was dialogue, so we'll display that to the user
                            OnStatementNodeBegin ( outputNode as Dialogue );
                            return;
                        }
                    }

                    //for (int x = 0; x < groupPort.ConnectionCount; x++)
                    //{
                    //    NodePort groupConnection = port.GetConnection ( x );


                    //    if (groupConnection.node is Dialogue)
                    //    {
                    //        Dialogue dialogue = (groupConnection.node as Dialogue);

                    //        if (dialogue.ConditionsAreValid ())
                    //        {
                    //            // The first valid item we found was dialogue, so we'll display that to the user
                    //            OnStatementNodeBegin ( dialogue );
                    //            return;
                    //        }
                    //    }
                    //    else if (groupConnection.node is Answer)
                    //    {
                    //        Answer answer = (groupConnection.node as Answer);

                    //        if (answer.ConditionsAreValid ())
                    //        {
                    //            // The first valid item we found is an answer, so we'll compile all answers to display to the user
                    //            List<Answer> validAnswerConnections = new List<Answer> ();
                    //            List<Group> validGroupConnections = new List<Group> ();
                    //            GetAnswersRecursively ( ref validAnswerConnections, ref validGroupConnections, port );

                    //            for (int g = 0; g < validGroupConnections.Count; g++)
                    //            {
                    //                OnGroupNodeBegin ( validGroupConnections[g] );
                    //            }

                    //            OnAnswerNodeBegin ( validAnswerConnections );
                    //            return;
                    //        }
                    //    }

                    //}

                    //if (group.ConditionsAreValid ())
                    //{
                    //    // The first valid item we found was dialogue, so we'll display that to the user
                    //    OnGroupNodeBegin ( group );
                    //    return;
                    //}
                }
                else if (connection.node is Answer)
                {
                    Answer answer = (connection.node as Answer);

                    if (answer.ConditionsAreValid ())
                    {
                        GetAnswers ( port );
                        return;
                    }
                }
            }

            // No valid connections found. End the conversation.
            OnConversationEnd ();
        }

        private void GetDialogue (NodePort port)
        {
            // The first valid item we found is dialogue, so we'll compile all dialogue to display to the user
            List<Answer> validAnswerConnections = new List<Answer> ();
            List<Group> validGroupConnections = new List<Group> ();
            GetAnswersRecursively ( ref validAnswerConnections, ref validGroupConnections, port );

            for (int g = 0; g < validGroupConnections.Count; g++)
            {
                OnGroupNodeBegin ( validGroupConnections[g] );
            }

            OnAnswerNodeBegin ( validAnswerConnections );
        }

        private void GetAnswers (NodePort port)
        {
            // The first valid item we found is an answer, so we'll compile all answers to display to the user
            List<Answer> validAnswerConnections = new List<Answer> ();
            List<Group> validGroupConnections = new List<Group> ();
            GetAnswersRecursively ( ref validAnswerConnections, ref validGroupConnections, port );

            for (int g = 0; g < validGroupConnections.Count; g++)
            {
                OnGroupNodeBegin ( validGroupConnections[g] );
            }

            OnAnswerNodeBegin ( validAnswerConnections );
        }

        private bool GetRecursiveFirstGroupChildNode (NodePort groupPort, out Node outputNode)
        {
            for (int i = 0; i < groupPort.ConnectionCount; i++)
            {
                NodePort connection = groupPort.GetConnection ( i );

                if (connection.node is Answer)
                {
                    Answer answer = (connection.node as Answer);
                    if (answer.ConditionsAreValid ())
                    {
                        outputNode = connection.node;
                        return true;
                    }
                }
                else if (connection.node is Dialogue)
                {
                    if ((connection.node as Dialogue).ConditionsAreValid ())
                    {
                        outputNode = connection.node;
                        return true;
                    }
                }
                else if (connection.node is Group)
                {
                    if ((connection.node as Group).ConditionsAreValid ())
                    {
                        if (GetRecursiveFirstGroupChildNode ( connection.node.GetOutputPort ( "output" ), out outputNode ))
                        {
                            return true;
                        }
                    }
                }
            }

            outputNode = null;
            return false;
        }

        private void GetAnswersRecursively(ref List<Answer> validAnswers, ref List<Group> validGroups, NodePort port)
        {
            for (int i = 0; i < port.ConnectionCount; i++)
            {
                NodePort connection = port.GetConnection ( i );

                if (connection.node is Answer)
                {
                    Answer answer = (connection.node as Answer);
                    if (answer.ConditionsAreValid ())
                    {
                        validAnswers.Add ( answer as Answer );
                    }
                }
                else if (connection.node is Group)
                {
                    Group group = (connection.node as Group);

                    if (group.ConditionsAreValid ())
                    {
                        validGroups.Add ( connection.node as Group );
                        NodePort groupPort = group.GetOutputPort ( "output" );

                        GetAnswersRecursively ( ref validAnswers, ref validGroups, groupPort );
                    }
                }
            }
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
            node.DoActions ();
        }

        private void OnConversationEnd ()
        {
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