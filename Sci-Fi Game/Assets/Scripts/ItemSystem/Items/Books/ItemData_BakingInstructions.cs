using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_BakingInstructions : ItemBaseDataBook
{
    public ItemData_BakingInstructions (int ID) : base ( ID )
    {
        base.Name = "Baking Instructions";
        base.Description = "The cook gave me this to help with my cake baking";
        base.category = ItemCategory.Book;

        base.IsSellable = false;
        base.IsSoulbound = false;
        base.IsUnique = false;

        base.IsStackable = true;
        base.RelatedQuestIDs = new string[] { "CooksApprentice" };
        
        base.BuyPrice = 0;
        base.FetchSprite ();

        this.SetTitle ( "Baking Instructions" );

        this.InsertHeader ( "Step One" );
        this.InsertBreak ();
        this.InsertParagraph ( "Mix the flour with the milk to create a light, fluffy mixture." );
        this.InsertBreak ( 2 );

        this.InsertHeader ( "Step Two" );
        this.InsertBreak ();
        this.InsertParagraph ( "Whisk in the egg to form a sweet batter." );
        this.InsertBreak ( 2 );

        this.InsertHeader ( "Step Three" );
        this.InsertBreak ();
        this.InsertParagraph ( "Pour the mixture into a cake tin, and bake in an oven." );
        this.InsertBreak ( 2 );

        this.InsertHeader ( "Step Four" );
        this.InsertBreak ();
        this.InsertParagraph ( "Create some chocolate shavings by using a knife on a chocolate bar." );
        this.InsertBreak ( 2 );

        this.InsertHeader ( "Step Five" );
        this.InsertBreak ();
        this.InsertParagraph ( "Sprinkle the chocolate onto the cake." );
        this.InsertBreak ( 2 );

        this.InsertHeader ( "Step Six" );
        this.InsertBreak ();
        this.InsertParagraph ( "Use a knife on the cake to cut it into equal slices." );
    }
}
