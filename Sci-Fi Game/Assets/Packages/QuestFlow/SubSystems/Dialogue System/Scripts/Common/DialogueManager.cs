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

        public void StartConversation (Conversation conversation)
        {
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