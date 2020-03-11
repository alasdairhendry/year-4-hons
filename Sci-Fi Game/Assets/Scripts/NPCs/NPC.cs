using QuestFlow.DialogueEngine;
using UnityEngine;

public enum NPCAttackOption { CanBeAttacked, CannotBeAttack }

public class NPC : MonoBehaviour
{
    [SerializeField] protected NPCData npcData;
    [SerializeField] protected bool overrideDefaultAttackOption = false;
    [NaughtyAttributes.ShowIf ( nameof ( overrideDefaultAttackOption ) )] [SerializeField] protected NPCAttackOption attackOptionOverride = NPCAttackOption.CanBeAttacked;
    [Space]
    [SerializeField] protected Transform healthIndicatorPlaceholder;
    [SerializeField] protected bool setInteractableNameToNPCDataName = true;

    public NPCAttackOption NPCAttackOption
    {
        get
        {
            if (overrideDefaultAttackOption)
            {
                return attackOptionOverride;
            }
            else
            {
                return npcData.DefaultAttackOption;
            }
        }
    }
    public Health Health { get; protected set; }
    public FloatingTextIndicator FloatingTextIndicator { get; protected set; }

    public System.Action OnDeathAction;
    public System.Action OnDeleteAction;

    protected float healthIndicatorDisplayCounter = 0;

    public NPCNavMesh NPCNavMesh { get; protected set; }
    public NPCStateController NPCStateController { get; protected set; }
    public MobSpawner MobSpawner { get; protected set; }
    public Character Character { get; protected set; }
    public NPCData NpcData { get => npcData; set => npcData = value; }
    public SkinnedMeshRenderer MeshRenderer { get; set; }
    public bool IsInCombat { get; set; } = false;
    protected HealthBarUI healthBar;
    public bool isInConversation { get; set; } = false;

    protected virtual void Awake ()
    {
        InitialiseData ();
        SetMaterial ();
        CreateHealthIndicator ();
        SubscribeToEvents ();

        if (QuestFlow.DialogueEngine.DialogueManager.instance) QuestFlow.DialogueEngine.DialogueManager.instance.onConversationEnd += OnConversationEnd;
    }

    protected virtual void OnConversationEnd (Conversation obj)
    {
        isInConversation = false;
    }

    protected virtual void Update ()
    {
        CheckShouldShowHealthIndicator ();
    }

    public virtual void SetMobSpawner (MobSpawner spawner)
    {
        MobSpawner = spawner;
    }

    protected virtual void InitialiseData ()
    {
        MeshRenderer = GetComponentInChildren<SkinnedMeshRenderer> ( false );
        Health = GetComponent<Health> ();
        Health.SetMaxHealth ( NPCCombatStats.GetMaxHealth ( NpcData ), true );

        FloatingTextIndicator = GetComponent<FloatingTextIndicator> ();
        NPCNavMesh = GetComponent<NPCNavMesh> ();
        NPCStateController = GetComponent<NPCStateController> ();
        Character = GetComponent<Character> ();

        Interactable interactable = GetComponentInChildren<Interactable> ();

        switch (NPCAttackOption)
        {
            case NPCAttackOption.CanBeAttacked:
                interactable.TextColour = ColourDescription.InteractionAttackableEnemy;
                if (setInteractableNameToNPCDataName)
                {
                    interactable.SetInteractType ( "Level " + NpcData.CombatLevel.ToString ( "0" ) );
                    interactable.SetInteractName ( NpcData.NpcName + " [" + Character.cFaction.CurrentFaction.factionName + "]" );
                }
                break;
            case NPCAttackOption.CannotBeAttack:
                interactable.TextColour = ColourDescription.InteractionDefault;
                if (setInteractableNameToNPCDataName)
                    interactable.SetInteractName ( NpcData.NpcName + " [" + Character.cFaction.CurrentFaction.factionName + "]" );
                break;
        }
    }

    private void SetMaterial ()
    {
        if (npcData.CharacterMaterials.Count > 0)
        {
            MeshRenderer.sharedMaterial = npcData.CharacterMaterials.GetRandom ();
        }
    }

    protected virtual void CreateHealthIndicator ()
    {
        if (NPCHealthCanvas.instance == null) return;
        healthBar = NPCHealthCanvas.instance.SpawnHealthIndicator ( healthIndicatorPlaceholder ).GetComponent<HealthBarUI> ();
        healthBar.Initialise ( Health, healthIndicatorPlaceholder );
        healthBar.SetInactive ();
    }

