using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CityRegions { TheGates, Midtown, BaysideFort, FadaPark, NorthernTerritory, SouthernTerritory, UpperVale, LowerVale }

public class CityRegion : MonoBehaviour
{
    [SerializeField] private CityRegions region;
    public CityRegions Region { get => region; }


}
