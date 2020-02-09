using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRegion : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float detectionRadius;
    private CityRegions currentRegion = CityRegions.TheGates;
    private bool hasSetRegion = false;
    private Ray ray;
    private RaycastHit[] hits;

    private void Update ()
    {
        DetectRegions ();
    }

    private void DetectRegions ()
    {
        Vector3 origin = transform.position + Vector3.up;
        List<CityRegions> regionsDetected = new List<CityRegions> ();

        DetectRegionHit ( origin, regionsDetected );
        DetectRegionHit ( origin + Vector3.right * detectionRadius, regionsDetected );
        DetectRegionHit ( origin + Vector3.left * detectionRadius, regionsDetected );
        DetectRegionHit ( origin + Vector3.forward * detectionRadius, regionsDetected );
        DetectRegionHit ( origin + Vector3.back * detectionRadius, regionsDetected );

        if (regionsDetected.Count == 1)
        {
            if (regionsDetected[0] != currentRegion || hasSetRegion == false)
            {
                OnEnterNewRegion ( regionsDetected[0] );
            }
        }
    }

    private void OnEnterNewRegion (CityRegions newRegion)
    {
        hasSetRegion = true;
        currentRegion = newRegion;
        MiniMapCanvas.instance.UpdateLocationText ( newRegion.ToString ().ToProperCase () );
    }

    private void DetectRegionHit (Vector3 position, List<CityRegions> detectedRegions)
    {
        ray = new Ray ( position, Vector3.down );
        hits = new RaycastHit[0];
        hits = Physics.RaycastAll ( ray, 100.0f, layerMask, QueryTriggerInteraction.Ignore );

        for (int i = 0; i < hits.Length; i++)
        {
            CityRegion region = hits[i].collider.GetComponent<CityRegion> ();
            if (region == null) continue;
            if (detectedRegions.Contains ( region.Region )) continue;
            detectedRegions.Add ( region.Region );
        }
    }
}
