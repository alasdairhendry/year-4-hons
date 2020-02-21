using System;
using QuestFlow.DialogueEngine;
using UnityEngine;

public class NPCPortrait : MonoBehaviour
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

    private void Awake ()
    {
        if (animator == null)
            animator = GetComponent<Animator> ();

        skinnedRendererObject = GetComponentInChildren<SkinnedMeshRenderer> ( false ).gameObject;
    }

    private void OnValidate ()
    {
        if (animator == null)
            animator = GetComponent<Animator> ();

        skinnedRendererObject = GetComponentInChildren<SkinnedMeshRenderer> ( false ).gameObject;
    }
}
