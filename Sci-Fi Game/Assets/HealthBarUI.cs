using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform fillImage = null;
    [SerializeField] private TextMeshProUGUI healthText = null;

    private Health health;
    private Transform target;

    public void Initialise (Health health, Transform target)
    {
        this.health = health;
        this.target = target;
        this.health.onHealthChanged += OnHealthChanged;
    }

    public void SetActive ()
    {
        gameObject.SetActive ( true );
    }

    public void SetInactive ()
    {
        gameObject.SetActive ( false );
    }

    private void OnHealthChanged ()
    {
        fillImage.localScale = new Vector3 (health.healthNormalised, 1.0f, 1.0f );
        healthText.text = health.currentHealth.ToString ( "0" ) + " / " + health.MaxHealth.ToString ( "0" );

        if(health.healthNormalised <= 0.0f)
        {
            this.health.onHealthChanged -= OnHealthChanged;
            Destroy ( this.gameObject );
        }
    }

    private void LateUpdate ()
    {
        if (target == null)
        {
            Destroy ( this.gameObject );
            return;
        }

        transform.position = target.transform.position;
    }
}
