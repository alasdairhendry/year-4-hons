using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    private static MessageBox instance;

    public enum Type { Info, Warning, Error }
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private GameObject panel;

    private List<Message> messages = new List<Message> ();

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        AddMessage ( "Welcome to the game." );
    }

    public void Open ()
    {
        panel.SetActive ( true );
    }

    public void Close ()
    {
        panel.SetActive ( false );
    }

    public void Trigger ()
    {
        if (panel.activeSelf) Close ();
        else Open ();
    }


    public static void AddMessage (string message, Type type = Type.Info)
    {
        if (instance == null) return;

        Message _message = new Message ( message, type );

        instance.messages.Add ( _message );
        instance.UpdateMessageList ( _message );
    }

    private void UpdateMessageList (Message message)
    {
        if (messages.Count > 1)
            textField.text += "\n";

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

        textField.text += string.Format ( "[{0}] {1}", message.time.ToShortTimeString (), colourisedMessage );
    }

    public class Message
    {
        public string message;
        public Type type;
        public DateTime time;

        public Message (string message, Type type = Type.Info)
        {
            this.message = message;
            this.type = type;
            this.time = DateTime.Now;
        }
    }
}
