using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { PlayerAttack, EnemyAttack, FallDamage, FireDamage, WaterDamage, BluntForce }
public enum HealType { Consumable }

public class Health : MonoBehaviour
{
    public System.Action onDeath;
    public System.Action<float, HealType> onHealthAdded;
    public System.Action<float, DamageType> onHealthRemoved;
    public System.Action onHealthChanged;
    public bool isDead { get; protected set; }

    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private FloatingTextIndicator floatingTextIndicator;
    public float currentHealth { get; protected set; }
    public float healthNormalised { get => currentHealth / maxHealth; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    private void Awake ()
    {
        currentHealth = maxHealth;
    }

    public void SetMaxHealth(float value, bool resetCurrent)
    {
        maxHealth = value;

        if (resetCurrent)
            currentHealth = maxHealth;
    }

    public float AddHealth(float amount, HealType healType)
    {
        float added = Mathf.Min ( amount, maxHealth - currentHealth );
        currentHealth += amount;
        currentHealth = Mathf.Clamp ( currentHealth, 0.0f, maxHealth );
        onHealthAdded?.Invoke ( added, healType );
        onHealthChanged?.Invoke ();
        return added;
    }

    public float RemoveHealth (float amount, DamageType damageType)
    {
        if (currentHealth <= 0) return 0;

        float removed = Mathf.Min ( amount, currentHealth );
        currentHealth -= removed;
        currentHealth = Mathf.Clamp ( currentHealth, 0.0f, maxHealth );
        onHealthRemoved?.Invoke ( removed, damageType );
        onHealthChanged?.Invoke ();
        CheckDeath ();
        if (floatingTextIndicator != null)
            floatingTextIndicator.CreateText ( removed.ToString (), FloatingTextType.Damage );
        return removed;
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
