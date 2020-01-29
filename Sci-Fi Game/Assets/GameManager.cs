﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake ()
    {
        if (instance == null) instance = this;
        else if (instance != this) { Destroy ( this.gameObject ); return; }

        InitialiseObjects ();
    }

    private void InitialiseObjects ()
    {
        ItemDatabase.Initialise ();
    }
}