    protected virtual void SubscribeToEvents ()
    {
        Health.onDeath += OnDeathByDamage;
        Health.onHealthChanged += OnHealthChanged;
        Health.onHealthAdded += OnHealthAdded;
        Health.onHealthRemoved += OnHealthRemoved;
        Character.cWeapon.OnAttack += OnAttack;
    }

    protected virtual void UnsubscribeToEvents ()
    {
        Health.onDeath -= OnDeathByDamage;
        Health.onHealthChanged -= OnHealthChanged;
        Health.onHealthAdded -= OnHealthAdded;
        Health.onHealthRemoved -= OnHealthRemoved;
        Character.cWeapon.OnAttack -= OnAttack;
    }

    protected virtual void CheckShouldShowHealthIndicator ()
    {
        if (healthBar == null) return;

        if (healthBar.gameObject.activeSelf == true)
        {
            healthIndicatorDisplayCounter += Time.deltaTime;
            if (healthIndicatorDisplayCounter > 5.0f)
            {
                healthBar.SetInactive ();
            }
        }
    }

    protected virtual void OnAttack (WeaponAttackType attackType)
    {
        healthIndicatorDisplayCounter = 0;
        healthBar.SetActive ();
    }

    protected virtual void OnHealthAdded (float amount, HealType healType)
    {
        healthBar.SetActive ();
    }

    protected virtual void OnHealthRemoved (float amount, DamageType damageType)
    {
        healthBar.SetActive ();

        if (Random.value < 0.65f)
        {
            SoundEffectManager.Play3D ( npcData.DamageTakenAudioClips.GetRandom(), AudioMixerGroup.SFX, this.transform.position, minDistance: 2, maxDistance: 10 );
        }
    }

    protected virtual void OnHealthChanged ()
    {
        healthIndicatorDisplayCounter = 0;
    }

    protected virtual void OnDeathByDamage ()
    {
        UnsubscribeToEvents ();
        Character.SetIsDead ();

        NPCStateController.SwitchToIdleBehaviour ();

        if (Random.value > 0.65f)
            Character.cWeapon.Unequip ( true );

        SoundEffectManager.Play3D ( npcData.DeathAudioClips.GetRandom (), AudioMixerGroup.SFX, this.transform.position, minDistance: 2, maxDistance: 10 );
        Destroy ( healthBar.gameObject );

        Destroy ( GetComponentInChildren<Interactable> ().gameObject );

        GameObject lootableNPC = Instantiate ( AssetManager.instance.GetPrefab ( PrefabAsset.LootableNPC ) );
        lootableNPC.GetComponent<LootableNPC> ().Initialise ( GetComponent<Animator> ().GetBoneTransform ( HumanBodyBones.Chest ), npcData );

        GetComponent<Animator> ().SetBool ( "die", true );

        Destroy ( GetComponent<Health> () );
        Destroy ( GetComponent<Character> () );
        Destroy ( GetComponent<CharacterInput> () );
        Destroy ( GetComponent<CharacterMovement> () );
        Destroy ( GetComponent<CharacterAnimator> () );
        Destroy ( GetComponent<Rigidbody> () );
        Destroy ( GetComponent<CapsuleCollider> () );
        Destroy ( GetComponent<CharacterIK> () );
        Destroy ( GetComponent<CharacterPhysics> () );
        Destroy ( GetComponent<NPCPathMovement> () );
        Destroy ( GetComponent<NPCNavMesh> () );
        Destroy ( GetComponent<QuestFlow.Actor> () );
        Destroy ( GetComponent<NPCStateController> () );
        Destroy ( GetComponent<NPCStateMachine> () );
        Destroy ( GetComponent<UnityEngine.AI.NavMeshAgent> () );

        this.gameObject.AddComponent<SelfDestruct> ().Initialise ( 180, true );

        OnDeathAction?.Invoke ();
    }

    public virtual void DieImmediately ()
    {
        UnsubscribeToEvents ();
        OnDeleteAction?.Invoke ();
        Destroy ( this.gameObject );
    }

    protected virtual void OnDestroy ()
    {
        if(QuestFlow.DialogueEngine.DialogueManager.instance)
        QuestFlow.DialogueEngine.DialogueManager.instance.onConversationEnd -= OnConversationEnd;
    }

    public virtual float OnBeforeDamagedByWeapon (float damage, WeaponData currentWeaponData)
    {
        return damage;
    }
}
