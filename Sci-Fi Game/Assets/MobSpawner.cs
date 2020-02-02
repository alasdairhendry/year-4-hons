﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private float radius = 10.0f;
    [SerializeField] private int maxInstances = 10;
    [SerializeField] private float respawnDelay = 10.0f;
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private bool populateOnAwake = true;

    private List<NPC> currentInstances = new List<NPC> ();
    private float currentRespawnDelay = 0.0f;
    Ray ray = new Ray ();
    RaycastHit hit;

    private void Awake ()
    {
        if (populateOnAwake)
        {
            SpawnMax ();
        }
    }

    public void SpawnMax ()
    {
        for (int i = 0; i < maxInstances; i++)
        {
            SpawnInstance ();
        }
    }

    public void DespawnAll ()
    {
        for (int i = 0; i < currentInstances.Count; i++)
        {
            Destroy ( currentInstances[i].gameObject );
        }
    }

    private void Update ()
    {
        if (currentInstances.Count <= maxInstances - 1)
        {
            currentRespawnDelay += Time.deltaTime;

            if (currentRespawnDelay >= respawnDelay)
            {
                SpawnInstance ();
            }
        }
    }

    private void OnInstanceDeath(NPC npc)
    {
        if (currentInstances.Contains ( npc ))
        {
            currentInstances.Remove ( npc );
        }
        else
        {
            Debug.LogError ( "Instance does not exist", this.gameObject );
        }
    }

    private void SpawnInstance ()
    {
        if (currentInstances.Count >= maxInstances) return;

        GameObject instance = Instantiate ( mobPrefab );
        instance.transform.SetParent ( this.transform );

        Vector3 spawnPosition = Random.insideUnitSphere * radius;
        ray = new Ray ( spawnPosition + (Vector3.up * radius * 4), Vector3.down );
        hit = new RaycastHit ();

        if(Physics.Raycast(ray, out hit, radius * 10 ))
        {
            spawnPosition = hit.point + (Vector3.up * 0.25f);
        }
        else
        {
            Debug.LogError ( "Error ray casting suitable terrain" );
        }

        instance.transform.localPosition = spawnPosition;
        instance.transform.localEulerAngles = new Vector3 ( 0.0f, Random.Range ( 0.0f, 360.0f ), 0.0f );

        NPC npc = instance.GetComponent<NPC> ();
        currentInstances.Add ( npc );
        npc.OnDeathAction += () => { OnInstanceDeath ( npc ); };
        currentRespawnDelay = 0.0f;
    }

    private void OnDrawGizmosSelected ()
    {
        Gizmos.DrawWireSphere ( transform.position, radius );
    }
}