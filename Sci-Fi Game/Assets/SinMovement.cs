using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    private void LateUpdate ()
    {
        transform.localPosition = new Vector3 ( 0.0f, Mathf.Sin ( Time.time ), 0.0f );
    }
}
