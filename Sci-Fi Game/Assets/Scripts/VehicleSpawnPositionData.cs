using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VehicleSpawnPositionData : ScriptableObject
{
    [NaughtyAttributes.ReorderableList] public List<GameObject> possibleVehicles = new List<GameObject> ();
}
