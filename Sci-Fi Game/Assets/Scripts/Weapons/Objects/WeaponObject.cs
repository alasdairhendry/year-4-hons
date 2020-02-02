using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public bool isHolstered { get; protected set; }

    public virtual void OnHolstered () { isHolstered = true; }
    public virtual void OnUnholstered () { isHolstered = false; }
}
