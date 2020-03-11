using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankObject : MonoBehaviour
{
    public void Interact ()
    {
        BankCanvas.instance.Open ();
    }
}
