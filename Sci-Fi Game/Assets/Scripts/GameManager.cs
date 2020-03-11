using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private float globalCooldownTime = 0.5f;
    public float GlobalCooldownTimeModified
    {
        get
        {
            return globalCooldownTime - (globalCooldownTime * TalentManager.instance.GetTalentModifier ( TalentType.Haste ));
        }
    }
    private float globalCooldownCounter = 0.0f;
    public bool GlobalCooldownIsActive { get { return globalCooldownCounter > 0.0f; } }
    public float GlobalCooldownNormalised { get { return globalCooldownCounter / GlobalCooldownTimeModified; } }
    public System.Action<bool> OnGlobalCooldownStateChange;
    public System.Action OnGlobalCooldownFired;
    public System.Action OnGlobalCooldownReset;

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

    private void Update ()
    {
        if(globalCooldownCounter > 0)
        {
            globalCooldownCounter -= Time.deltaTime;

            if (globalCooldownCounter <= 0)
            {
                OnGlobalCooldownReset?.Invoke ();
                OnGlobalCooldownStateChange?.Invoke ( GlobalCooldownIsActive );
                globalCooldownCounter = 0.0f;
            }
        }
    }

    public void FireGlobalCooldown ()
    {
        globalCooldownCounter = GlobalCooldownTimeModified;
        OnGlobalCooldownFired?.Invoke ();
        OnGlobalCooldownStateChange?.Invoke ( GlobalCooldownIsActive );
    }

    public bool CanFireEvent (bool fireIfAble)
    {
        if (GlobalCooldownIsActive)
        {
            MessageBox.AddMessage ( "I can't do that yet.", MessageBox.Type.Error );
            return false;
        }

        if (fireIfAble)
            FireGlobalCooldown ();

        return true;
    }
}
