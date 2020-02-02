using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private RectTransform fillImage;

    private void Update ()
    {
        fillImage.localScale = new Vector3 ( 1.0f, EntityManager.instance.PlayerCharacter.Health.healthNormalised, 1.0f );
    }
}
