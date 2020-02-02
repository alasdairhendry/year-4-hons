using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Passive means the player cannot attack this NPC and they cant attack the player
/// Retaliates means the player can attack this NPC and they will retaliate
/// Flee means the player can attack this NPC and they will try to flee
/// Aggressive means the player can attack this NPC, and the NPC will attack the player on sight
/// </summary>
public enum HostilityLevel { Passive, Retaliates, Flee, Aggressive }

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCData npcData;
    [SerializeField] private bool overrideHostilityLevel = false;
    [NaughtyAttributes.ShowIf ( nameof ( overrideHostilityLevel ) )] [SerializeField] private HostilityLevel hostilityLevelOverride = HostilityLevel.Passive;    
    [Space]
    [SerializeField] private GameObject lootableNPCPrefab;
    [Space]
    [SerializeField] private Transform healthIndicatorPlaceholder;

    public bool IsAttackable { get => HostilityLevel != HostilityLevel.Passive; }
    public HostilityLevel HostilityLevel { get; protected set; }
    public Health Health { get; protected set; }
    public FloatingTextIndicator FloatingTextIndicator { get; protected set; }

    public System.Action OnDeathAction;

    private GameObject healthIndicatorParent;
    private RectTransform healthIndicatorGreenFill;
    private float timeSinceLastHealthChange = 0;

    private void Awake ()
    {
        Health = GetComponent<Health> ();
        FloatingTextIndicator = GetComponent<FloatingTextIndicator> ();

        Health.SetMaxHealth ( npcData.MaxHealth, true );

        if (overrideHostilityLevel)
        {
            HostilityLevel = hostilityLevelOverride;
        }
        else
        {
            HostilityLevel = npcData.DefaultHostilityLevel;
        }

        healthIndicatorParent = NPCHealthCanvas.instance.SpawnHealthIndicator ( healthIndicatorPlaceholder ).gameObject;
        healthIndicatorGreenFill = healthIndicatorParent.transform.GetChild ( 0 ).GetChild ( 0 ).GetComponent<RectTransform> ();
        healthIndicatorParent.SetActive ( false );

        Health.onDeath += OnDeath;
        Health.onHealthChanged += OnHealthChanged;
        Health.onHealthAdded += OnHealthAdded;
        Health.onHealthRemoved += OnHealthRemoved;
    }

    private void Update ()
    {
        if (healthIndicatorParent == null) return;
        if (healthIndicatorParent.gameObject.activeSelf == true)
        {
            timeSinceLastHealthChange += Time.deltaTime;
            if (timeSinceLastHealthChange > 5.0f)
            {
                healthIndicatorParent.SetActive ( false );
            }
        }
    }

    private void OnHealthAdded (float amount, HealType healType)
    {

    }

    private void OnHealthRemoved (float amount, DamageType damageType)
    {
        if(Random.value > 0.85f)
        {
            SoundEffect.Play3D ( npcData.DamageTakenAudioClips.GetRandom(), this.transform.position, 2, 10 );
        }
    }

    private void OnHealthChanged ()
    {
        healthIndicatorParent.gameObject.SetActive ( true );
        healthIndicatorGreenFill.localScale = new Vector3 ( 1.0f * Health.healthNormalised, 1.0f, 1.0f );
        timeSinceLastHealthChange = 0;
    }

    private void OnDeath ()
    {
        SoundEffect.Play3D ( npcData.DeathAudioClips.GetRandom (), this.transform.position, 2, 10 );
        Destroy ( healthIndicatorParent );

        Destroy ( GetComponentInChildren<Interactable> ().gameObject );

        GameObject lootableNPC = Instantiate ( lootableNPCPrefab );        
        lootableNPC.GetComponent<LootableNPC> ().Initialise ( GetComponent<Animator> ().GetBoneTransform ( HumanBodyBones.Chest ), npcData );

        GetComponent<Animator> ().SetTrigger ( "die" );

        Destroy ( GetComponent<Health> () );
        Destroy ( GetComponent<Character> () );
        Destroy ( GetComponent<CharacterInput> () );
        Destroy ( GetComponent<CharacterMovement> () );
        Destroy ( GetComponent<CharacterAnimator> () );
        Destroy ( GetComponent<Rigidbody> () );
        Destroy ( GetComponent<CapsuleCollider> () );
        Destroy ( GetComponent<CharacterIK> () );
        Destroy ( GetComponent<CharacterPhysics> () );

        this.gameObject.AddComponent<SelfDestruct> ().Initialise ( 180, true );

        OnDeathAction?.Invoke ();
    }

    private void LateUpdate ()
    {
        if (healthIndicatorParent == null) return;

        if (healthIndicatorParent.gameObject.activeSelf == true)
        {
            healthIndicatorParent.transform.position = healthIndicatorPlaceholder.position;
        }
    }

    public void SetHostilityLevel (HostilityLevel level)
    {
        HostilityLevel = level;
    }
}
