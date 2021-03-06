﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public Character character { get; protected set; }
    public CharacterIK characterIK { get; protected set; }
    public Animator animator { get; protected set; }

    [SerializeField] private float locomotionLerp = 10.0f;
    [SerializeField] private float runningStanceLerp = 2.5f;
    [SerializeField] private float fightStanceLerp = 5.0f;
    [SerializeField] private float crouchStanceLerp = 5.0f;

    [SerializeField] private List<AnimationClip> deathClips = new List<AnimationClip> ();
    //[SerializeField] private List<AnimationClip> meleeAttackClips = new List<AnimationClip> ();

    float animatorFloatForward = 0.0f;
    float animatorFloatSideway = 0.0f;
    float animatorFloatRunning = 0.0f;
    private AnimatorOverrideController overrideController;

    private void Awake ()
    {
        character = GetComponent<Character> ();
        characterIK = GetComponent<CharacterIK> ();
        animator = GetComponent<Animator> ();
        SetupOverrideController ();
    }

    private void SetupOverrideController ()
    {
        overrideController = new AnimatorOverrideController ( animator.runtimeAnimatorController );
        overrideController["death-01"] = deathClips.GetRandom ();
        animator.runtimeAnimatorController = overrideController;
    }

    public void OverrideDriveIdle(AnimationClip clip)
    {
        overrideController["driving-idle"] = clip;
    }

    public MeleeAttackAnimation RandomiseAttackAnimation (List<MeleeAttackAnimation> possibleClips, MeleeAttackAnimation previous)
    {
        MeleeAttackAnimation randomAnimation = possibleClips.GetRandom ( previous );
        overrideController["melee-attack-01"] = randomAnimation.clip;
        return randomAnimation;
    }

    public MeleeAttackComboAnimation RandomiseAttackAnimation (List<MeleeAttackComboAnimation> possibleClips, MeleeAttackComboAnimation previous)
    {
        MeleeAttackComboAnimation randomAnimation = possibleClips.GetRandom ( previous );
        overrideController["melee-attack-01"] = randomAnimation.clip;
        return randomAnimation;
    }

    private void Update ()
    {
        animatorFloatForward = animator.GetFloat ( "forward" );
        animatorFloatSideway = animator.GetFloat ( "sideway" );
        animatorFloatRunning = animator.GetFloat ( "running" );

        if (character.cInput.rawInput.y != 0 || character.cInput.rawInput.x != 0)
        {
            // TODO
            //DialogueManager.StopConversation ();
        }

        if (character.currentState == Character.State.Driving || character.currentState == Character.State.Falling)
        {
            animator.SetFloat ( "forward", Mathf.Lerp ( animatorFloatForward, 0.0f, Time.deltaTime * locomotionLerp ) );
            animator.SetFloat ( "sideway", Mathf.Lerp ( animatorFloatSideway, 0.0f, Time.deltaTime * locomotionLerp ) );
            animator.SetFloat ( "running", Mathf.Lerp ( animatorFloatRunning, 0.0f, Time.deltaTime * locomotionLerp ) );
            return;
        }


        if (character.shouldJump)
        {
            animator.SetTrigger ( "jump" );
            animator.ResetTrigger ( "jump" );
            character.shouldJump = false;
        }

        float fwd = (character.cInput.input.magnitude);

        if(character.IsAiming)
        {
            animator.SetFloat ( "forward", Mathf.Lerp ( animatorFloatForward, character.cInput.input.y, Time.deltaTime * locomotionLerp ) );
            animator.SetFloat ( "sideway", Mathf.Lerp ( animatorFloatSideway, character.cInput.input.x, Time.deltaTime * locomotionLerp ) );
        }
        else
        {
            animator.SetFloat ( "forward", Mathf.Lerp ( animatorFloatForward, fwd, Time.deltaTime * locomotionLerp ) );
            animator.SetFloat ( "sideway", Mathf.Lerp ( animatorFloatSideway, fwd, Time.deltaTime * locomotionLerp ) );
        }

        ////
        animator.SetFloat ( "running", Mathf.Lerp ( animatorFloatRunning, character.isRunning ? character.runPercentNormalised : 0.0f, Time.deltaTime * runningStanceLerp ) );
        animator.SetFloat ( "ads", character.IsAiming ? 1.0f : 0.0f );
        animator.SetBool ( "isGun", character.cWeapon.isEquipped && !character.cWeapon.isHolstered && character.cWeapon.currentWeaponData.weaponAttackType == WeaponAttackType.Gun );
        animator.SetBool ( "isMelee", character.cWeapon.isEquipped && !character.cWeapon.isHolstered && character.cWeapon.currentWeaponData.weaponAttackType == WeaponAttackType.Melee );
        ////

        //animator.SetFloat ( "fightstance", Mathf.Lerp ( animator.GetFloat ( "fightstance" ), character.IsAiming ? 1.0f : 0.0f, Time.deltaTime * fightStanceLerp ) );

        //animator.SetFloat ( "crouchstance", Mathf.Lerp ( animator.GetFloat ( "crouchstance" ), character.isCrouching ? 1.0f : 0.0f, Time.deltaTime * crouchStanceLerp ) );
        //animator.SetBool ( "weaponEquipped", character.cWeapon.isEquipped && !character.cWeapon.isHolstered );
        animator.SetBool ( "isPistol", character.cWeapon.weaponIsPistol );
    }

    public void OnFootStep ()
    {
        if (character.cInput.rawInput == Vector2.zero) return;
        SoundEffectManager.Play3D ( AssetManager.instance.GetAudioClip ( AudioClipAsset.Footstep ), AudioMixerGroup.SFX, transform.position, minDistance: 2, maxDistance: 15 );
    }
}
