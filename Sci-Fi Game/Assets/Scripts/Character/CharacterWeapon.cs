using System;
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

    public bool isHolstered { get; protected set; }
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
        //if (character.IsAI) return;

        if(currentWeaponData != null)
        {
            if (currentWeaponData.weaponAttackType == WeaponAttackType.Gun)
            {
                CheckFireInput ();
                CheckIsFiring ();
                CheckBurstCooldown ();
                CheckReload ();
                CheckAimPosition ();
                SetWeaponUI ();
            }
            else
            {
                CheckMeleeAttackDelay ();
                CheckMeleeAttack ();
            }
        }        

        CheckShouldAim ();
        CheckWeaponSwitch ();

        if (character.IsAI) return;

        if (Input.GetKeyDown ( KeyCode.H ))
        {
            SetHolsterState ( !isHolstered );
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

    private void CheckWeaponSwitch ()
    {
        if (character.IsAI) return;
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

    public void SetHolsterState (bool state)
    {
        if (!isEquipped) return;
        if (isHolstered == state) return;

        if (state)
        {
            if (currentMeleeAnimationLength > 0 || nextAttackAnimationDelay > 0)
                return;
        }

        isHolstered = state;

        if (isHolstered)
        {
            Mouse.SetCursorState ( CursorLockMode.None, true );
            currentWeaponObject.transform.SetParent ( GetComponent<Animator> ().GetBoneTransform ( currentWeaponData.holsterBodyPart ) );
            currentWeaponData.holsterData.SetLocal ( currentWeaponObject.transform );

            onWeaponHolstered?.Invoke ( currentWeaponData );
            AimingCanvas.instance.SetReticle ( ReticleType.Off );

            if (currentWeaponData.sheatheSoundData.AudioClips.Count > 0)
                SoundEffect.Play3D ( currentWeaponData.sheatheSoundData.GetRandom (), transform.position + (Vector3.up), 1, 5 );

            currentWeaponObject.GetComponent<WeaponObject> ().OnHolstered ();
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

            SoundEffect.Play3D ( currentWeaponData.unsheatheSoundData.GetRandom (), transform.position + (Vector3.up), 1, 5 );
            currentWeaponObject.GetComponent<WeaponObject> ().OnUnholstered ();
        }
    }

    public void Equip (WeaponData weaponData)
    {
        if (currentMeleeAnimationLength > 0 || nextAttackAnimationDelay > 0)
            return;

        if (currentWeaponData)
        {
            Unequip ();
        }

        currentWeaponData = weaponData;
        currentWeaponGunData = weaponData as WeaponGunData;
        currentWeaponMeleeData = weaponData as WeaponMeleeData;

        currentWeaponObject = Instantiate ( weaponData.prefab );

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

    [NaughtyAttributes.Button]
    public void Unequip (bool dropWeaponMeshOnGround = false)
    {
        if (currentWeaponData == null) return;

        if (currentMeleeAnimationLength > 0 || nextAttackAnimationDelay > 0)
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
                //currentWeaponObject.transform.GetChild ( 0 ).gameObject.AddComponent<BoxCollider> ();
                currentWeaponObject.AddComponent<Rigidbody> ().AddForce ( Vector3.up * 2.0f, ForceMode.VelocityChange );
            }
            else
            {
                Destroy ( currentWeaponObject );
            }
        }

        onWeaponUnequiped?.Invoke ( w );
    }

    #region Melee
    private MeleeAttackAnimation previousGernericAttackAnimation;
    private MeleeAttackAnimation previousSpecialAttackAnimation;
    private MeleeAttackComboAnimation previousComboAttackAnimation;

    private void CheckMeleeAttack ()
    {
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
        float baseSpecialChance = 0.1f;
        float baseComboChance = 0.025f;

        EntityManager.instance.CameraController.SetCinematicComboView ( null );
        currentMeleeAnimationLength = 0;

        NPC potentialTarget = GetPotentialMeleeHitNPC ();

        if (potentialTarget == null)
        {
            DoGenericMeleeAttack ();
        }
        else
        {
            if (attackTypeRoll <= baseSpecialChance)
            {
                // Attack is a special attack
                DoSpecialMeleeAttack ();
            }
            else
            {
                attackTypeRoll -= baseSpecialChance;

                if (attackTypeRoll <= baseComboChance)
                {
                    // Attack is a combo attack
                    comboNPCTarget = potentialTarget;
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

    private void DoGenericMeleeAttack ()
    {
        MeleeAttackAnimation selectedClip = GetComponent<CharacterAnimator> ().RandomiseAttackAnimation ( currentWeaponMeleeData.genericAnimationClips, previousGernericAttackAnimation );
        previousGernericAttackAnimation = selectedClip;
        GetComponent<Animator> ().SetTrigger ( "meleeAttack" );
        character.cMovement.SnapCharacterRotationToCamera ();

        SoundEffect.Play3D ( currentWeaponMeleeData.swingSoundData.GetRandom (), currentWeaponObject.transform.position, 1, 5, delay: selectedClip.swingAudioDelay / 60.0f );
        Invoke ( nameof ( DetermineMeleeHitChance ), selectedClip.resultAudioDelay / 60.0f );
        nextAttackAnimationDelay = Mathf.Min ( (selectedClip.resultAudioDelay / 60.0f) + 0.25f, selectedClip.clipLength / 60.0f );
        currentMeleeAnimationLength = selectedClip.clipLength / 60.0f;
    }

    private void DoSpecialMeleeAttack ()
    {
        MeleeAttackAnimation selectedClip = GetComponent<CharacterAnimator> ().RandomiseAttackAnimation ( currentWeaponMeleeData.specialAnimationClips, previousSpecialAttackAnimation );
        previousSpecialAttackAnimation = selectedClip;
        GetComponent<Animator> ().SetTrigger ( "meleeAttack" );
        character.cMovement.SnapCharacterRotationToCamera ();

        SoundEffect.Play3D ( currentWeaponMeleeData.swingSoundData.GetRandom (), currentWeaponObject.transform.position, 1, 5, delay: selectedClip.swingAudioDelay / 60.0f );
        Invoke ( nameof ( PerformHit ), selectedClip.resultAudioDelay / 60.0f );
        nextAttackAnimationDelay = Mathf.Min ( (selectedClip.resultAudioDelay / 60.0f) + 0.25f, selectedClip.clipLength / 60.0f );
        currentMeleeAnimationLength = selectedClip.clipLength / 60.0f;

        character.FloatingTextIndicator.CreateText ( "SPECIAL", FloatingTextType.Info );
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
            SoundEffect.Play3D ( currentWeaponMeleeData.swingSoundData.GetRandom (), currentWeaponObject.transform.position, 1, 5, delay: selectedClip.data[i].swingAudioDelay / 60.0f );
            Invoke ( nameof ( PerformComboHit ), selectedClip.data[i].resultAudioDelay / 60.0f );
        }


        Invoke ( nameof ( FinishComboMode ), selectedClip.clipLength / 60.0f );

        nextAttackAnimationDelay = Mathf.Min ( (selectedClip.data[selectedClip.data.Count - 1].resultAudioDelay / 60.0f) + 0.25f, selectedClip.clipLength / 60.0f );
        currentMeleeAnimationLength = selectedClip.clipLength / 60.0f;
        character.FloatingTextIndicator.CreateText ( "COMBO", FloatingTextType.Info, true );
        EntityManager.instance.CameraController.SetCinematicComboView ( comboViews.GetRandom () );
    }

    private void DetermineMeleeHitChance ()
    {
        if (UnityEngine.Random.value < 0.75f)
        {
            PerformHit ();
        }
        else
        {
            PerformMiss ();
        }
    }

    [SerializeField] private float meleeStrikeDistance;
    RaycastHit[] attackSphereHit;
    public NPC comboNPCTarget { get; protected set; }

    private void PerformComboHit ()
    {
        if (comboNPCTarget != null)
        {
            comboNPCTarget.Health.RemoveHealth ( currentWeaponMeleeData.baseDamage, DamageType.PlayerAttack );
            SoundEffect.Play3D ( currentWeaponMeleeData.hitSoundData.GetRandom (), currentWeaponObject.transform.position, 1, 5 );
        }
        else
        {
            PerformMiss ();
        }
    }

    private void FinishComboMode ()
    {
        if (comboNPCTarget != null)
        {
            comboNPCTarget.Character.SetCanMove ( true );
        }

        IsComboMode = false;
        character.SetCanMove ( true );
    }

    private void PerformHit ()
    {
        List<NPC> hitNPCs = new List<NPC> ();
        float healthRemoved = 0;

        if (GetMeleeHitData(out hitNPCs))
        {
            for (int i = 0; i < hitNPCs.Count; i++)
            {
                healthRemoved += hitNPCs[i].Health.RemoveHealth ( currentWeaponMeleeData.baseDamage, DamageType.PlayerAttack );
            }

            SkillManager.instance.AddXpToSkill ( SkillManager.SkillType.Combat, healthRemoved * 0.25f );
            SoundEffect.Play3D ( currentWeaponMeleeData.hitSoundData.GetRandom (), currentWeaponObject.transform.position, 1, 5 );
        }
        else
        {
            PerformMiss ();
        }
    }

    private void PerformMiss ()
    {
        SkillManager.instance.AddXpToSkill ( SkillManager.SkillType.Combat, 0.25f );
        SoundEffect.Play3D ( currentWeaponMeleeData.missSoundData.GetRandom (), currentWeaponObject.transform.position, 1, 5 );
    }

    private bool GetMeleeHitData (out List<NPC> hitNPCs)
    {
        hitNPCs = new List<NPC> ();
        attackSphereHit = new RaycastHit[0];
        attackSphereHit = Physics.SphereCastAll ( EntityManager.instance.MainCamera.transform.parent.position, 0.25f, EntityManager.instance.MainCamera.transform.parent.forward, meleeStrikeDistance );

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

    private NPC GetPotentialMeleeHitNPC ()
    {
        attackSphereHit = new RaycastHit[0];
        attackSphereHit = Physics.SphereCastAll ( EntityManager.instance.MainCamera.transform.parent.position, 0.25f, EntityManager.instance.MainCamera.transform.parent.forward, meleeStrikeDistance );

        for (int i = 0; i < attackSphereHit.Length; i++)
        {
            if (attackSphereHit[i].collider.gameObject.CompareTag ( "NPC" ))
            {
                return attackSphereHit[i].collider.gameObject.GetComponent<NPC> ();
            }
        }

        return null;
    }
    #endregion

    #region Gun
    public void CheckAimPosition ()
    {
        if (character.IsAI)
        {
            AimPosition = EntityManager.instance.PlayerCharacter.Animator.GetBoneTransform ( HumanBodyBones.Head ).position;
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
    }

    private void SetWeaponUI ()
    {
        if (character.IsAI) return;

        if (currentWeaponData == null || character.currentState != Character.State.Standing)
        {
            clipText.text = "";
        }
        else
        {
            Vector3 position = currentWeaponObject.transform.TransformPoint ( currentWeaponGunData.clipTextLocalPosition );

            if (isHolstered)
            {
                clipText.text = "[H]";
                position = currentWeaponObject.transform.TransformPoint ( currentWeaponGunData.clipTextHolsteredLocalPosition );
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
                SoundEffect.Play3D ( currentWeaponGunData.weaponSoundData.audioClipReloadFinished.GetRandom (), currentWeaponObject.transform.position, 1, 10 );
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
        currentReloadTime = currentWeaponGunData.reloadTime;
        SoundEffect.Play ( currentWeaponGunData.weaponSoundData.audioClipReload.GetRandom (), true );
    }

    public void Fire ()
    {
        if (character.isDead) { isFiring = false; }
        if (!isFiring) return;

        if (npcInSights != null && npcInSights.HostilityLevel == HostilityLevel.Passive)
        {
            isFiring = false;
            return;
        }

        if (currentAmmo > 0)
        {
            SoundEffect.Play3D ( currentWeaponGunData.weaponSoundData.audioClipFire.GetRandom (), muzzlePoint.position, 10, 100 );
            character.cIK.AddRecoil ( currentWeaponGunData.recoilData );
            currentAmmo--;
            RayBullet ();

            GameObject ps = Instantiate ( currentWeaponGunData.muzzlePrefab );
            ps.transform.SetParent ( muzzlePoint );
            ps.transform.localPosition = Vector3.zero;
            ps.transform.localRotation = Quaternion.identity;
        }
        else
        {
            SoundEffect.Play3D ( currentWeaponGunData.weaponSoundData.audioClipEmptyFire.GetRandom (), muzzlePoint.position, 2, 10 );
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
                if(npcInSights.HostilityLevel == HostilityLevel.Passive)
                {
                    return;
                }
                else
                {
                    if (aimEnemyHit.distance > currentWeaponGunData.maxDistance) return;

                    float distNrm = Mathf.InverseLerp ( 0.0f, currentWeaponGunData.maxDistance, aimEnemyHit.distance );
                    float baseDamageWithFalloff = currentWeaponGunData.damageByDistanceFalloff.Evaluate ( distNrm ) * currentWeaponData.baseDamage;

                    float healthRemoved = npcInSights.Health.RemoveHealth ( baseDamageWithFalloff, DamageType.PlayerAttack );

                    GameObject go = Instantiate ( ricochetPrefab );
                    go.transform.position = aimEnemyHit.point;
                    go.transform.rotation = Quaternion.LookRotation ( -character.cCameraController.cameraTransform.forward );

                    SkillManager.instance.AddXpToSkill ( SkillManager.SkillType.Shooting, healthRemoved * 0.25f );
                    return;
                }
            }         
        }

        SkillManager.instance.AddXpToSkill ( SkillManager.SkillType.Shooting, 0.25f );
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

        if (currentAmmo > 0)
        {
            SoundEffect.Play3D ( currentWeaponGunData.weaponSoundData.audioClipFire.GetRandom (), muzzlePoint.position, 10, 100 );
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
            SoundEffect.Play3D ( currentWeaponGunData.weaponSoundData.audioClipEmptyFire.GetRandom (), muzzlePoint.position, 2, 10 );
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
        if(UnityEngine.Random.value <= GetComponent<NPC>().NpcData.GetBaseHitChance)
        {
            float damage = GetComponent<NPC> ().NpcData.BaseDamageModifier * currentWeaponData.baseDamage * UnityEngine.Random.Range ( 0.9f, 1.1f );
            EntityManager.instance.PlayerCharacter.Health.RemoveHealth ( damage, DamageType.EnemyAttack );
        }
    }

    [NaughtyAttributes.Button]
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
