using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPortraitController : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3 ( -0.7024824f, 0.6575006f, 0.0f );
    [SerializeField] private Vector3 euler = new Vector3 ( 83.67f, 270.0f, 0.0f );
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject skinnedRendererObject;

    public void OnConversationStart ()
    {
        //Debug.Log ( string.Format ( "{0}: {1}", subtitle.speakerInfo.transform.name, subtitle.formattedText.text ) );
        NPCPortraitCamera.instance.SetTarget ( skinnedRendererObject, animator.GetBoneTransform ( HumanBodyBones.Hips ), offset, euler );
    }

    public void OnConversationLineEnd ()
    {
        //Debug.Log ( string.Format ( "{0}: {1}", subtitle.speakerInfo.transform.name, subtitle.formattedText.text ) );
        NPCPortraitCamera.instance.SetTarget ( skinnedRendererObject, animator.GetBoneTransform ( HumanBodyBones.Hips ), offset, euler );
    }

    private void OnValidate ()
    {
        if (animator == null)
            animator = GetComponent<Animator> ();

        skinnedRendererObject = GetComponentInChildren<SkinnedMeshRenderer> ( false ).gameObject;
    }
}
