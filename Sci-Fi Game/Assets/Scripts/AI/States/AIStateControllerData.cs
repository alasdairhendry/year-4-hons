using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/New AI State Controller Data")]
public class AIStateControllerData : ScriptableObject
{
    [SerializeField] private float nearMissRadius = 1.0f;
    [SerializeField] private float fleesBelowHealthPercent = 0.0f;
    [Space]
    [SerializeField] private List<WeaponData> weaponChoices = new List<WeaponData> ();
    [SerializeField] private bool weaponAlwaysEquipped = true;
    [SerializeField] private bool weaponAlwaysUnholstered = false;
    [Space]
    [SerializeField] private AIStateBaseIdle idleBehaviour;
    [SerializeField] private AIStateBase nearMissBehaviour;
    [SerializeField] private AIStateBaseFlee fleeBehaviour;
    [SerializeField] private AIStateBaseCombat combatBehaviour;
    [Header ( "Combat Gun Stats" )]
    [SerializeField] private float strategicReloadChance = 0.0001f;
    [SerializeField] private float emptyClipReloadChance = 0.1f;
    [SerializeField] private float stopCombatAfterPlayerDeathChance = 1.0f;
    [SerializeField] private float fireChance = 0.1f;
    [SerializeField] private float stopFiringChance = 0.3f;

    public float NearMissRadius { get => nearMissRadius; set => nearMissRadius = value; }
    public float FleesBelowHealthPercent { get => fleesBelowHealthPercent; set => fleesBelowHealthPercent = value; }
    public AIStateBaseIdle IdleBehaviour { get => idleBehaviour; set => idleBehaviour = value; }
    public AIStateBase NearMissBehaviour { get => nearMissBehaviour; set => nearMissBehaviour = value; }
    public AIStateBaseFlee FleeBehaviour { get => fleeBehaviour; set => fleeBehaviour = value; }
    public AIStateBaseCombat CombatBehaviour { get => combatBehaviour; set => combatBehaviour = value; }
    public List<WeaponData> WeaponChoices { get => weaponChoices; set => weaponChoices = value; }
    public bool WeaponAlwaysEquipped { get => weaponAlwaysEquipped; set => weaponAlwaysEquipped = value; }
    public bool WeaponAlwaysUnholstered { get => weaponAlwaysUnholstered; set => weaponAlwaysUnholstered = value; }

    public float StrategicReloadChance { get => strategicReloadChance; set => strategicReloadChance = value; }
    public float EmptyClipReloadChance { get => emptyClipReloadChance; set => emptyClipReloadChance = value; }
    public float StopCombatAfterPlayerDeathChance { get => stopCombatAfterPlayerDeathChance; set => stopCombatAfterPlayerDeathChance = value; }
    public float FireChance { get => fireChance; set => fireChance = value; }
    public float StopFiringChance { get => stopFiringChance; set => stopFiringChance = value; }
}
