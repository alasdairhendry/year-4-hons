using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public float damp = 10.0f;
    [SerializeField] private bool lookX = true;
    [SerializeField] private bool lookY = false;
    [SerializeField] private bool lookZ = true;

    // Update is called once per frame
    void LateUpdate ()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;

        if (!lookX)
            dir.x = 0.0f;

        if (!lookY)
            dir.y = 0.0f;

        if (!lookZ)
            dir.z = 0.0f;

        dir.Normalize ();

        Quaternion lookDir = Quaternion.LookRotation ( dir );
        transform.rotation = Quaternion.Slerp ( transform.rotation, lookDir, Time.deltaTime * damp );
    }
}
