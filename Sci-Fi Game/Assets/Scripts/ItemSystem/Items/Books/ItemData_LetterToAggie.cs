using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_LetterToAggie : ItemBaseDataBook
{
    public ItemData_LetterToAggie (int ID) : base ( ID )
    {
        base.Name = "Letter";
        base.Description = "A letter addressed to Aggie";
        base.category = ItemCategory.Book;

        base.IsSellable = false;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = false;
        base.RelatedQuestIDs = new string[] { "onesmallfavour" };
        
        base.BuyPrice = 0;
        base.FetchSprite ();

        this.SetTitle ( "Letter" );
        this.InsertParagraph ( "Dearest Aggie," );
        this.InsertBreak (2);
        this.InsertParagraph ( "The heart aches with every moment that passes with you not around. I lay alone, in bed at night, waiting for your return." );    
        this.InsertBreak (2);
        this.InsertParagraph ( "I knock on your door every day, hopeful that one day it will be unlocked." );
        this.InsertBreak (2);
        this.InsertParagraph ( "If this letter ever reaches you, please know that I will never again eat your last french fry." );
        this.InsertBreak (2);
        this.InsertParagraph ( "With Love," );
        this.InsertBreak (1);
        this.InsertParagraph ( "Brian" );
    }
}
