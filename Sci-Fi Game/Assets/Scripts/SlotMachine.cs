using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> ledMeshRenderers = new List<MeshRenderer> ();
    [SerializeField] private Material ledOffMaterial;
    [SerializeField] private Material ledOnMaterial;
    [Space]
    [SerializeField] private float runTime = 2.0f;
    [SerializeField] private float ledBlinkDelay = 0.2f;
    [Space]
    [SerializeField] private int costToPlay = 15;
    [SerializeField] private DropTable dropTable;
    [Space]
    [SerializeField] private AudioClip runningAudioClip;
    [SerializeField] private AudioClip winAudioClip;
    [SerializeField] private AudioClip loseAudioClip;
    [SerializeField] private AudioClip leverAudioClip;
    [SerializeField] private Animator leverAnimator;

    private bool isRunning = false;
    private float currentRunTime = 0.0f;
    private float currentBlinkTime = 0.0f;
    private AudioSource runningAudioSource;
    private int currentLEDIndex = -1;

    private void Start ()
    {
        runningAudioSource = SoundEffect.Create ( this, 1, 1, 5, this.transform );
        runningAudioSource.clip = runningAudioClip;
        runningAudioSource.loop = true;
    }

    public void Interact ()
    {
        if (isRunning) return;

        if (EntityManager.instance.PlayerInventory.CheckHasItemQuantity ( 3, costToPlay ))
        {
            EntityManager.instance.PlayerInventory.RemoveCoins ( costToPlay );
        }
        else
        {
            SoundEffect.Play3D ( leverAudioClip, transform.position, 1, 5 );
            leverAnimator.SetTrigger ( "pull" );
            return;
        }

        currentLEDIndex = ledMeshRenderers.GetRandomIndex (currentLEDIndex);
        SwitchLED ();
        SoundEffect.Play3D ( leverAudioClip, transform.position, 1, 5 );
        leverAnimator.SetTrigger ( "pull" );
        runningAudioSource.Play ();
        isRunning = true;
    }

    private void Update ()
    {
        if (!isRunning) return;

        currentRunTime += Time.deltaTime;
        currentBlinkTime += Time.deltaTime;

        if (currentBlinkTime >= ledBlinkDelay)
        {
            currentBlinkTime = 0.0f;
            SwitchLED ();
        }

        if(currentRunTime >= runTime)
        {
            OnFinishRun ();
        }
    }

    private void SwitchLED ()
    {
        ledMeshRenderers[currentLEDIndex].material = ledOffMaterial;
        currentLEDIndex = ledMeshRenderers.GetRandomIndex ( currentLEDIndex );
        ledMeshRenderers[currentLEDIndex].material = ledOnMaterial;
    }

    private void OnFinishRun ()
    {
        isRunning = false;
        currentRunTime = 0.0f;
        currentBlinkTime = 0.0f;
        ledMeshRenderers[currentLEDIndex].material = ledOffMaterial;
        runningAudioSource.Stop ();

        Inventory.ItemStack roll = dropTable.RollTable ();

        if(roll == null)
        {
            OnLose ();
        }
        else
        {
            OnWin ( roll );
        }
    }

    private void OnWin (Inventory.ItemStack roll)
    {
        EntityManager.instance.PlayerInventory.AddItem ( roll.ID, roll.Amount );
        SoundEffect.Play3D ( winAudioClip, transform.position, 1, 5 );
    }

    private void OnLose ()
    {
        SoundEffect.Play3D ( loseAudioClip, transform.position, 1, 5 );
    }
}
