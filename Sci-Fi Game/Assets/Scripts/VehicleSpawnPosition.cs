using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawnPosition : MonoBehaviour
{
    [NaughtyAttributes.ReorderableList] public List<VehicleSpawnPositionData> possibleVehiclesCollections = new List<VehicleSpawnPositionData> ();
    [NaughtyAttributes.ReorderableList] public List<GameObject> possibleVehiclesPrefabs = new List<GameObject> ();
    public bool requiredSpawn = false;
    public GameObject registeredVehicle;

    private List<GameObject> allPrefabs = new List<GameObject> ();

    public void Initialise ()
    {
        for (int i = 0; i < possibleVehiclesCollections.Count; i++)
        {
            for (int x = 0; x < possibleVehiclesCollections[i].possibleVehicles.Count; x++)
            {
                allPrefabs.Add ( possibleVehiclesCollections[i].possibleVehicles[x] );
            }
        }

        for (int i = 0; i < possibleVehiclesPrefabs.Count; i++)
        {
            allPrefabs.Add ( possibleVehiclesPrefabs[i] );
        }
    }

    public GameObject GetRandomVehicle ()
    {
        return allPrefabs.GetRandom ();
    }
}
