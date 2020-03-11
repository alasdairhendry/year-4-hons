using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : UIPanel
{
    public static MessageBox instance;

    public enum Type { Info, Warning, Error }
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject messageBoxEntryPrefab;
    [SerializeField] private Transform entriesPanel;

    private List<Message> messages = new List<Message> ();
    private List<Message> messagesQueue = new List<Message> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }
        Open ();
        AddMessage ( "Welcome to the game." );
    }

    private void Update ()
    {
        CheckQueue ();
    }

    private void CheckQueue ()
    {
        for (int i = messagesQueue.Count - 1; i >= 0; i--)
        {
            if (messagesQueue[i] == null)
            {
                messagesQueue.RemoveAt ( i );
                continue;
            }
        }

        for (int i = 0; i < messagesQueue.Count; i++)
        {
            messagesQueue[i].delay -= Time.deltaTime;

            if (messagesQueue[i].delay <= 0.0f)
            {
                CreateMessage ( messagesQueue[i] );
                messagesQueue[i] = null;
            }
        }
    }

    public override void Open ()
    {
        mainPanel.SetActive ( true );
        isOpened = true;
        UIPanelController.instance.OnPanelOpened ( this );
    }

    public override void Close (bool bypassCloseCheck = false)
    {
        if (isOpened == false && !bypassCloseCheck) return;

        mainPanel.SetActive ( false );
        isOpened = false;
        UIPanelController.instance.OnPanelClosed ( this );
    }

    public static void AddMessage (string message, Type type = Type.Info, float delay = 0)
    {
        if (instance == null) return;
        instance.messagesQueue.Add ( new Message ( message, null, type, delay ) );
    }

    public static void CreateMessage (Message message)
    {
        if (instance == null) return;

        if(instance.messages.Count > 20)
        {
            DeleteFirstMessage ();
        }

        GameObject go = Instantiate ( instance.messageBoxEntryPrefab );
        go.transform.SetParent ( instance.entriesPanel );

        message.time = DateTime.Now;
        message.gameObject = go;

        string colourisedMessage = "";

        switch (message.type)
        {
            case Type.Info:
                colourisedMessage = ColourHelper.TagColour ( message.message, ColourDescription.MessageBoxInfo );
                break;
            case Type.Warning:
                colourisedMessage = ColourHelper.TagColour ( message.message, ColourDescription.MessageBoxWarning );
                break;
            case Type.Error:
                colourisedMessage = ColourHelper.TagColour ( message.message, ColourDescription.MessageBoxError );
                break;
            default:
                colourisedMessage = ColourHelper.TagColour ( message.message, ColourDescription.MessageBoxInfo );
                break;
        }

        go.GetComponentInChildren<TextMeshProUGUI> ().text = string.Format ( "[{0}] {1}", message.time.ToShortTimeString (), colourisedMessage );
        instance.messages.Add ( message );
    }

    private static void DeleteFirstMessage ()
    {
        Destroy ( instance.messages[0].gameObject );
        instance.messages.RemoveAt ( 0 );
    }

    public class Message
    {
        public string message;
        public Type type;
        public GameObject gameObject;
        public DateTime time;
        public float delay;

        public Message (string message, GameObject gameObject = null, Type type = Type.Info, float delay = 0)
        {
            this.message = message;
            this.gameObject = gameObject;
            this.type = type;
            this.delay = delay;
        }
    }
}
