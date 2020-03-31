using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public Character character { get; protected set; }

    public Vector2 input { get; set; }
    public Vector2 rawInput { get; set; }

    private void Awake ()
    {
        character = GetComponent<Character> ();
    }

    private void Update ()
    {
        if (!character.IsAI)
        {
            CheckAim ();

            if (character.CanMove)
            {
                input = new Vector2 ( Input.GetAxis ( "Horizontal" ), Input.GetAxis ( "Vertical" ) ).normalized;
                rawInput = new Vector2 ( Input.GetAxisRaw ( "Horizontal" ), Input.GetAxisRaw ( "Vertical" ) ).normalized;
            }
            else
            {
                input = Vector2.zero;
                rawInput = Vector2.zero;
            }

            if (Input.GetKeyDown ( KeyCode.C ))
            {
                character.isCrouchingInput = !character.isCrouchingInput;
            }

            character.isRunning = Input.GetKey ( KeyCode.LeftShift );

            if (Input.GetKeyDown ( KeyCode.Space ))
            {
                character.shouldJump = true;
            }
        }
        else
        {
            if (!character.CanMove)
            {
                input = Vector2.zero;
                rawInput = Vector2.zero;
            }
        }
    }

    public void SetInput (Vector2 input)
    {
        if (character.CanMove)
        {
            this.input = input;
            this.rawInput = input;
        }
        else
        {
            input = Vector2.zero;
            rawInput = Vector2.zero;
        }
    }

    public void SetInput (float x, float y)
    {
        if (character.CanMove)
        {
            this.input = new Vector2 ( x, y );
            this.rawInput = new Vector2 ( x, y );
        }
        else
        {
            input = Vector2.zero;
            rawInput = Vector2.zero;
        }
    }

    public bool isAimInput { get; protected set; } = false;

    private void CheckAim ()
    {
        //if (character.cWeapon.ShouldAim)
        //{
            //isAimInput = true;
            //return;
        //}

        isAimInput = Input.GetMouseButton ( 1 ) && character.isDead == false;
    }
}
