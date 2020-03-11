using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverboardCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject hoverboardPrefab;

    private GameObject currentHoverboard = null;

    public void SetActive ()
    {
        canvasGroup.DOFade ( 1.0f, 0.5f ).OnComplete ( () => { canvasGroup.blocksRaycasts = true; } );
    }

    public void Interact ()
    {
        if (currentHoverboard == null)
        {
            SpawnHoverboard ();
        }

        if (currentHoverboard == null)
        {
            MessageBox.AddMessage ( "I don't have a hoverboard to call.", MessageBox.Type.Error );
            return;
        }

        Vector2 randomCircle = Random.insideUnitCircle * 2.0f;
        Vector3 targetPosition = EntityManager.instance.PlayerCharacter.transform.position + new Vector3 ( randomCircle.x, 2.0f, randomCircle.y );
        currentHoverboard.transform.position = targetPosition;

        MessageBox.AddMessage ( "You summon your hoverboard.", MessageBox.Type.Warning );
        SoundEffectManager.Play ( AudioClipAsset.UseGem, AudioMixerGroup.SFX );
    }

    private void SpawnHoverboard ()
    {
        currentHoverboard = Instantiate ( hoverboardPrefab );        
    }
}
