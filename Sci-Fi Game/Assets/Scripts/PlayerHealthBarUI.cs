using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;
    private float velocity = 0;

    private void Update ()
    {
        fillImage.fillAmount = Mathf.SmoothDamp ( fillImage.fillAmount, EntityManager.instance.PlayerCharacter.Health.healthNormalised, ref velocity, 0.5f );
        healthText.text = string.Format ( "{0:0.#} / {1:0}", EntityManager.instance.PlayerCharacter.Health.currentHealth, EntityManager.instance.PlayerCharacter.Health.MaxHealth );
    }
}
