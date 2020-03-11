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
    private CityController cityController;

    private void Awake ()
    {
        cityController = FindObjectOfType<CityController> ();
    }

    private void Update ()
    {
        DetectRegions ();
    }

    private void DetectRegions ()
    {
        CityRegions regionDetected = CityRegionController.GetRegion ( transform.position + Vector3.up, detectionRadius );

        if (regionDetected != CityRegions.None)
        {
            if (regionDetected != currentRegion || hasSetRegion == false)
            {
                OnEnterNewRegion ( regionDetected );
            }

            cityController.OnCharacterEnterCity ();
        }
        else
        {
            cityController.OnCharacterLeaveCity ();
        }
    }

    private void OnEnterNewRegion (CityRegions newRegion)
    {
        hasSetRegion = true;
        currentRegion = newRegion;
        MiniMapCanvas.instance.UpdateLocationText ( newRegion.ToString ().ToProperCase () );
    }
}
