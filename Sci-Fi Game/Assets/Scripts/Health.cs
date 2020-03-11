using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { PlayerAttack, EnemyAttack, FallDamage, FireDamage, WaterDamage, BluntForce, CompostDamage }
public enum HealType { Consumable, FactionEffect, Regeneration }

public class Health : MonoBehaviour
{
    public System.Action onDeath;
    public System.Action<float, HealType> onHealthAdded;
    public System.Action<float, DamageType> onHealthRemoved;
    public System.Action onHealthChanged;
    public bool isDead { get; protected set; }

    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private FloatingTextIndicator floatingTextIndicator;

    [SerializeField] private bool regeneratesOverTime = false;
    [SerializeField] [NaughtyAttributes.ShowIf("regeneratesOverTime")] private float regenerateEveryXSeconds = 10;
    [SerializeField] [NaughtyAttributes.ShowIf ( "regeneratesOverTime" )] private float regeneratePercentOfMaxHealth = 0.05f;
    [SerializeField] [NaughtyAttributes.ShowIf ( "regeneratesOverTime" )] private float startRegenerationAfterXSeconds = 30.0f;
    private float regenerationDelayCounter = 0.0f;
    private float gameTimeAtLastHealthRemoved = 0.0f;
    public float TimeSinceLastHealthRemoved
    {
        get
        {
            return Time.time - gameTimeAtLastHealthRemoved;
        }
    }

    [NaughtyAttributes.ShowNativeProperty] public float currentHealth { get; protected set; }
    [NaughtyAttributes.ShowNativeProperty] public float healthNormalised { get => currentHealth / maxHealth; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
     public bool IsPlayer { get; set; } = false;

    private void Awake ()
    {
        currentHealth = maxHealth;
    }

    [NaughtyAttributes.Button]
    private void TakeDamage ()
    {
        RemoveHealth ( MaxHealth * 0.1f, DamageType.EnemyAttack );
    }

    private void Update ()
    {
        if(Input.GetKeyDown(KeyCode.L) && IsPlayer)
        {
            TakeDamage ();
        }

        if (regeneratesOverTime)
        {
            if (healthNormalised < 1.0f)
            {
                if (TimeSinceLastHealthRemoved >= startRegenerationAfterXSeconds)
                {
                    regenerationDelayCounter += Time.deltaTime;

                    if (regenerationDelayCounter >= regenerateEveryXSeconds)
                    {
                        regenerationDelayCounter = 0.0f;
                        float healthToRegenerate = (MaxHealth * regeneratePercentOfMaxHealth) * ( 1 + TalentManager.instance.GetTalentModifier ( TalentType.Restoration ));

                        AddHealth ( healthToRegenerate, HealType.Regeneration );

                        if (healthNormalised >= 1.0f && IsPlayer && TalentManager.instance.GetTalent ( TalentType.Restoration ).currentLevel > 1)
                        {
                            MessageBox.AddMessage ( "Your health regenerated faster because of your " + TalentManager.instance.GetTalent ( TalentType.Restoration ).talentData.talentName + " talent" );
                        }
                    }
                }
            }
        }
    }

    public void SetMaxHealth(float value, bool resetCurrent)
    {
        maxHealth = value;

        if (resetCurrent)
            currentHealth = maxHealth;

        onHealthChanged?.Invoke ();
    }

    public void HealOverTime(float overallHealAmount, float interval, float length)
    {
        StartCoroutine ( HealOverTimeIE ( overallHealAmount, interval, length ) );
    }

    private IEnumerator HealOverTimeIE (float overallHealAmount, float interval, float length)
    {
        float intervalHealAmount = overallHealAmount / (length / interval);
        float amountHealed = 0;
        int maxCount = Mathf.RoundToInt ( length / interval );
        int count = 0;

        while (count < maxCount)
        {
            yield return new WaitForSeconds ( interval );
            if (isDead) yield break;

            float localHealed = 0;

            if (count == maxCount - 1) localHealed = overallHealAmount - amountHealed;
            else localHealed = intervalHealAmount;

            AddHealth ( localHealed, HealType.Consumable );
            amountHealed += intervalHealAmount;

            count++;
        }
    }

    public float AddHealth(float amount, HealType healType)
    {
        if (isDead) return 0.0f;
        float added = Mathf.Min ( amount, maxHealth - currentHealth );
        currentHealth += amount;
        currentHealth = Mathf.Clamp ( currentHealth, 0.0f, maxHealth );

        if (floatingTextIndicator != null)
            floatingTextIndicator.CreateHealText ( amount );

        onHealthAdded?.Invoke ( added, healType );
        onHealthChanged?.Invoke ();
        return added;
    }

    public float RemoveHealth (float amount, DamageType damageType, bool isCritical = false)
    {
        if (isDead) return 0.0f;
        if (currentHealth <= 0) return 0;

        float removed = Mathf.Min ( amount, currentHealth );
        currentHealth -= removed;
        currentHealth = Mathf.Clamp ( currentHealth, 0.0f, maxHealth );
        onHealthRemoved?.Invoke ( removed, damageType );
        onHealthChanged?.Invoke ();
        CheckDeath ();
        gameTimeAtLastHealthRemoved = Time.time;
        regenerationDelayCounter = 0.0f;
        if (floatingTextIndicator != null)
            floatingTextIndicator.CreateDamageText ( removed, isCritical);
        return removed;
    }

    public void Revive ()
    {
        isDead = false;
        currentHealth = maxHealth;
        onHealthChanged?.Invoke ();
    }

    private void CheckDeath ()
    {
        if(currentHealth <= 0.0f)
        {
            isDead = true;
            onDeath?.Invoke ();
        }
    }

    public bool WillDamageKill(float amount)
    {
        return currentHealth - amount <= 0;
    }
}
