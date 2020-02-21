using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationBeam : MonoBehaviour
{
    [SerializeField] private CityRegions region;
    [SerializeField] private Transform spawnPosition;

    public CityRegions Region { get => region; set => region = value; }
    public Transform SpawnPosition { get => spawnPosition; set => spawnPosition = value; }

    public void Teleport(Transform transform)
    {
        transform.position = spawnPosition.position;
    }
}
