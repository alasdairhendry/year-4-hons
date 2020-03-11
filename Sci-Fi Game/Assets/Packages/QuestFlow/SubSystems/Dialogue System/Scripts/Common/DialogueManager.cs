using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestFlow.DialogueEngine
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;

        private Dialogue currentDialogueNode = null;
        private List<Answer> currentAnswerNodes = new List<Answer> ();

        [SerializeField] private ActorData playerActor;
        public ActorData GetPlayerActor { get => playerActor; }
        public bool ConversationIsActive { get => conversationIsActive; protected set => conversationIsActive = value; }

        private bool conversationIsActive = false;
        private Conversation currentConversation;

        public enum DialogueState { None, Statement, Answer }
        private DialogueState currentState = DialogueState.None;

        public System.Action<Conversation> onConversationStart;
        public System.Action<Dialogue> onDialogueNodeBegin;
        public System.Action<List<Answer>> onAnswerNodeBegin;
        public System.Action<Group> onGroupNodeBegin;
        public System.Action<ActorData> onActorDialogueBegins;
        public System.Action<Conversation> onConversationEnd;

        private void Awake ()
        {
            if (instance == null) instance = this;
            else if (instance != this) { Destroy ( this.gameObject ); return; }
        }

        public void OnHotkeyPressed (KeyCode keyCode, bool isShift, bool isControl, bool isAlt)
        {
            if (currentConversation != null)
            {
                if (keyCode == KeyCode.Space)
                {
                    switch (currentState)
                    {
                        case DialogueState.None:
                            break;
                        case DialogueState.Statement:
                            OnClickContinue ();
                            break;
                        case DialogueState.Answer:
                            if (currentAnswerNodes.Count == 1)
                                OnClickAnswer ( 0 );
                            break;
                    }
                }

                if (currentState == DialogueState.Answer)
                {
                    if (currentAnswerNodes.Count > 1)
                    {
                        if (keyCode == KeyCode.Alpha1)
                            OnClickAnswer ( 0 );

                        if (keyCode == KeyCode.Alpha2)
                            OnClickAnswer ( 1 );

                        if (keyCode == KeyCode.Alpha3)
                            OnClickAnswer ( 2 );

                        if (keyCode == KeyCode.Alpha4)
                            OnClickAnswer ( 3 );

                        if (keyCode == KeyCode.Alpha5)
                            OnClickAnswer ( 4 );

                        if (keyCode == KeyCode.Alpha6)
                            OnClickAnswer ( 5 );
                    }
                }
            }
        }

        public void StartConversation (Conversation conversation)
        {
            EndCurrentConversation ();
            currentState = DialogueState.None;
            conversationIsActive = true;
            currentConversation = conversation;
            onConversationStart?.Invoke ( conversation );
            currentConversation.BeginConversation ();
        }

        public void EndCurrentConversation ()
        {
            if (currentConversation != null)
            {
                currentConversation.EndConversation ();
            }
        }

        public void OnClickContinue ()
        {
            Dialogue node = currentDialogueNode;
            currentDialogueNode = null;
            currentConversation.ContinueDialogue ( node );
        }

        public void OnClickAnswer (int index)
        {
            if (index >= currentAnswerNodes.Count) return;
            Answer answer = currentAnswerNodes[index];
            currentAnswerNodes.Clear ();
            currentConversation.OnSelectAnswer ( answer );
        }

        public void OnDialogueNodeBegin (Dialogue node)
        {
            currentDialogueNode = node;
            currentState = DialogueState.Statement;
            onDialogueNodeBegin?.Invoke ( node );
            onActorDialogueBegins?.Invoke ( node.GetActor );
        }

        public void OnAnswerNodeBegin (List<Answer> answers)
        {
            currentAnswerNodes.Clear ();

            for (int i = 0; i < answers.Count; i++)
            {
                currentAnswerNodes.Add ( answers[i] );
            }

            currentState = DialogueState.Answer;

            onAnswerNodeBegin?.Invoke ( answers );
            onActorDialogueBegins?.Invoke ( playerActor );
        }

        public void OnGroupNodeBegin(Group node)
        {
            onGroupNodeBegin?.Invoke ( node );
        }

        public void OnConversationEnd ()
        {
            currentState = DialogueState.None;
            currentDialogueNode = null;
            currentAnswerNodes.Clear ();
            conversationIsActive = false;
            Conversation conversation = currentConversation;
            currentConversation = null;
            onConversationEnd?.Invoke ( conversation );
        }
    }
}