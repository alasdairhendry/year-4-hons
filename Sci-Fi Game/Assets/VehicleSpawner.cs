using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    [SerializeField] [Range ( 0.0f, 1.0f )] private float spawnChance = 0.75f;
    private List<VehicleSpawnPosition> vehicleSpawnPositions = new List<VehicleSpawnPosition> ();

    private void Awake ()
    {
        InitialiseSpawnPoints ();
        SpawnInitialVehicles ();
    }

    private void InitialiseSpawnPoints ()
    {
        vehicleSpawnPositions = GetComponentsInChildren<VehicleSpawnPosition> ().ToList ();

        for (int i = 0; i < vehicleSpawnPositions.Count; i++)
        {
            vehicleSpawnPositions[i].Initialise ();
        }
    }

    private void SpawnInitialVehicles ()
    {
        for (int i = 0; i < vehicleSpawnPositions.Count; i++)
        {
            if(Random.value <= spawnChance || vehicleSpawnPositions[i].requiredSpawn)
            {
                SpawnVehicle ( vehicleSpawnPositions[i] );
            }
        }
    }

    private void SpawnVehicle ()
    {
        List<VehicleSpawnPosition> eligibleSpawnPositions = vehicleSpawnPositions.Where ( x => x.registeredVehicle == null || (x.transform.position - x.registeredVehicle.transform.position).sqrMagnitude > 25 ).ToList ();
        if (eligibleSpawnPositions.Count <= 0) return;
        SpawnVehicle ( eligibleSpawnPositions.GetRandom () );
    }

    private void SpawnVehicle (VehicleSpawnPosition vehicleSpawnPosition)
    {
        GameObject go = Instantiate ( vehicleSpawnPosition.GetRandomVehicle () );
        go.transform.position = vehicleSpawnPosition.transform.position;
        go.transform.rotation = vehicleSpawnPosition.transform.rotation;
        go.transform.localScale = Vector3.one;
        vehicleSpawnPosition.registeredVehicle = go;
        go.GetComponent<Health> ().onDeath += () => { vehicleSpawnPosition.registeredVehicle = null; SpawnVehicle (); };
    }
}
