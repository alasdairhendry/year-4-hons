using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFOV : MonoBehaviour
{
    [SerializeField] private Camera thisCamera;
    [SerializeField] private Camera targetCamera;

    void Update()
    {
        thisCamera.fieldOfView = targetCamera.fieldOfView;        
    }
}
