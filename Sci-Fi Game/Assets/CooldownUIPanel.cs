using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUIPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image fillImage;

    private bool isActive = false;

    public void SetActive (bool state)
    {
        isActive = state;
        fillImage.fillAmount = 1;

        if (state)
        {
            canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }

    private void Update ()
    {
        if (isActive)
        {
            fillImage.fillAmount = GameManager.instance.GlobalCooldownNormalised;
        }
    }
}
