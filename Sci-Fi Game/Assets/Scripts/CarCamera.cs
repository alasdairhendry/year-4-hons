using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public Transform target;
    public float damping = 5.0f;
    public float rDamping = 7.5f;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectsOfType<NewVehicleGround> ().FirstOrDefault ( x => x.gameObject.activeSelf ).transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Slerp ( transform.position, target.position, Time.deltaTime * damping );
        transform.rotation = Quaternion.Slerp ( transform.rotation, Quaternion.Euler ( 0.0f, target.eulerAngles.y, 0.0f ), Time.deltaTime * rDamping );
    }
}
