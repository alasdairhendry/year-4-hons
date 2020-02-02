using QuestFlow.DialogueEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QuestFlow
{
    public class DialogueCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [Space]
        [SerializeField] private GameObject dialoguePanel;
        [SerializeField] private TextMeshProUGUI dialogueActorText;
        [SerializeField] private TextMeshProUGUI dialogueEntryText;
        [SerializeField] private Image dialogueSpriteImage;
        [SerializeField] private Button continueButton;
        [Space]
        [SerializeField] private GameObject answerPanel;
        [SerializeField] private Button answerButton;

        private List<Button> answerButtons = new List<Button> ();

        private void Start ()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;

            answerButtons.Add ( answerButton );
            answerButton.onClick.AddListener ( () => { DialogueManager.instance.OnClickAnswer ( 0 ); } );

            DialogueManager.instance.onConversationStart += OnConversationStart;
            DialogueManager.instance.onConversationEnd += OnConversationEnd;
            DialogueManager.instance.onDialogueNodeBegin += OnDialogueNode;
            DialogueManager.instance.onAnswerNodeBegin += OnAnswerNode;
        }

        private void OnConversationStart (Conversation conversation)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }

        private void OnDialogueNode (Dialogue dialogue)
        {
            continueButton.onClick.RemoveAllListeners ();
            continueButton.onClick.AddListener ( () => { DialogueManager.instance.OnClickContinue (); } );

            DisplayDialogueWindow ( dialogue.GetActor.actorName, dialogue.dialogue, dialogue.GetActor.sprite );
        }

        private void DisplayDialogueWindow (string actor, string entry, Sprite sprite)
        {
            answerPanel.SetActive ( false );
            dialoguePanel.SetActive ( true );

            dialogueActorText.text = actor;
            dialogueEntryText.text = entry;
            dialogueSpriteImage.sprite = sprite;
        }

        private void OnAnswerNode (List<Answer> answers)
        {
            if (answers.Count == 1)
            {
                continueButton.onClick.RemoveAllListeners ();
                continueButton.onClick.AddListener ( () => { DialogueManager.instance.OnClickAnswer ( 0 ); } );
                DisplayDialogueWindow ( DialogueManager.instance.GetPlayerActor.actorName, answers[0].dialogue, DialogueManager.instance.GetPlayerActor.sprite );
                return;
            }

            answerPanel.SetActive ( true );
            dialoguePanel.SetActive ( false );

            if (answers.Count > answerButtons.Count)
            {
                for (int i = answerButtons.Count; i < answers.Count; i++)
                {
                    GameObject newButton = Instantiate ( answerButton.gameObject, answerButton.transform.parent );
                    answerButtons.Add ( newButton.GetComponent<Button> () );
                    int x = i;
                    answerButtons[i].onClick.AddListener ( () => { DialogueManager.instance.OnClickAnswer ( x ); } );
                }
            }

            for (int i = 0; i < answerButtons.Count; i++)
            {
                if (i >= answers.Count)
                {
                    answerButtons[i].gameObject.SetActive ( false );
                    continue;
                }

                answerButtons[i].gameObject.SetActive ( true );
                answerButtons[i].gameObject.GetComponentInChildren<TextMeshProUGUI> ().text = answers[i].dialogue;
            }
        }

        private void OnConversationEnd (Conversation conversation)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
