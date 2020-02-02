using UnityEngine;

public enum FloatingTextType { Damage, Health, Info }

public class FloatingTextIndicator : MonoBehaviour
{
    [SerializeField] private Transform damageIndicatorPlaceholder;

    public void CreateText (string text, FloatingTextType type, bool critical = false)
    {
        DamageCanvas.instance.SpawnIndicator ( damageIndicatorPlaceholder, 0.5f, text, type );
    }
}
