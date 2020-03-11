using System;
using System.Collections.Generic;
using System.Linq;
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
        runningAudioSource = SoundEffectManager.Create ( this, 1, 1, 5, this.transform );
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
            SoundEffectManager.Play3D ( leverAudioClip, AudioMixerGroup.SFX, transform.position, minDistance: 1, maxDistance: 5 );
            leverAnimator.SetTrigger ( "pull" );
            return;
        }

        currentLEDIndex = ledMeshRenderers.GetRandomIndex (currentLEDIndex);
        SwitchLED ();
        SoundEffectManager.Play3D ( leverAudioClip, AudioMixerGroup.SFX, transform.position, minDistance: 1, maxDistance: 5 );
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

        List<Inventory.ItemStack> drops = new List<Inventory.ItemStack> ();
        bool wasFactionRoll = false;

        if (dropTable.RollTable ( out drops, out wasFactionRoll ))
        {
            if (wasFactionRoll) MessageBox.AddMessage ( "Your Faction Specialisation helps you find an item", MessageBox.Type.Warning );
            OnWin ( drops );
        }
        else
        {
            OnLose ();
        }
    }

    private void OnWin (List<Inventory.ItemStack> roll)
    {
        EntityManager.instance.PlayerInventory.AddMultipleItems ( roll );
        SoundEffectManager.Play3D ( winAudioClip, AudioMixerGroup.SFX, transform.position, minDistance: 1, maxDistance: 5 );
        MessageBox.AddMessage ( "You won " + roll.Sum ( x => x.Amount ) + " crowns on the machine!", MessageBox.Type.Info );
    }

    private void OnLose ()
    {
        SoundEffectManager.Play3D ( loseAudioClip, AudioMixerGroup.SFX, transform.position,minDistance: 1,maxDistance: 5 );
        MessageBox.AddMessage ( "You didn't win anything.", MessageBox.Type.Error );
    }
}
