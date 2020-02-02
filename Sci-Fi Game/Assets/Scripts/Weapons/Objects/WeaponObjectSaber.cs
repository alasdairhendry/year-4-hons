using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObjectSaber : WeaponObject
{
    [SerializeField] private Animator animator;

    public override void OnHolstered ()
    {
        base.OnHolstered ();
        animator.SetBool ( "sheathed", true );
    }

    public override void OnUnholstered ()
    {
        base.OnUnholstered ();
        animator.SetBool ( "sheathed", false );
    }
}
