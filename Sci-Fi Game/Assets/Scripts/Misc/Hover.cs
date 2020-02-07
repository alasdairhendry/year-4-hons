using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hover : MonoBehaviour
{
    public new Rigidbody rigidbody { get; protected set; }

    [SerializeField] private List<Vector3> localSuspensionPoints = new List<Vector3> ();

    [SerializeField] public float hoverHeight = 1.0f;
    [SerializeField] private float hoverForce = 5.0f;
    [SerializeField] private float hoverForceGainRateModifier = 2.5f;
    [SerializeField] private float hoverForceLoseRateModifier = 2.5f;
    [SerializeField] private float hoverDamp = 2.5f;
    [SerializeField] private ForceMode forceMode = ForceMode.Acceleration;
    [SerializeField] private LayerMask collisionLayer;

    [SerializeField] private Vector3 hoverDirection = new Vector3 ( 0.0f, -1.0f, 0.0f );
    private float forceToApply = 0.0f;
    public Space hoverDirectionSpace = Space.Self;
    public bool isHovering { get; protected set; } = true;
    public bool DetectedHoverableSurface { get; protected set; } = false;
    public bool DetectedHoverSurfaceDoubleDistance { get; protected set; } = false;

    private void Awake ()
    {
        rigidbody = GetComponent<Rigidbody> ();
    }

    private void Update ()
    {
        if (isHovering)
        {
            if(forceToApply < hoverForce)
            {
                forceToApply += Time.deltaTime * hoverForceGainRateModifier;

                if (forceToApply > hoverForce)
                    forceToApply = hoverForce;
            }
        }
        else
        {
            if (forceToApply > 0)
            {
                forceToApply -= Time.deltaTime * hoverForceLoseRateModifier;

                if (forceToApply < 0)
                    forceToApply = 0;
            }
        }
    }

    private void FixedUpdate ()
    {
        Ray ray = new Ray ();
        RaycastHit[] hits;

        for (int i = 0; i < localSuspensionPoints.Count; i++)
        {
            ray.origin = transform.TransformPoint ( localSuspensionPoints[i] );

            if (hoverDirectionSpace == Space.Self)
                ray.direction = transform.TransformDirection ( hoverDirection.normalized );
            else ray.direction = hoverDirection.normalized;

            hits = Physics.RaycastAll ( ray, hoverHeight, collisionLayer );

            for (int x = 0; x < hits.Length; x++)
            {
                if (hits[x].collider.gameObject == this.gameObject) continue;

                float f = Mathf.Lerp ( 1.0f, 0.0f, hits[x].distance / hoverHeight );

                if(forceToApply > 0)
                rigidbody.AddForceAtPosition ( -ray.direction * forceToApply * f, ray.origin, forceMode );
                break;
            }

            if (hits.Length == 0) DetectedHoverableSurface = false;
            else DetectedHoverableSurface = true;

            hits = Physics.RaycastAll ( ray, hoverHeight * 1.5f, collisionLayer );
            if (hits.Length == 0) DetectedHoverSurfaceDoubleDistance = false;
            else DetectedHoverSurfaceDoubleDistance = true;
            //if (Physics.Raycast ( ray, out hits, hoverHeight, collisionLayer ))
            //{
            //    float f = Mathf.Lerp ( 1.0f, 0.0f, hits.distance / hoverHeight );

            //    rigidbody.AddForceAtPosition ( -ray.direction * hoverForce * f, ray.origin, forceMode );
            //}
        }
    }

    private void OnDrawGizmosSelected ()
    {
        Vector3 direction = new Vector3 ();

        if (hoverDirectionSpace == Space.Self)
            direction = transform.TransformDirection ( hoverDirection.normalized );
        else direction = hoverDirection.normalized;

        for (int i = 0; i < localSuspensionPoints.Count; i++)
        {
            Vector3 position = transform.TransformPoint ( localSuspensionPoints[i] );
            Debug.DrawLine ( position, position + (direction * hoverHeight), Color.red );
        }

        if (Application.isPlaying)
        {
            Ray ray = new Ray ();
            RaycastHit hit;

            for (int i = 0; i < localSuspensionPoints.Count; i++)
            {
                ray.origin = transform.TransformPoint ( localSuspensionPoints[i] );

                if (hoverDirectionSpace == Space.Self)
                    ray.direction = transform.TransformDirection ( hoverDirection.normalized );
                else ray.direction = hoverDirection.normalized;

                if (Physics.Raycast ( ray, out hit, hoverHeight, collisionLayer))
                {
                    float f = Mathf.Lerp ( 0.0f, 1.0f, hit.distance / hoverHeight );

                    Debug.DrawLine ( ray.origin, ray.origin + (ray.direction * hoverHeight * f), Color.blue );
                }
            }
        }
    }

    [NaughtyAttributes.Button]
    private void turnon ()
    {
        SetIsHovering ( true );
    }

    [NaughtyAttributes.Button]
    private void turnoff ()
    {
        SetIsHovering ( false );
    }

    public void SetIsHovering(bool isHovering)
    {
        this.isHovering = isHovering;
    }
}
