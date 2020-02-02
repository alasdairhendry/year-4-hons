﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterWeapon : MonoBehaviour
{
    public Character character { get; protected set; }

    public WeaponData currentWeaponData { get; protected set; }
    public GameObject currentWeaponObject { get; protected set; }
    public WeaponData.FireType currentFireType;/*{ get; protected set; }*/
    public Transform muzzlePoint { get; protected set; }

    [Range(0.0f, 1.0f)] public float fireCooldown = 0.0f;
    public bool isFiring = false;
    public float currentAmmo = 0;
    public float currentReloadTime = 0.0f;

    public bool isHolstered { get; protected set; }

    public bool isEquipped { get { return currentWeaponData != null; } }
    public bool weaponIsPistol { get { if (currentWeaponData == null) return false; return currentWeaponData.weaponType == WeaponData.WeaponType.Pistol; } }
    public TransformData GetCurrentIKData { get { if (currentWeaponData == null) return null; return (character.currentVehicle != null) ? currentWeaponData.inVehicleIkData : currentWeaponData.ikData; } }
    public Transform WeaponHandTarget
    {
        get
        {
            if (currentWeaponData == null) return null;
            if (currentWeaponObject == null) return null;
            return currentWeaponObject.transform.Find ( "hand-ik-target" );
        }
    }

    [SerializeField] private List<WeaponData> allWeaponData = new List<WeaponData> ();

    public System.Action<WeaponData> onWeaponEquiped;
    public System.Action<WeaponData> onWeaponUnequiped;

    public System.Action<WeaponData> onWeaponHolstered;
    public System.Action<WeaponData> onWeaponUnholstered;

    [SerializeField] private TextMeshProUGUI clipText;

    public bool ShouldAim { get; protected set; }
    private float shouldAimCooldown = 1.0f;

    [SerializeField] private float minHitDistance = 2.0f;

    public Vector3 AimPosition = new Vector3 ();
    public RaycastHit aimEnemyHit;
    public NPC npcInSights = null;
    public bool hasAimHit = false;

    private int currentBurstCounter = 0;
    private float currentBurstCooldown = 0.0f;

    private AudioSource spinUpDelaySource;
    private AudioSource spinDownDelaySource;

    [SerializeField] private GameObject ricochetPrefab;
    private float currentSpinUpTime = 0.0f;

    private void Awake ()
    {
        character = GetComponent<Character> ();
        if (allWeaponData.Count > 0)
            Equip ( allWeaponData[0] );
        SetHolsterState ( true );

        spinUpDelaySource = SoundEffect.Create ( this, 1.0f, parent: this.transform );
        spinDownDelaySource = SoundEffect.Create ( this, 1.0f, parent: this.transform );
    }

    private void Update ()
    {
        if (character.IsAI) return;

        CheckFireInput ();
        CheckIsFiring ();
        CheckBurstCooldown ();
        CheckReload ();    

        CheckAimPosition ();
        CheckShouldAim ();
        CheckWeaponSwitch ();

        SetWeaponUI ();

        if (Input.GetKeyDown ( KeyCode.H ))
        {
            SetHolsterState ( !isHolstered );
        }
    }

    public void CheckAimPosition ()
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

            hits = Physics.RaycastAll ( ray, currentWeaponData.maxDistance );

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
            if (!isHolstered)
                AimingCanvas.instance.SetReticle ( ReticleType.CanFire );
            npcInSights = null;
            aimEnemyHit = new RaycastHit ();
            hasAimHit = false;
        }
        else
        {
            if (npcInSights?.HostilityLevel == HostilityLevel.Passive)
            {
                if (!isHolstered)
                    AimingCanvas.instance.SetReticle ( ReticleType.CantFire );
            }
        }
    }

    private void SetWeaponUI ()
    {
        if (currentWeaponData == null || character.currentState != Character.State.Standing)
        {
            clipText.text = "";
        }
        else
        {
            Vector3 position = currentWeaponObject.transform.TransformPoint ( currentWeaponData.clipTextLocalPosition );

            if (isHolstered)
            {
                clipText.text = "[H]";
                position = currentWeaponObject.transform.TransformPoint ( currentWeaponData.clipTextHolsteredLocalPosition );
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
                        clipText.text = currentAmmo.ToString ( "00" ) + " / " + currentWeaponData.clipSize.ToString ( "00" );
                    }
                }
                else
                {
                    clipText.text = "";

                }
            }

            clipText.transform.position = position;
        }

    }

    private void CheckFireInput ()
    {
        if (isHolstered || currentWeaponData == null) { isFiring = false; return; }
        if (EventSystem.current.IsPointerOverGameObject ()) return;

        if (Input.GetMouseButtonDown ( 0 ))
        {
            if(currentWeaponData.spinUpDelaySeconds > 0)
            {
                if(currentSpinUpTime < currentWeaponData.spinUpDelaySeconds)
                {
                    // TODO start playing spin up delay sound
                    // TODO STOP playing spin down sound
                }
            }
        }

        if (Input.GetMouseButton ( 0 ))
        {
            TryFire ();

            ShouldAim = true;
            shouldAimCooldown = 1.0f;
        }
        else
        {
            if (currentFireType == WeaponData.FireType.Burst && currentBurstCounter != 3 && currentBurstCounter != 0)
            {
                ShouldAim = true;
                shouldAimCooldown = 1.0f;
                Debug.Log ( "wut" );
                return;
            }

            isFiring = false;

            if (currentSpinUpTime > 0)
                currentSpinUpTime -= Time.deltaTime / currentWeaponData.spinDownDelaySeconds;

            if (currentSpinUpTime < 0) currentSpinUpTime = 0;
        }

        if (Input.GetMouseButtonUp ( 0 ))
        {
            if (currentWeaponData.spinUpDelaySeconds > 0)
            {
                if (currentSpinUpTime > 0.0f)
                {
                    // TODO STOP playing spin up delay sound
                    // TODO START playing spin down sound
                }
            }
        }

        //switch (currentFireType)
        //{
        //    case WeaponData.FireType.Single:
        //        if (Input.GetMouseButtonDown ( 0 ))
        //        {
        //            TryFire ();

        //            ShouldAim = true;
        //            shouldAimCooldown = 1.0f;
        //        }
        //        break;

        //    case WeaponData.FireType.Burst:                
        //        if (Input.GetMouseButtonDown ( 0 ))
        //        {
        //            TryFire ();

        //            ShouldAim = true;
        //            shouldAimCooldown = 1.0f;
        //        }
        //        break;

        //    case WeaponData.FireType.Auto:
        //        if (Input.GetMouseButton ( 0 ))
        //        {
        //            TryFire ();

        //            ShouldAim = true;
        //            shouldAimCooldown = 1.0f;
        //        }
        //        else
        //        {
        //            isFiring = false;
        //        }
        //        break;
        //}
    }

    private void TryFire ()
    {
        if (isFiring) return;
        if (fireCooldown > 0.0f) return;
        if (currentFireType == WeaponData.FireType.Burst && currentBurstCooldown > 0.0f) return;
        if (currentReloadTime > 0.0f) { isFiring = false; return; }
        if (!character.IsAiming) return;

        if (currentSpinUpTime < currentWeaponData.spinUpDelaySeconds) { currentSpinUpTime += Time.deltaTime; return; }
        if (currentSpinUpTime > currentWeaponData.spinUpDelaySeconds)
        {
            // TODO stop playing spin up audio loop
            // TODO stop playing spin down audio loop
            currentSpinUpTime = currentWeaponData.spinUpDelaySeconds;
        }

        isFiring = true;
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

        if (isFiring)
        {
            Fire ();
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
                currentAmmo = currentWeaponData.clipSize;
                SoundEffect.Play ( currentWeaponData.weaponSoundData.audioClipReloadFinished.GetRandom (), true );
            }
        }
        else
        {
            if (Input.GetKeyDown ( KeyCode.R ))
            {
                Reload ();
            }
        }
    }

    public void Reload ()
    {
        if (currentReloadTime > 0.0f) return;
        if (isHolstered || currentWeaponData == null) { return; }
        currentReloadTime = currentWeaponData.reloadTime;
        SoundEffect.Play ( currentWeaponData.weaponSoundData.audioClipReload.GetRandom (), true );
    }

    public void Fire ()
    {
        if (!isFiring) return;

        if (npcInSights != null && npcInSights.HostilityLevel == HostilityLevel.Passive)
        {
            isFiring = false;
            return;
        }

        if (currentAmmo > 0)
        {
            SoundEffect.Play ( currentWeaponData.weaponSoundData.audioClipFire.GetRandom (), true );
            character.cIK.AddRecoil ( currentWeaponData.recoilData );
            currentAmmo--;
            RayBullet ();

        }
        else
        {
            SoundEffect.Play ( currentWeaponData.weaponSoundData.audioClipEmptyFire.GetRandom (), true );
            character.cIK.AddRecoil ( currentWeaponData.recoilData );
        }

        fireCooldown = 60.0f / currentWeaponData.fireRate;

        switch (currentFireType)
        {
            case WeaponData.FireType.Single:
                isFiring = false;
                break;

            case WeaponData.FireType.Burst:

                currentBurstCounter++;
                if (currentBurstCounter == 3)
                {
                    isFiring = false;
                    currentBurstCooldown = 60.0f / currentWeaponData.burstDelay;
                    currentBurstCounter = 0;
                }

                break;

            case WeaponData.FireType.Auto:
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
                if(npcInSights.HostilityLevel == HostilityLevel.Passive)
                {
                    return;
                }
                else
                {
                    if (aimEnemyHit.distance > currentWeaponData.maxDistance) return;

                    float distNrm = Mathf.InverseLerp ( 0.0f, currentWeaponData.maxDistance, aimEnemyHit.distance );
                    float baseDamageWithFalloff = currentWeaponData.damageByDistanceFalloff.Evaluate ( distNrm ) * currentWeaponData.baseDamage;

                    npcInSights.Health.RemoveHealth ( baseDamageWithFalloff, DamageType.PlayerAttack );

                    GameObject go = Instantiate ( ricochetPrefab );
                    go.transform.position = aimEnemyHit.point;
                    go.transform.rotation = Quaternion.LookRotation ( -character.cCameraController.cameraTransform.forward );

                    GameObject ps = Instantiate ( currentWeaponData.muzzlePrefab );
                    ps.transform.SetParent ( muzzlePoint );
                    ps.transform.localPosition = Vector3.zero;
                    ps.transform.localRotation = Quaternion.identity;

                    SkillManager.instance.AddXp ( SkillManager.SkillType.Shooting, 10.0f );
                }
            }         
        }
    }

    private void CheckWeaponSwitch ()
    {
        if (Input.GetKeyDown ( KeyCode.Alpha1 ))
        {
            Equip ( allWeaponData[0] );
        }
        if (Input.GetKeyDown ( KeyCode.Alpha2 ))
        {
            Equip ( allWeaponData[1] );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha3 ))
        {
            Equip ( allWeaponData[2] );
        }
        if (Input.GetKeyDown ( KeyCode.Alpha4 ))
        {
            Equip ( allWeaponData[3] );
        }

        if (Input.GetKeyDown ( KeyCode.Alpha5 ))
        {
            Equip ( allWeaponData[4] );
        }
        if (Input.GetKeyDown ( KeyCode.Alpha6 ))
        {
            Equip ( allWeaponData[5] );
        }
    }   

    private void CheckShouldAim ()
    {
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

    public void SetHolsterState (bool state)
    {
        if (!isEquipped) return;
        if (isHolstered == state) return;

        isHolstered = state;

        if (isHolstered)
        {
            Mouse.SetCursorState ( CursorLockMode.None, true );
            currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( currentWeaponData.holsterBodyPart ) );
            currentWeaponData.holsterData.SetLocal ( currentWeaponObject.transform );

            onWeaponHolstered?.Invoke ( currentWeaponData );
            AimingCanvas.instance.SetReticle ( ReticleType.Off );
        }
        else
        {
            Mouse.SetCursorState ( CursorLockMode.Locked, false );
            currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( currentWeaponData.activeBodyPart ).Find ( "weapon-holder" ) );
            currentWeaponObject.transform.localPosition = currentWeaponData.offsetPosition;
            currentWeaponObject.transform.localEulerAngles = currentWeaponData.offsetRotation;
            currentWeaponObject.transform.localScale = currentWeaponData.localScale;

            onWeaponUnholstered?.Invoke ( currentWeaponData );
            AimingCanvas.instance.SetReticle ( ReticleType.CanFire );
        }
    }

    public void Equip (WeaponData weaponData)
    {
        if (currentWeaponData)
        {
            Unequip ();
        }

        currentWeaponData = weaponData;
        currentWeaponObject = Instantiate ( weaponData.prefab );

        if (isHolstered)
        {
            currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( currentWeaponData.holsterBodyPart ) );
            currentWeaponData.holsterData.SetLocal ( currentWeaponObject.transform );
        }
        else
        {
            currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( weaponData.activeBodyPart ).Find ( "weapon-holder" ) );
            currentWeaponObject.transform.localPosition = weaponData.offsetPosition;
            currentWeaponObject.transform.localEulerAngles = weaponData.offsetRotation;
            currentWeaponObject.transform.localScale = weaponData.localScale;
        }

        onWeaponEquiped?.Invoke ( currentWeaponData );

        currentAmmo = currentWeaponData.clipSize;
        currentReloadTime = 0.0f;
        currentSpinUpTime = 0.0f;
        isFiring = false;

        if (currentWeaponData.weaponSoundData.spinUpDelayLoop)
            spinUpDelaySource.clip = currentWeaponData.weaponSoundData.spinUpDelayLoop;

        if (currentWeaponData.weaponSoundData.spinDownDelayLoop)
            spinDownDelaySource.clip = currentWeaponData.weaponSoundData.spinDownDelayLoop;

        currentFireType = currentWeaponData.fireTypes[0];
        muzzlePoint = currentWeaponObject.transform.Find ( "muzzle-point" );
    }

    [NaughtyAttributes.Button]
    public void ToggleFireType ()
    {
        int index = currentWeaponData.fireTypes.IndexOf ( currentFireType );
        index++;

        if(index >= currentWeaponData.fireTypes.Count)
        {
            index = 0;
        }

        currentFireType = currentWeaponData.fireTypes[index];
    }

    [NaughtyAttributes.Button]
    public void Unequip ()
    {
        WeaponData w = currentWeaponData;
        currentWeaponData = null;

        if (currentWeaponObject)
        {
            currentWeaponObject.transform.SetParent ( null );
            currentWeaponObject.transform.GetChild ( 0 ).gameObject.AddComponent<BoxCollider> ();
            currentWeaponObject.AddComponent<Rigidbody> ();
        }

        onWeaponUnequiped?.Invoke ( w );
    }
}
