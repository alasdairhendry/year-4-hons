using QuestFlow.DialogueEngine;
using UnityEngine;

public enum NPCAttackOption { CanBeAttacked, CannotBeAttack }

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCData npcData;
    [SerializeField] private bool overrideDefaultAttackOption = false;
    [NaughtyAttributes.ShowIf ( nameof ( overrideDefaultAttackOption ) )] [SerializeField] private NPCAttackOption attackOptionOverride = NPCAttackOption.CanBeAttacked;
    [Space]
    [SerializeField] private GameObject lootableNPCPrefab;
    [Space]
    [SerializeField] private Transform healthIndicatorPlaceholder;
    [SerializeField] private bool setInteractableNameToNPCDataName = true;

    public NPCAttackOption NPCAttackOption { get; protected set; }
    public Health Health { get; protected set; }
    public FloatingTextIndicator FloatingTextIndicator { get; protected set; }

    public System.Action OnDeathAction;
    public System.Action OnDeleteAction;

    private float healthIndicatorDisplayCounter = 0;

    public NPCNavMesh NPCNavMesh { get; protected set; }
    public NPCStateController NPCStateController { get; protected set; }
    public MobSpawner MobSpawner { get; protected set; }
    public Character Character { get; protected set; }
    public NPCData NpcData { get => npcData; set => npcData = value; }
    public SkinnedMeshRenderer MeshRenderer { get; set; }
    public bool IsInCombat { get; set; } = false;
    private HealthBarUI healthBar;
    public bool isInConversation { get; set; } = false;

    private void Awake ()
    {
        InitialiseData ();
        CreateHealthIndicator ();
        SubscribeToEvents ();
        QuestFlow.DialogueEngine.DialogueManager.instance.onConversationEnd += OnConversationEnd;
    }

    private void OnConversationEnd (Conversation obj)
    {
        isInConversation = false;
    }

    private void Update ()
    {
        CheckShouldShowHealthIndicator ();
    }

    public void SetMobSpawner (MobSpawner spawner)
    {
        MobSpawner = spawner;
    }

    private void InitialiseData ()
    {
        MeshRenderer = GetComponentInChildren<SkinnedMeshRenderer> ( false );
        Health = GetComponent<Health> ();
        Health.SetMaxHealth ( NPCCombatStats.GetMaxHealth ( NpcData ), true );

        FloatingTextIndicator = GetComponent<FloatingTextIndicator> ();
        NPCNavMesh = GetComponent<NPCNavMesh> ();
        NPCStateController = GetComponent<NPCStateController> ();
        Character = GetComponent<Character> ();

        if (setInteractableNameToNPCDataName)
            GetComponentInChildren<Interactable> ().SetInteractName ( NpcData.NpcName + " [" + Character.cFaction.CurrentFaction.factionName + "]" );

        if (overrideDefaultAttackOption)
        {
            NPCAttackOption = attackOptionOverride;
        }
        else
        {
            NPCAttackOption = npcData.DefaultAttackOption;
        }
    }

    private void CreateHealthIndicator ()
    {
        healthBar = NPCHealthCanvas.instance.SpawnHealthIndicator ( healthIndicatorPlaceholder ).GetComponent<HealthBarUI> ();
        healthBar.Initialise ( Health, healthIndicatorPlaceholder );
        healthBar.SetInactive ();
    }

    private void SubscribeToEvents ()
    {
        Health.onDeath += OnDeathByDamage;
        Health.onHealthChanged += OnHealthChanged;
        Health.onHealthAdded += OnHealthAdded;
        Health.onHealthRemoved += OnHealthRemoved;
        Character.cWeapon.OnAttack += OnAttack;
        SkillManager.instance.OnCharacterLevelIncreased += RefreshMaxHealth;
    }

    private void UnsubscribeToEvents ()
    {
        Health.onDeath -= OnDeathByDamage;
        Health.onHealthChanged -= OnHealthChanged;
        Health.onHealthAdded -= OnHealthAdded;
        Health.onHealthRemoved -= OnHealthRemoved;
        Character.cWeapon.OnAttack -= OnAttack;
        SkillManager.instance.OnCharacterLevelIncreased -= RefreshMaxHealth;
    }

    private void RefreshMaxHealth ()
    {
        Health.SetMaxHealth ( NPCCombatStats.GetMaxHealth ( NpcData ), !IsInCombat );

    }

    private void CheckShouldShowHealthIndicator ()
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

    private void OnAttack (WeaponAttackType attackType)
    {
        healthIndicatorDisplayCounter = 0;
        healthBar.SetActive ();
    }

    private void OnHealthAdded (float amount, HealType healType)
    {
        healthBar.SetActive ();
    }

    private void OnHealthRemoved (float amount, DamageType damageType)
    {
        healthBar.SetActive ();

        if (Random.value < 0.65f)
        {
            SoundEffect.Play3D ( npcData.DamageTakenAudioClips.GetRandom(), this.transform.position, 2, 10 );
        }
    }

    private void OnHealthChanged ()
    {
        healthIndicatorDisplayCounter = 0;
    }

    private void OnDeathByDamage ()
    {
        UnsubscribeToEvents ();
        Character.SetIsDead ();

        NPCStateController.SwitchToIdleBehaviour ();

        if (Random.value > 0.65f)
            Character.cWeapon.Unequip ( true );

        SoundEffect.Play3D ( npcData.DeathAudioClips.GetRandom (), this.transform.position, 2, 10 );
        Destroy ( healthBar.gameObject );

        Destroy ( GetComponentInChildren<Interactable> ().gameObject );

        GameObject lootableNPC = Instantiate ( lootableNPCPrefab );        
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
        Destroy ( GetComponent<NPCNavMesh> () );
        Destroy ( GetComponent<NPCStateController> () );
        Destroy ( GetComponent<NPCStateMachine> () );
        Destroy ( GetComponent<UnityEngine.AI.NavMeshAgent> () );

        this.gameObject.AddComponent<SelfDestruct> ().Initialise ( 180, true );

        OnDeathAction?.Invoke ();
    }

    public void DieImmediately ()
    {
        UnsubscribeToEvents ();
        OnDeleteAction?.Invoke ();
        Destroy ( this.gameObject );
    }

    private void OnDestroy ()
    {
        QuestFlow.DialogueEngine.DialogueManager.instance.onConversationEnd -= OnConversationEnd;
    }
}
