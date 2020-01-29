using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WheelSlipController : MonoBehaviour
{
    private AudioSource source;
    private WheelCollider col;
    private WheelHit wHit;
    public float f = 0.0f;
    public float s = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource> ();
        col = GetComponent<WheelCollider> ();
        source.volume = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (col.GetGroundHit ( out wHit ))
        {
            f = wHit.forwardSlip;
            s = wHit.sidewaysSlip;

            float fo = Mathf.Lerp ( 0.0f, 0.25f, Mathf.InverseLerp ( 0.50f, 1.0f, Mathf.Abs ( wHit.forwardSlip * 2.0f ) ) );
            float si = Mathf.Lerp ( 0.0f, 0.25f, Mathf.InverseLerp ( 0.50f, 1.0f, Mathf.Abs ( wHit.sidewaysSlip * 2.0f ) ) );

            source.volume = Mathf.Max ( fo, si );
        }
        else
        {
            source.volume = Mathf.Lerp ( source.volume, 0.0f, Time.deltaTime * 10.0f );
        }
    }
}
