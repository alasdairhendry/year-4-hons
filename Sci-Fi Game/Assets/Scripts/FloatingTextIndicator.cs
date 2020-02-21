using UnityEngine;

public enum FloatingTextType { Damage, Health, Info }

public class FloatingTextIndicator : MonoBehaviour
{
    [SerializeField] private Transform damageIndicatorPlaceholder;
    private bool isPlayer = false;

    private void Awake ()
    {
        if (EntityManager.instance.PlayerCharacter.transform == transform) isPlayer = true;
    }

    public void CreateDamageText (float text, bool isCritical = false)
    {
        DamageCanvas.instance.SpawnDamageIndicator ( damageIndicatorPlaceholder, 0.5f, text, isCritical, isPlayer );
    }

    public void CreateInfoText(string text, ColourDescription colourDescription = ColourDescription.None)
    {
        DamageCanvas.instance.SpawnInfoText ( damageIndicatorPlaceholder, 0.5f, text );

    }
}
