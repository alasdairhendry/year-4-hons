using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CityRegionController
{
    private static LayerMask layerMask = 1 << 13;
    private static Ray ray;
    private static RaycastHit[] hits;

    public static CityRegions GetRegion (Vector3 worldPosition, float detectionRadius = 1)
    {
        CityRegions region = CityRegions.None;

        region = DetectRegionHit ( worldPosition );
        if (region != CityRegions.None) return region;

        DetectRegionHit ( worldPosition + Vector3.right * detectionRadius );
        if (region != CityRegions.None) return region;

        DetectRegionHit ( worldPosition + Vector3.left * detectionRadius );
        if (region != CityRegions.None) return region;

        DetectRegionHit ( worldPosition + Vector3.forward * detectionRadius );
        if (region != CityRegions.None) return region;

        DetectRegionHit ( worldPosition + Vector3.back * detectionRadius );
        if (region != CityRegions.None) return region;

        return CityRegions.None;
    }

    private static CityRegions DetectRegionHit (Vector3 position)
    {
        ray = new Ray ( position, Vector3.down );
        hits = new RaycastHit[0];
        hits = Physics.RaycastAll ( ray, 100.0f, layerMask, QueryTriggerInteraction.Ignore );

        for (int i = 0; i < hits.Length; i++)
        {
            CityRegion region = hits[i].collider.GetComponent<CityRegion> ();
            if (region == null) continue;
            return region.Region;
        }

        return CityRegions.None;
    }
}
