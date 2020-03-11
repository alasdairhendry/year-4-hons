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
        base.IsStackable = true;
        base.RelatedQuestIDs = new string[] { };
        
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

        this.InsertHeader ( "Step Four - Optional" );
        this.InsertBreak ();
        this.InsertParagraph ( "Top of your cake with some chocolate using a bar of chocolate." );
        this.InsertBreak ( 2 );

        this.InsertHeader ( "Step Five" );
        this.InsertBreak ();
        this.InsertParagraph ( "Serve and enjoy!" );
    }
}
