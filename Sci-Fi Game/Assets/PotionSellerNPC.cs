using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class PotionSellerNPC : MonoBehaviour
{
    public bool shouldWarp = false;
    public Transform headBone;
    private Transform currentConversationActor;

    public void OnConversationStart (Transform actor)
    {
        shouldWarp = true;
        currentConversationActor = actor;
    }

    public void OnConversationEnd (Transform actor)
    {
        shouldWarp = false;
        currentConversationActor = null;
    }

    public void SetShouldWarp (bool state)
    {
        shouldWarp = state;
    }

    void Update ()
    {
        if (currentConversationActor)
        {
            Vector3 dir = currentConversationActor.position - transform.position;
            dir.y = 0;
            if (dir == Vector3.zero) return;
            transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.LookRotation ( dir.normalized ), Time.deltaTime * 3.5f );
        }

        if (shouldWarp)
        {
            float min = 0.25f;
            float max = 1.5f;
            float x = Mathf.Lerp ( min, max, Mathf.InverseLerp ( -1.0f, 1.0f, Mathf.Sin ( Time.time + Random.value ) ) );
            float y = Mathf.Lerp ( min, max, Mathf.InverseLerp ( -1.0f, 1.0f, Mathf.Sin ( Time.time + Random.value ) ) );
            float z = Mathf.Lerp ( min, max, Mathf.InverseLerp ( -1.0f, 1.0f, Mathf.Sin ( Time.time + Random.value ) ) );
            headBone.localScale = Vector3.Slerp ( headBone.localScale, new Vector3 ( x, y, z ), Time.deltaTime * 2.0f );
        }
        else
        {
            headBone.localScale = Vector3.Slerp ( headBone.localScale, Vector3.one, Time.deltaTime );
        }
    }
}
