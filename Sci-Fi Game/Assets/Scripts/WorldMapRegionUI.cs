using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldMapRegionUI : MonoBehaviour
{
    [SerializeField] private CityRegions region = CityRegions.TheGates;
    private Image image;

    private void Awake ()
    {
        image = GetComponent<Image> ();
        image.alphaHitTestMinimumThreshold = 0.5f;
    }
}
