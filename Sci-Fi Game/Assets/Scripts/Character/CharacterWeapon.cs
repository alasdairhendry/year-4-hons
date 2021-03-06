﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterWeapon : MonoBehaviour
{
    [SerializeField] private List<WeaponData> allWeaponData = new List<WeaponData> ();
    [SerializeField] private TextMeshProUGUI clipText;
    [SerializeField] private float minHitDistance = 2.0f;
    [SerializeField] private GameObject ricochetPrefab;
    [SerializeField] private List<Transform> comboViews = new List<Transform> ();

    public Character character { get; protected set; }
    public WeaponData currentWeaponData { get; protected set; }
    public WeaponGunData currentWeaponGunData { get; protected set; }
    public WeaponMeleeData currentWeaponMeleeData { get; protected set; }
    public GameObject currentWeaponObject { get; protected set; }
    public WeaponGunData.FireType currentFireType { get; protected set; }
    public Transform muzzlePoint { get; protected set; }

    public bool isHolstered { get; protected set; } = true;
    public bool isEquipped { get { return currentWeaponData != null; } }
    public bool weaponIsPistol
    {
        get
        {
            if (currentWeaponData == null) return false;
            if (currentWeaponData.weaponAttackType == WeaponAttackType.Melee) return false;
            return currentWeaponGunData.weaponType == WeaponGunData.WeaponType.Pistol;
        }
    }
    public TransformData GetCurrentIKData
    {
        get
        {
            if (currentWeaponData == null) return null;
            if (currentWeaponData.weaponAttackType == WeaponAttackType.Melee) return null;
            return (character.currentVehicle != null) ? currentWeaponGunData.inVehicleIkData : currentWeaponGunData.ikData;
        }
    }
    public Transform WeaponHandTarget
    {
        get
        {
            if (currentWeaponData == null) return null;
            if (currentWeaponObject == null) return null;
            return currentWeaponObject.transform.Find ( "hand-ik-target" );
        }
    }

    [Range(0.0f, 1.0f)]
    public float fireCooldown = 0.0f;
    public bool isFiring = false;
    public float currentAmmo = 0;
    public float currentReloadTime = 0.0f;

    public System.Action<WeaponData> onWeaponEquiped;
    public System.Action<WeaponData> onWeaponUnequiped;
    public System.Action<WeaponData> onWeaponHolstered;
    public System.Action<WeaponData> onWeaponUnholstered;

    public bool ShouldAim { get; protected set; }
    private float shouldAimCooldown = 1.0f;
    public bool IsComboMode { get; protected set; }

    public Vector3 AimPosition = new Vector3 ();
    public RaycastHit aimEnemyHit;
    public NPC npcInSights = null;
    public bool hasAimHit = false;

    private int currentBurstCounter = 0;
    private float currentBurstCooldown = 0.0f;

    private AudioSource spinUpDelaySource;
    private AudioSource spinDownDelaySource;

    private float currentSpinUpTime = 0.0f;

    private float currentMeleeAnimationLength = 0;
    private float nextAttackAnimationDelay = 0;
    public System.Action<WeaponAttackType> OnAttack;

    private void Awake ()
    {
        character = GetComponent<Character> ();
        spinUpDelaySource = SoundEffectManager.Create ( this, 1.0f, parent: this.transform );
        spinDownDelaySource = SoundEffectManager.Create ( this, 1.0f, parent: this.transform );
        if (character.IsAI == false)
            Equip ( EntityManager.instance.FistsWeaponData, true );

        SetWeaponUI (); 
    }

    private void Update ()
    {
        CheckMeleeAttackDelay ();

        if (currentWeaponData != null)
        {
            if (currentWeaponData.weaponAttackType == WeaponAttackType.Gun)
            {
                CheckFireInput ();
                CheckIsFiring ();
                CheckBurstCooldown ();
                CheckReload ();
                CheckAimPosition ();
            }
            else
            {
                CheckMeleeAttack ();
            }
        }

        SetWeaponUI ();
        CheckShouldAim ();

        if (character.IsAI) return;

        if (Input.GetKeyDown ( KeyCode.H ))
        {
            SetHolsterState ( !isHolstered );
        }
    }

    private void CheckShouldAim ()
    {
        if (isHolstered)
        {
            shouldAimCooldown = 0.0f;
            ShouldAim = false;
            return;
        }

        if (shouldAimCooldown > 0.0f)
        {
            shouldAimCooldown -= Time.deltaTime;
            if (shouldAimCooldown <= 0.0f)
            {
                shouldAimCooldown = 0.0f;
                ShouldAim = false;
            }
        }
    }

    public void SetHolsterState (bool state, bool bypassAnimationDelay = false)
    {
        if (!isEquipped) return;
        if (isHolstered == state) return;

        if (state && !bypassAnimationDelay)
        {
            if (currentMeleeAnimationLength > 0 || nextAttackAnimationDelay > 0)
                return;
        }

        isHolstered = state;

        if (isHolstered)
        {
            if (character.IsAI == false)
            {
                Mouse.SetCursorState ( CursorLockMode.None, true );
                AimingCanvas.instance.SetReticle ( ReticleType.Off );
            }

            if (currentWeaponObject != null)
            {
                currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( currentWeaponData.holsterBodyPart ) );
                currentWeaponData.holsterData.SetLocal ( currentWeaponObject.transform );
            }

            onWeaponHolstered?.Invoke ( currentWeaponData );

            if (currentWeaponData.sheatheSoundData.AudioClips.Count > 0)
                SoundEffectManager.Play3D ( currentWeaponData.sheatheSoundData.GetRandom (), AudioMixerGroup.SFX, transform.position + (Vector3.up),minDistance: 1,maxDistance: 5 );

            if (currentWeaponObject != null)
                currentWeaponObject.GetComponent<WeaponObject> ().OnHolstered ();
        }
        else
        {
            if (character.IsAI == false)
            {
                Mouse.SetCursorState ( CursorLockMode.Locked, false );
                AimingCanvas.instance.SetReticle ( ReticleType.CanFire );
            }

            if (currentWeaponObject != null)
            {
                currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( currentWeaponData.activeBodyPart ).Find ( "weapon-holder" ) );
                currentWeaponObject.transform.localPosition = currentWeaponData.offsetPosition;
                currentWeaponObject.transform.localEulerAngles = currentWeaponData.offsetRotation;
                currentWeaponObject.transform.localScale = currentWeaponData.localScale;
            }

            onWeaponUnholstered?.Invoke ( currentWeaponData );

            SoundEffectManager.Play3D ( currentWeaponData.unsheatheSoundData.GetRandom (), AudioMixerGroup.SFX, transform.position + (Vector3.up), minDistance: 1, maxDistance: 5 );

            if (currentWeaponObject != null)
                currentWeaponObject.GetComponent<WeaponObject> ().OnUnholstered ();
        }
    }

    public bool CanEquip ()
    {
        if (currentMeleeAnimationLength > 0 || nextAttackAnimationDelay > 0)
            return false;
        return true;
    }

    public void Equip (WeaponData weaponData, bool bypassEquipCheck = false)
    {
        if (!CanEquip () && !bypassEquipCheck) return;

        if (currentWeaponData)
        {
            Unequip ();
        }

        currentWeaponData = weaponData;
        currentWeaponGunData = weaponData as WeaponGunData;
        currentWeaponMeleeData = weaponData as WeaponMeleeData;

        if (weaponData.prefab != null)
            currentWeaponObject = Instantiate ( weaponData.prefab );
        else currentWeaponObject = null;

        if (currentWeaponObject != null)
        {
            if (isHolstered)
            {
                currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( currentWeaponData.holsterBodyPart ) );
                currentWeaponData.holsterData.SetLocal ( currentWeaponObject.transform );
                currentWeaponObject.GetComponent<WeaponObject> ().OnHolstered ();
            }
            else
            {
                currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( weaponData.activeBodyPart ).Find ( "weapon-holder" ) );
                currentWeaponObject.transform.localPosition = weaponData.offsetPosition;
                currentWeaponObject.transform.localEulerAngles = weaponData.offsetRotation;
                currentWeaponObject.transform.localScale = weaponData.localScale;
                currentWeaponObject.GetComponent<WeaponObject> ().OnUnholstered ();
            }
        }

        onWeaponEquiped?.Invoke ( currentWeaponData );

        // IF IS GUN
        if (currentWeaponData.weaponAttackType == WeaponAttackType.Gun)
        {
            currentAmmo = currentWeaponGunData.clipSize;
            currentReloadTime = 0.0f;
            currentSpinUpTime = 0.0f;
            isFiring = false;

            if (currentWeaponGunData.weaponSoundData.spinUpDelayLoop)
                spinUpDelaySource.clip = currentWeaponGunData.weaponSoundData.spinUpDelayLoop;

            if (currentWeaponGunData.weaponSoundData.spinDownDelayLoop)
                spinDownDelaySource.clip = currentWeaponGunData.weaponSoundData.spinDownDelayLoop;

            currentFireType = currentWeaponGunData.fireTypes[0];
            muzzlePoint = currentWeaponObject.transform.Find ( "muzzle-point" );
        }
    }

    public void Unequip (bool dropWeaponMeshOnGround = false, bool bypassAnimationDelay = false)
    {
        if (currentWeaponData == null) return;

        if ((currentMeleeAnimationLength > 0 || nextAttackAnimationDelay > 0) && bypassAnimationDelay == false)
            return;

        WeaponData w = currentWeaponData;
        currentWeaponData = null;
        currentWeaponGunData = null;
        currentWeaponMeleeData = null;

        if (currentWeaponObject)
        {
            if (dropWeaponMeshOnGround)
            {
                currentWeaponObject.transform.SetParent ( null );
                currentWeaponObject.GetComponentInChildren<MeshRenderer> ().gameObject.AddComponent<MeshCollider> ().convex = true;
                currentWeaponObject.AddComponent<Rigidbody> ().AddForce ( Vector3.up * 2.0f, ForceMode.VelocityChange );
            }
            else
            {
                Destroy ( currentWeaponObject );
            }
        }

        onWeaponUnequiped?.Invoke ( w );

        if (character.IsAI == false)
            Equip ( EntityManager.instance.FistsWeaponData, true );
    }

    public void BreakCurrentWeapon ()
    {
        if (currentWeaponData == null || !currentWeaponData.isBreakable || !ItemDatabase.ItemExists ( currentWeaponData.weaponItemID ))
        {
            return;
        }

        isFiring = false;
        currentBurstCooldown = 0.0f;
        currentBurstCounter = 0;

        MessageBox.AddMessage ( "Your weapon has broken and will need to be repaired.", MessageBox.Type.Error );

        WeaponData data = currentWeaponData;
        SetHolsterState ( true, true );
        character.cGear.SetWeaponIndexNull ();
        Unequip ( false, true );

        SoundEffectManager.Play ( AudioClipAsset.WeaponBreak, AudioMixerGroup.SFX );

        if (EntityManager.instance.PlayerInventory.CheckCanRecieveItem ( data.brokenVariantItemID, 1 ))
        {
            EntityManager.instance.PlayerInventory.AddItem ( data.brokenVariantItemID );
        }
        else
        {
            EntityManager.instance.PlayerBankInventory.AddItem ( data.brokenVariantItemID );
            MessageBox.AddMessage ( "Your broken weapon has been sent to your bank.", MessageBox.Type.Info );
        }
    }

    #region Melee
    private MeleeAttackAnimation previousGernericAttackAnimation;
    private MeleeAttackAnimation previousSpecialAttackAnimation;
    private MeleeAttackComboAnimation previousComboAttackAnimation;

    private void CheckMeleeAttack ()
    {
        if (character.IsAI) return;
        if (Mouse.Down ( 0 ) && !isHolstered && nextAttackAnimationDelay <= 0.0f && !character.isDead)
        {
            MeleeAttack ();
        }
    }

    private void CheckMeleeAttackDelay ()
    {
        if (currentMeleeAnimationLength > 0)
        {
            currentMeleeAnimationLength -= Time.deltaTime;
            ShouldAim = true;

            if (currentMeleeAnimationLength <= 0)
            {
                currentMeleeAnimationLength = 0.0f;
                shouldAimCooldown = 1.5f;
                EntityManager.instance.CameraController.SetCinematicComboView ( null );
            }
        }        

        if (nextAttackAnimationDelay > 0)
        {
            nextAttackAnimationDelay -= Time.deltaTime;

            if (nextAttackAnimationDelay <= 0)
            {
                nextAttackAnimationDelay = 0.0f;
            }
        }
    }

    private void MeleeAttack ()
    {
        float attackTypeRoll = UnityEngine.Random.value;

        EntityManager.instance.CameraController.SetCinematicComboView ( null );
        currentMeleeAnimationLength = 0;

        //NPC potentialTarget = GetPotentialMeleeHitNPC ();

        List<NPC> potentialNPCs = new List<NPC> ();

        if(GetMeleeHitData(out potentialNPCs ))
        {
            //if (potentialTarget.NPCAttackOption == NPCAttackOption.CannotBeAttack)
            if (potentialNPCs.Exists ( x => x.NPCAttackOption == NPCAttackOption.CannotBeAttack ))
            {
                DoGenericMeleeAttack ( true );
            }
            else
            {
                //if (Input.GetKey ( KeyCode.LeftAlt ))
                //{
                //    DoSpecialMeleeAttack ();
                //    Debug.Log ( "Special: Debug" );
                //    return;
                //}

                //if (Input.GetKey ( KeyCode.LeftControl ))
                //{
                //    comboNPCTarget = potentialNPCs[0];
                //    DoComboMeleeAttack ();
                //    Debug.Log ( "Combo: Debug" );
                //    return;
                //}
                if (attackTypeRoll <= SkillModifiers.MeleeSpecialChance)
                {
                    // Attack is a special attack
                    DoSpecialMeleeAttack ();
                }
                else
                {
                    attackTypeRoll -= SkillModifiers.MeleeSpecialChance;

                    if (attackTypeRoll <= SkillModifiers.MeleeComboChance)
                    {
                        // Attack is a combo attack
                        comboNPCTarget = potentialNPCs[0];
                        DoComboMeleeAttack ();
                    }
                    else
                    {
                        // Attack is a generic attack
                        DoGenericMeleeAttack ();
                    }
                }
            }
        }
        else
        {
            DoGenericMeleeAttack ();
        }

        //if (potentialTarget == null)
        //{
        //    DoGenericMeleeAttack ();
        //    Debug.Log ( "Generic: Null Target" );
        //}
        //else
        //{
        //    if (potentialTarget.NPCAttackOption == NPCAttackOption.CannotBeAttack)
        //    {
        //        DoGenericMeleeAttack ( true );
        //        Debug.Log ( "Generic: Unattackable Target" );
        //    }
        //    else
        //    {
        //        if (Input.GetKey ( KeyCode.LeftAlt ))
        //        {
        //            DoSpecialMeleeAttack ();
        //            Debug.Log ( "Special: Debug" );
        //            return;
        //        }

        //        if (Input.GetKey ( KeyCode.LeftControl ))
        //        {
        //            comboNPCTarget = potentialTarget;
        //            DoComboMeleeAttack ();
        //            Debug.Log ( "Combo: Debug" );
        //            return;
        //        }
        //        if (attackTypeRoll <= SkillModifiers.MeleeSpecialChance)
        //        {
        //            // Attack is a special attack
        //            DoSpecialMeleeAttack ();
        //            Debug.Log ( "Special: Chance" );
        //        }
        //        else
        //        {
        //            attackTypeRoll -= SkillModifiers.MeleeSpecialChance;

        //            if (attackTypeRoll <= SkillModifiers.MeleeComboChance)
        //            {
        //                // Attack is a combo attack
        //                comboNPCTarget = potentialTarget;
        //                DoComboMeleeAttack ();
        //                Debug.Log ( "Combo: Chance" );
        //            }
        //            else
        //            {
        //                // Attack is a generic attack
        //                DoGenericMeleeAttack ();
        //                Debug.Log ( "Generic: Chance" );
        //            }
        //        }
        //    }
        //}
    }

    public enum MeleeAttackType { Generic, Special, Combo }

    private IEnumerator DoAttackDelayed(float delay, MeleeAttackType attackType, bool mustMiss = false)
    {
        yield return new WaitForSeconds ( delay );
        List<NPC> hitNPCs = new List<NPC> ();

        switch (attackType)
        {
            case MeleeAttackType.Generic:
                if (GetMeleeHitData ( out hitNPCs ) && DetermineMeleeHitChance() && !mustMiss)
                {
                    PerformHit ( hitNPCs.ToArray () );
                }
                else
                {
                    PerformMiss ();
                }
                break;
            case MeleeAttackType.Special:
                if (GetMeleeHitData ( out hitNPCs ))
                {
                    PerformHit ( hitNPCs.ToArray () );
                }
                else
                {
                    PerformMiss ();
                }
                break;
            case MeleeAttackType.Combo:
                if (comboNPCTarget != null)
                {
                    PerformHit ( comboNPCTarget );
                }
                else
                {
                    PerformMiss ();
                }
                break;
        }

    }

    private void DoGenericMeleeAttack (bool mustMiss = false)
    {
        MeleeAttackAnimation selectedClip = GetComponent<CharacterAnimator> ().RandomiseAttackAnimation ( currentWeaponMeleeData.genericAnimationClips, previousGernericAttackAnimation );
        previousGernericAttackAnimation = selectedClip;
        GetComponent<Animator> ().SetTrigger ( "meleeAttack" );
        character.cMovement.SnapCharacterRotationToCamera ();

        SoundEffectManager.Play3D ( currentWeaponMeleeData.swingSoundData.GetRandom (), AudioMixerGroup.SFX, character.Animator.GetBoneTransform(currentWeaponData.activeBodyPart).position, minDistance: 1, maxDistance: 5, delay: selectedClip.swingAudioDelay / 60.0f );
        StartCoroutine ( DoAttackDelayed ( selectedClip.resultAudioDelay / 60.0f, MeleeAttackType.Generic, mustMiss ) );
        nextAttackAnimationDelay = Mathf.Min ( (selectedClip.resultAudioDelay / 60.0f) + 0.25f, selectedClip.clipLength / 60.0f );
        //nextAttackAnimationDelay = Mathf.Max ( 0.5f, Mathf.Min ( (selectedClip.resultAudioDelay / 60.0f) + 0.5f, selectedClip.clipLength / 60.0f ) );
        currentMeleeAnimationLength = selectedClip.clipLength / 60.0f;
    }

    private void DoSpecialMeleeAttack ()
    {
        MeleeAttackAnimation selectedClip = GetComponent<CharacterAnimator> ().RandomiseAttackAnimation ( currentWeaponMeleeData.specialAnimationClips, previousSpecialAttackAnimation );
        previousSpecialAttackAnimation = selectedClip;
        GetComponent<Animator> ().SetTrigger ( "meleeAttack" );
        character.cMovement.SnapCharacterRotationToCamera ();

        SoundEffectManager.Play3D ( currentWeaponMeleeData.swingSoundData.GetRandom (), AudioMixerGroup.SFX, character.Animator.GetBoneTransform ( currentWeaponData.activeBodyPart ).position, minDistance: 1, maxDistance: 5, delay: selectedClip.swingAudioDelay / 60.0f );
        StartCoroutine ( DoAttackDelayed ( selectedClip.resultAudioDelay / 60.0f, MeleeAttackType.Special ) );
        nextAttackAnimationDelay = Mathf.Min ( (selectedClip.resultAudioDelay / 60.0f) + 0.25f, selectedClip.clipLength / 60.0f );
        currentMeleeAnimationLength = selectedClip.clipLength / 60.0f;

        character.FloatingTextIndicator.CreateInfoText ( "SPECIAL", ColourDescription.White );
    }

    private void DoComboMeleeAttack ()
    {
        if (comboNPCTarget == null) return;
        comboNPCTarget.Character.SetCanMove ( false );
        character.SetCanMove ( false );
        IsComboMode = true;

        comboNPCTarget.transform.position = transform.position + (transform.forward * 1.25f);
        Vector3 dir = transform.position - comboNPCTarget.transform.position;
        dir.y = 0;

        if(dir != Vector3.zero)
        {
            comboNPCTarget.transform.rotation = Quaternion.LookRotation ( dir );
        }

        MeleeAttackComboAnimation selectedClip = GetComponent<CharacterAnimator> ().RandomiseAttackAnimation ( currentWeaponMeleeData.comboAnimationClips, previousComboAttackAnimation );
        previousComboAttackAnimation = selectedClip;
        GetComponent<Animator> ().SetTrigger ( "meleeAttack" );
        character.cMovement.SnapCharacterRotationToCamera ();

        for (int i = 0; i < selectedClip.data.Count; i++)
        {
            SoundEffectManager.Play3D ( currentWeaponMeleeData.swingSoundData.GetRandom (), AudioMixerGroup.SFX, character.Animator.GetBoneTransform ( currentWeaponData.activeBodyPart ).position, minDistance: 1, maxDistance: 5, delay: selectedClip.data[i].swingAudioDelay / 60.0f );
            StartCoroutine ( DoAttackDelayed ( selectedClip.data[i].resultAudioDelay / 60.0f, MeleeAttackType.Combo ) );
            //Invoke ( nameof ( PerformComboHit ), selectedClip.data[i].resultAudioDelay / 60.0f );
        }


        Invoke ( nameof ( FinishComboMode ), selectedClip.clipLength / 60.0f );

        nextAttackAnimationDelay = Mathf.Min ( (selectedClip.data[selectedClip.data.Count - 1].resultAudioDelay / 60.0f) + 0.25f, selectedClip.clipLength / 60.0f );
        currentMeleeAnimationLength = selectedClip.clipLength / 60.0f;
        character.FloatingTextIndicator.CreateInfoText ( "COMBO", ColourDescription.White );
        EntityManager.instance.CameraController.SetCinematicComboView ( comboViews.GetRandom () );
    }

    private bool DetermineMeleeHitChance ()
    {
        if (UnityEngine.Random.value <= SkillModifiers.MeleeHitChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [SerializeField] private float meleeStrikeDistance;
    public float MeleeStrikeDistance { get => meleeStrikeDistance; set => meleeStrikeDistance = value; }

    RaycastHit[] attackSphereHit;
    public NPC comboNPCTarget { get; protected set; }

    private void FinishComboMode ()
    {
        if (comboNPCTarget != null)
        {
            comboNPCTarget.Character.SetCanMove ( true );
        }

        IsComboMode = false;
        character.SetCanMove ( true );
    }

    private void PerformHit (params NPC[] hitNPCs)
    {
        OnAttack?.Invoke ( WeaponAttackType.Melee );

        float healthRemoved = 0;

        for (int i = 0; i < hitNPCs.Length; i++)
        {
            if (hitNPCs[i].NPCAttackOption == NPCAttackOption.CannotBeAttack) continue;

            float damage = currentWeaponMeleeData.baseDamage * SkillModifiers.MeleeDamageModifier * GetBaseHitModifier() * SkillModifiers.GLOBAL_PLAYER_DAMAGE_MODIFIER;

            bool isCritical = GetShouldBeCriticalHit ();
            if (isCritical) damage *= GetCriticalHitDamageModifier ();

            if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Sylas)
            {
                float factionBasedReduction = damage * 0.25f;
                damage -= factionBasedReduction;
            }

            if (ItemDatabase.ItemExists ( currentWeaponData.weaponItemID ))
            {
                damage = (ItemDatabase.GetItem ( currentWeaponData.weaponItemID ) as ItemGearWeapon).GetWeaponDamage ( damage, hitNPCs[i] );
                damage = hitNPCs[i].OnBeforeDamagedByWeapon ( damage, currentWeaponData );
            }

            healthRemoved += hitNPCs[i].Health.RemoveHealth ( damage, DamageType.PlayerAttack, isCritical );
        }

        if (character.cFaction.CurrentFaction.factionType == FactionType.Xavix)
        {
            character.Health.AddHealth ( healthRemoved * 0.05f, HealType.FactionEffect );
        }

        SkillManager.instance.AddXpToSkill ( SkillType.Melee, healthRemoved * 0.25f );
        
        SoundEffectManager.Play3D ( currentWeaponMeleeData.hitSoundData.GetRandom (), AudioMixerGroup.SFX, character.Animator.GetBoneTransform ( currentWeaponData.activeBodyPart ).position, minDistance: 1, maxDistance: 5 );
    }

    private void PerformMiss ()
    {
        OnAttack?.Invoke ( WeaponAttackType.Melee );
        SkillManager.instance.AddXpToSkill ( SkillType.Melee, 0.25f );
        SoundEffectManager.Play3D ( currentWeaponMeleeData.missSoundData.GetRandom (), AudioMixerGroup.SFX, character.Animator.GetBoneTransform ( currentWeaponData.activeBodyPart ).position, minDistance: 1, maxDistance: 5 );
        DamageCanvas.instance.SpawnInfoText ( character.Animator.GetBoneTransform ( currentWeaponData.activeBodyPart ), 1.0f, "Miss", ColourDescription.MessageBoxError );
    }

    private float GetBaseHitModifier ()
    {
        return UnityEngine.Random.Range ( 0.75f, 1.25f );
    }

    private bool GetShouldBeCriticalHit ()
    {
        float baseLine = UnityEngine.Random.value;
        float modifiedLine = baseLine - (baseLine * TalentManager.instance.GetTalentModifier ( TalentType.BigShot ));

        if(baseLine <= SkillModifiers.CriticalHitChance)
        {
            return true;
        }
        else if(modifiedLine <= SkillModifiers.CriticalHitChance)
        {
            MessageBox.AddMessage ( "You perform a critical hit because of your " + TalentManager.instance.GetTalent ( TalentType.BigShot ).talentData.talentName + " talent." );
            return true;
        }

        return false;
    }

    private float GetCriticalHitDamageModifier ()
    {
        return UnityEngine.Random.Range ( 1.1f, 1.5f );
    }

    private bool GetMeleeHitData (out List<NPC> hitNPCs)
    {
        hitNPCs = new List<NPC> ();
        float playerToCameraDistance = (EntityManager.instance.PlayerCharacter.transform.position - EntityManager.instance.MainCamera.transform.parent.position).magnitude - 0.1f;
        Vector3 origin = EntityManager.instance.MainCamera.transform.parent.position + (EntityManager.instance.MainCamera.transform.parent.forward * playerToCameraDistance);
        attackSphereHit = new RaycastHit[0];
        attackSphereHit = Physics.SphereCastAll ( origin, 0.25f, EntityManager.instance.MainCamera.transform.parent.forward, meleeStrikeDistance );

        bool hit = false;

        for (int i = 0; i < attackSphereHit.Length; i++)
        {
            if (attackSphereHit[i].collider.gameObject.CompareTag ( "NPC" ))
            {
                hitNPCs.Add ( attackSphereHit[i].collider.gameObject.GetComponent<NPC> () );
                hit = true;
            }
        }

        return hit;
    }

    //private NPC GetPotentialMeleeHitNPC ()
    //{
    //    attackSphereHit = new RaycastHit[0];
    //    attackSphereHit = Physics.SphereCastAll ( EntityManager.instance.MainCamera.transform.parent.position, 0.25f, EntityManager.instance.MainCamera.transform.parent.forward, meleeStrikeDistance );

    //    for (int i = 0; i < attackSphereHit.Length; i++)
    //    {
    //        if (attackSphereHit[i].collider.gameObject.CompareTag ( "NPC" ))
    //        {
    //            return attackSphereHit[i].collider.gameObject.GetComponent<NPC> ();
    //        }
    //    }

    //    return null;
    //}
    #endregion

    #region Gun
    public void CheckAimPosition ()
    {
        if (currentWeaponData == null) return;

        if (character.IsAI)
        {
            if (EntityManager.instance) AimPosition = EntityManager.instance.PlayerCharacter.Animator.GetBoneTransform ( HumanBodyBones.Head ).position;
        }
        else
        {
            if (!character.IsAiming && isHolstered)
            {
                return;
            }

            bool setAimPosition = false;
            bool setHit = false;

            if (character.IsAiming || !isHolstered)
            {
                Ray ray = new Ray ( character.cCameraController.cameraTransform.position, character.cCameraController.cameraTransform.forward );
                RaycastHit[] hits;

                hits = Physics.RaycastAll ( ray, currentWeaponGunData.maxDistance );

                hits = hits.OrderBy ( x => x.distance ).ToArray ();

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].distance < minHitDistance) continue;

                    NPC npc = hits[i].collider.gameObject.GetComponent<NPC> ();

                    if (npc == null)
                    {
                        continue;
                    }

                    AimPosition = hits[i].point;
                    setAimPosition = true;

                    npcInSights = npc;
                    aimEnemyHit = hits[i];
                    setHit = true;
                    hasAimHit = true;
                    break;
                }
            }

            if (!setAimPosition)
            {
                AimPosition = character.cCameraController.cameraTransform.position + (character.cCameraController.cameraTransform.forward * 15);
            }

            if (!setHit)
            {
                if (!isHolstered && character.IsAI == false)
                    AimingCanvas.instance.SetReticle ( ReticleType.CanFire );
                npcInSights = null;
                aimEnemyHit = new RaycastHit ();
                hasAimHit = false;
            }
            else
            {
                if (npcInSights?.NPCAttackOption == NPCAttackOption.CannotBeAttack)
                {
                    if (!isHolstered && character.IsAI == false)
                        AimingCanvas.instance.SetReticle ( ReticleType.CantFire );
                }
            }
        }
    }

    [SerializeField] private float weaponUIOffset = 0.35f;

    private void SetWeaponUI ()
    {
        if (character.IsAI) return;

        if (currentWeaponData == null || character.currentState != Character.State.Standing || currentWeaponObject == null)
        {
            clipText.text = "";
        }
        else
        {
            //Vector3 position = currentWeaponObject.transform.TransformPoint ( currentWeaponGunData.clipTextLocalPosition );
            Vector3 position = currentWeaponObject.transform.position + (transform.right * weaponUIOffset);

            if (isHolstered)
            {
                clipText.text = "[H]";
                //position = currentWeaponObject.transform.TransformPoint ( currentWeaponGunData.clipTextHolsteredLocalPosition );
            }
            else
            {
                if (currentWeaponData.weaponAttackType == WeaponAttackType.Melee)
                {
                    clipText.text = "";
                }
                else
                {
                    if (character.IsAiming)
                    {
                        if (currentReloadTime > 0.0f)
                        {
                            clipText.text = "-----";
                        }
                        else
                        {
                            clipText.text = currentAmmo.ToString ( "00" ) + " / " + currentWeaponGunData.clipSize.ToString ( "00" );
                        }
                    }
                    else
                    {
                        clipText.text = "";

                    }
                }
                
            }

            clipText.transform.position = position;
        }

    }

    private void CheckFireInput ()
    {
        if (character.IsAI) return;
        if (character.isDead)
        {
            ShouldAim = false;
            return;
        }
        if (isHolstered || currentWeaponData == null) { isFiring = false; return; }
        if (EventSystem.current.IsPointerOverGameObject ()) return;

        if (Input.GetMouseButtonDown ( 0 ))
        {
            if(currentWeaponGunData.spinUpDelaySeconds > 0)
            {
                if(currentSpinUpTime < currentWeaponGunData.spinUpDelaySeconds)
                {
                    // TODO start playing spin up delay sound
                    // TODO STOP playing spin down sound
                }
            }
        }

        if (currentFireType == WeaponGunData.FireType.Single || currentFireType == WeaponGunData.FireType.Burst)
        {
            if (Input.GetMouseButtonDown ( 0 ))
            {
                TryFire ();

                ShouldAim = true;
                shouldAimCooldown = 1.0f;
            }
        }
        else
        {
            if (Input.GetMouseButton ( 0 ))
            {
                TryFire ();

                ShouldAim = true;
                shouldAimCooldown = 1.0f;
            }
        }
       
        if(!Input.GetMouseButton (0))
        {
            if (currentFireType == WeaponGunData.FireType.Burst && currentBurstCounter != 3 && currentBurstCounter != 0)
            {
                ShouldAim = true;
                shouldAimCooldown = 1.0f;
                return;
            }

            isFiring = false;

            if (currentSpinUpTime > 0)
                currentSpinUpTime -= Time.deltaTime / currentWeaponGunData.spinDownDelaySeconds;

            if (currentSpinUpTime < 0) currentSpinUpTime = 0;
        }

        if (Input.GetMouseButtonUp ( 0 ))
        {
            if (currentWeaponGunData.spinUpDelaySeconds > 0)
            {
                if (currentSpinUpTime > 0.0f)
                {
                    // TODO STOP playing spin up delay sound
                    // TODO START playing spin down sound
                }
            }
        }
    }

    private void TryFire ()
    {
        if (character.IsAI)
        {
            if (isFiring) return;
            if (fireCooldown > 0.0f) return;
            if (currentFireType == WeaponGunData.FireType.Burst && currentBurstCooldown > 0.0f) return;
            if (currentReloadTime > 0.0f) { isFiring = false; return; }
            if (character.isDead) { isFiring = false; return; }

            if (currentSpinUpTime < currentWeaponGunData.spinUpDelaySeconds) { currentSpinUpTime += Time.deltaTime; return; }
            if (currentSpinUpTime > currentWeaponGunData.spinUpDelaySeconds)
            {
                // TODO stop playing spin up audio loop
                // TODO stop playing spin down audio loop
                currentSpinUpTime = currentWeaponGunData.spinUpDelaySeconds;
            }

            isFiring = true;
        }
        else
        {
            if (isFiring) return;
            if (fireCooldown > 0.0f) return;
            if (currentFireType == WeaponGunData.FireType.Burst && currentBurstCooldown > 0.0f) return;
            if (currentReloadTime > 0.0f) { isFiring = false; return; }
            if (!character.IsAiming) return;
            if (character.isDead) { isFiring = false; return; }

            if (currentSpinUpTime < currentWeaponGunData.spinUpDelaySeconds) { currentSpinUpTime += Time.deltaTime; return; }
            if (currentSpinUpTime > currentWeaponGunData.spinUpDelaySeconds)
            {
                // TODO stop playing spin up audio loop
                // TODO stop playing spin down audio loop
                currentSpinUpTime = currentWeaponGunData.spinUpDelaySeconds;
            }

            isFiring = true;
        }
    }

    private void CheckIsFiring ()
    {
        if (fireCooldown > 0.0f)
        {
            fireCooldown -= Time.deltaTime;

            if (fireCooldown <= 0.0f)
                fireCooldown = 0.0f;

            return;
        }

        if (currentReloadTime > 0.0f) { isFiring = false; return; }

        if(character.IsAI)
        {
            if (isFiring)
            {
                NPC_Fire ();
            }
        }
        else
        {
            if (isFiring)
            {
                Fire ();
            }
        }
 
    }

    private void CheckBurstCooldown ()
    {
        if (currentBurstCooldown > 0.0f)
        {
            currentBurstCooldown -= Time.deltaTime;

            if (currentBurstCooldown <= 0.0f)
                currentBurstCooldown = 0.0f;

            return;
        }
    }

    private void CheckReload ()
    {       
        if(currentReloadTime > 0.0f)
        {
            currentReloadTime -= Time.deltaTime;

            if(currentReloadTime <= 0.0f)
            {
                currentReloadTime = 0.0f;
                currentAmmo = currentWeaponGunData.clipSize;
                SoundEffectManager.Play3D ( currentWeaponGunData.weaponSoundData.audioClipReloadFinished.GetRandom (), AudioMixerGroup.SFX, character.Animator.GetBoneTransform ( currentWeaponData.activeBodyPart ).position, minDistance: 1, maxDistance: 10 );
            }
        }
        else
        {
            if (!character.IsAI && character.currentVehicle == null)
            {
                if (Input.GetKeyDown ( KeyCode.R ))
                {
                    Reload ();
                }
            }
        }
    }

    public void Reload ()
    {
        if (currentReloadTime > 0.0f) return;
        if (isHolstered || currentWeaponData == null) { return; }
        currentReloadTime = currentWeaponGunData.reloadTime - (currentWeaponGunData.reloadTime * TalentManager.instance.GetTalentModifier ( TalentType.SleightOfHand ));
        SoundEffectManager.Play ( currentWeaponGunData.weaponSoundData.audioClipReload.GetRandom (), AudioMixerGroup.SFX );
    }

    public void Fire ()
    {
        if (character.isDead) { isFiring = false; }
        if (!isFiring) return;

        if (npcInSights != null && npcInSights.NPCAttackOption == NPCAttackOption.CannotBeAttack)
        {
            isFiring = false;
            return;
        }

        OnAttack?.Invoke ( WeaponAttackType.Gun );

        if (currentAmmo > 0)
        {
            if (currentWeaponData.isBreakable)
            {
                if (UnityEngine.Random.value <= (1 / currentWeaponGunData.fireRate) * ((currentWeaponData.weaponAttackType == WeaponAttackType.Melee) ? 0.5f: 1.0f))
                {
                    if (UnityEngine.Random.value <= TalentManager.instance.GetTalentModifier ( TalentType.Unbreakable ))
                    {
                        MessageBox.AddMessage ( "Your weapon almost broke, but your " + TalentManager.instance.GetTalent ( TalentType.Unbreakable ).talentData.talentName + " talent saved it." );
                    }
                    else
                    {
                        BreakCurrentWeapon ();
                        return;
                    }
                }
            }

            SoundEffectManager.Play3D ( currentWeaponGunData.weaponSoundData.audioClipFire.GetRandom (), AudioMixerGroup.SFX, muzzlePoint.position, volume: 0.3f, minDistance: 5, maxDistance: 50 );
            character.cIK.AddRecoil ( currentWeaponGunData.recoilData );

            if(UnityEngine.Random.value <= TalentManager.instance.GetTalentModifier ( TalentType.SecondChance ))
            {
                MessageBox.AddMessage ( "Your bullet is not consumed because of your " + TalentManager.instance.GetTalent ( TalentType.SecondChance ).talentData.talentName + " talent." );
            }
            else
            {
                currentAmmo--;
            }

            RayBullet ();

            if (UnityEngine.Random.value <= TalentManager.instance.GetTalentModifier ( TalentType.EagleEye ))
            {
                RayBullet ();
                MessageBox.AddMessage ( "You performed an extra hit on your enemy because of your " + TalentManager.instance.GetTalent ( TalentType.EagleEye ).talentData.talentName + " talent." );
            }

            GameObject ps = Instantiate ( currentWeaponGunData.muzzlePrefab );
            ps.transform.SetParent ( muzzlePoint );
            ps.transform.localPosition = Vector3.zero;
            ps.transform.localRotation = Quaternion.identity;
        }
        else
        {
            SoundEffectManager.Play3D ( currentWeaponGunData.weaponSoundData.audioClipEmptyFire.GetRandom (), AudioMixerGroup.SFX, muzzlePoint.position, minDistance: 2, maxDistance: 10 );
            character.cIK.AddRecoil ( currentWeaponGunData.recoilData );
        }

        fireCooldown = 60.0f / currentWeaponGunData.fireRate;

        switch (currentFireType)
        {
            case WeaponGunData.FireType.Single:
                isFiring = false;
                break;

            case WeaponGunData.FireType.Burst:

                currentBurstCounter++;
                if (currentBurstCounter == 3)
                {
                    isFiring = false;
                    currentBurstCooldown = 60.0f / currentWeaponGunData.burstDelay;
                    currentBurstCounter = 0;
                }

                break;

            case WeaponGunData.FireType.Auto:
                if (!Input.GetMouseButton ( 0 ))
                {
                    isFiring = false;
                }
                break;
        }
    }

    private void RayBullet ()
    {
        if (hasAimHit)
        {
            if(npcInSights != null)
            {
                if(npcInSights.NPCAttackOption == NPCAttackOption.CannotBeAttack)
                {
                    return;
                }
                else
                {
                    if (aimEnemyHit.distance > currentWeaponGunData.maxDistance) return;

                    float damageFalloff = currentWeaponGunData.damageByDistanceFalloff.Evaluate ( Mathf.InverseLerp ( 0.0f, currentWeaponGunData.maxDistance, aimEnemyHit.distance ) );
                    float baseDamage = currentWeaponData.baseDamage * SkillModifiers.ShootingDamageModifier * GetBaseHitModifier () * SkillModifiers.GLOBAL_PLAYER_DAMAGE_MODIFIER * damageFalloff;

                    bool isCritical = GetShouldBeCriticalHit ();
                    if (isCritical) baseDamage *= GetCriticalHitDamageModifier ();

                    if (EntityManager.instance.PlayerCharacter.cFaction.CurrentFaction.factionType == FactionType.Sylas)
                    {
                        float factionBasedReduction = baseDamage * 0.25f;
                        baseDamage -= factionBasedReduction;
                    }

                    if (ItemDatabase.ItemExists ( currentWeaponData.weaponItemID ))
                    {
                        baseDamage = (ItemDatabase.GetItem ( currentWeaponData.weaponItemID ) as ItemGearWeapon).GetWeaponDamage ( baseDamage, npcInSights );
                        baseDamage = npcInSights.OnBeforeDamagedByWeapon ( baseDamage, currentWeaponData );
                    }

                    float healthRemoved = npcInSights.Health.RemoveHealth ( baseDamage, DamageType.PlayerAttack, isCritical );

                    if(character.cFaction.CurrentFaction.factionType == FactionType.Xavix)
                    {
                        character.Health.AddHealth ( healthRemoved * 0.05f, HealType.FactionEffect );
                    }

                    GameObject go = Instantiate ( ricochetPrefab );
                    go.transform.position = aimEnemyHit.point;
                    go.transform.rotation = Quaternion.LookRotation ( -character.cCameraController.cameraTransform.forward );

                    SkillManager.instance.AddXpToSkill ( SkillType.Shooting, healthRemoved * 0.25f );
                    return;
                }
            }         
        }

        SkillManager.instance.AddXpToSkill ( SkillType.Shooting, 0.25f );
    }

    public void NPC_StopFire ()
    {
        isFiring = false;
    }

    public bool NPC_TryFire ()
    {
        if (currentAmmo <= 0) return false;

        TryFire ();

        ShouldAim = true;
        shouldAimCooldown = 1.0f;

        return true;
    }

    private void NPC_Fire ()
    {
        if (character.isDead) { isFiring = false; }

        OnAttack?.Invoke ( WeaponAttackType.Gun );

        if (currentAmmo > 0)
        {
            SoundEffectManager.Play3D ( currentWeaponGunData.weaponSoundData.audioClipFire.GetRandom (), AudioMixerGroup.SFX, muzzlePoint.position, volume: 0.3f, minDistance: 5,maxDistance: 50 );
            character.cIK.AddRecoil ( currentWeaponGunData.recoilData );
            currentAmmo--;
            NPC_Bullet ();

            GameObject ps = Instantiate ( currentWeaponGunData.muzzlePrefab );
            ps.transform.SetParent ( muzzlePoint );
            ps.transform.localPosition = Vector3.zero;
            ps.transform.localRotation = Quaternion.identity;
        }
        else
        {
            SoundEffectManager.Play3D ( currentWeaponGunData.weaponSoundData.audioClipEmptyFire.GetRandom (), AudioMixerGroup.SFX, muzzlePoint.position, minDistance: 2, maxDistance: 10 );
            character.cIK.AddRecoil ( currentWeaponGunData.recoilData );
        }

        fireCooldown = 60.0f / currentWeaponGunData.fireRate;

        switch (currentFireType)
        {
            case WeaponGunData.FireType.Single:
                isFiring = false;
                break;

            case WeaponGunData.FireType.Burst:

                currentBurstCounter++;
                if (currentBurstCounter == 3)
                {
                    isFiring = false;
                    currentBurstCooldown = 60.0f / currentWeaponGunData.burstDelay;
                    currentBurstCounter = 0;
                }

                break;

            case WeaponGunData.FireType.Auto:
                break;
        }
    }

    private void NPC_Bullet ()
    {
        NPC npc = GetComponent<NPC> ();

        if (UnityEngine.Random.value <= NPCCombatStats.GetGunHitChance ( npc.NpcData ))
        {
            float damage = NPCCombatStats.GetGunDamageOutput ( npc.NpcData ) * UnityEngine.Random.Range ( 0.9f, 1.1f );
            EntityManager.instance.PlayerCharacter.Health.RemoveHealth ( damage, DamageType.EnemyAttack );
        }
    }

    public void ToggleFireType ()
    {
        int index = currentWeaponGunData.fireTypes.IndexOf ( currentFireType );
        index++;

        if (index >= currentWeaponGunData.fireTypes.Count)
        {
            index = 0;
        }

        currentFireType = currentWeaponGunData.fireTypes[index];
    }
    #endregion
}
