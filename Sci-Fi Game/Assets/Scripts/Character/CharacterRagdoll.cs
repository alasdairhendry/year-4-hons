using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRagdoll : MonoBehaviour
{
    [SerializeField] private List<RagdollPart> bodyParts = new List<RagdollPart> ();
    [Space]
    [SerializeField] private GameObject ActiveOnRagdoll;
    [SerializeField] private GameObject InactiveOnRagdoll;

    [NaughtyAttributes.Button]
    public void ToRagdoll ()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            bodyParts[i].ToRagdoll ();
        }

        ActiveOnRagdoll.SetActive ( true );
        ActiveOnRagdoll.transform.SetParent ( null );
        InactiveOnRagdoll.SetActive ( false );
        //FindObjectOfType<PlayerCameraController> ().SetTarget ( this.transform.GetChild ( 2 ).GetChild ( 0 ).GetChild ( 0 ).GetChild ( 0 ) );
    }

    [System.Serializable]
    public class RagdollPart
    {
        public Transform animatedBone;
        public Transform ragdollBone;

        public void ToRagdoll ()
        {
            ragdollBone.transform.localPosition = animatedBone.transform.localPosition;
            ragdollBone.transform.localRotation = animatedBone.transform.localRotation;
        }
    }
}

