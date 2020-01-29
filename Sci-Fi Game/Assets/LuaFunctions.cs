using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.QuestMachine;
using System.Collections.Generic;
using PixelCrushers;
using System;
using System.Linq;

public class LuaFunctions : MonoBehaviour, IMessageHandler
{
    [SerializeField] private QuestDatabase questDatabase;

    void OnEnable ()
    {
        Lua.RegisterFunction ( "GiveCoins", this, SymbolExtensions.GetMethodInfo ( () => GiveCoins ( 0 ) ) );
        Lua.RegisterFunction ( "TakeCoins", this, SymbolExtensions.GetMethodInfo ( () => TakeCoins ( 0 ) ) );
        Lua.RegisterFunction ( "GiveItem", this, SymbolExtensions.GetMethodInfo ( () => GiveItem ( 0, 0 ) ) );
        Lua.RegisterFunction ( "TakeItem", this, SymbolExtensions.GetMethodInfo ( () => TakeItem ( 0, 0 ) ) );
        Lua.RegisterFunction ( "HasItem", this, SymbolExtensions.GetMethodInfo ( () => HasItem ( 0 ) ) );
        Lua.RegisterFunction ( "HasItemAmount", this, SymbolExtensions.GetMethodInfo ( () => HasItemAmount ( 0, 0 ) ) );
        Lua.RegisterFunction ( "CanOffer", this, SymbolExtensions.GetMethodInfo ( () => CanOffer ( string.Empty ) ) );

        MessageSystem.AddListener ( this, "OnQuestFinish", "QuestID" );
        MessageSystem.AddListener ( this, "OnQuestStart", "QuestID" );
        MessageSystem.AddListener ( this, "Give", "Item" );
    }

    public bool HasItem (double id)
    {
        return EntityManager.instance.PlayerInventory.CheckHasItem ( (int)id );
    }

    public bool HasItemAmount (double id, double amount)
    {
        return EntityManager.instance.PlayerInventory.CheckHasItem ( (int)id, (int)amount );
    }

    void OnDisable ()
    {
        // Note: If this script is on your Dialogue Manager & the Dialogue Manager is configured
        // as Don't Destroy On Load (on by default), don't unregister Lua functions.
        Lua.UnregisterFunction ( "GiveCoins" ); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction ( "TakeCoins" ); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction ( "GiveItem" ); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction ( "TakeItem" ); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction ( "HasItem" ); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction ( "HasItemAmount" ); // <-- Only if not on Dialogue Manager.
        Lua.UnregisterFunction ( "CanOffer" ); // <-- Only if not on Dialogue Manager.

        MessageSystem.RemoveListener ( this );
    }

    public bool CanOffer(string questID)
    {
        PixelCrushers.QuestMachine.Quest q = questDatabase.questAssets.First ( x => x.id.value == questID );
        Debug.Log ( "Check CanOffer " + q.GetEditorName () + " - " + q.canOffer.ToString () );
        return q.canOffer;
    }

    public void GiveCoins (double amount)
    {
        EntityManager.instance.PlayerInventory.AddCoins ( (int)amount );
    }

    public void TakeCoins (double amount)
    {
        EntityManager.instance.PlayerInventory.RemoveCoins ( (int)amount );
    }

    public void GiveItem (double id, double amount)
    {
        EntityManager.instance.PlayerInventory.AddItem ( (int)id, (int)amount );
    }

    public void TakeItem (double id, double amount)
    {
        EntityManager.instance.PlayerInventory.RemoveItem ( (int)id, (int)amount );
    }

    public void OnFinishQuest(string questID)
    {
        Debug.Log ( "OnFinishQuest" );
    }

    void IMessageHandler.OnMessage (MessageArgs messageArgs)
    {
        if (messageArgs.message == "OnQuestFinish")
        {
            QuestRewards.instance.GiveQuestReward ( (string)messageArgs.firstValue );
            MessageBox.AddMessage ( "Congratulations! You have completed " + questDatabase.questAssets.First ( x => x.id.value == (string)messageArgs.firstValue ).GetEditorName () );
        }
        else if (messageArgs.message == "OnQuestStart")
        {
            MessageBox.AddMessage ( "Quest Started: " + ColourHelper.TagColour ( questDatabase.questAssets.First ( x => x.id.value == (string)messageArgs.firstValue ).GetEditorName (), ColourDescription.MessageBoxWarning ) );
        }
        else if(messageArgs.message == "Give" && messageArgs.parameter == "Item")
        {
            EntityManager.instance.PlayerInventory.AddItem ( (int)messageArgs.values[0], (int)messageArgs.values[1] );
        }
    }
}