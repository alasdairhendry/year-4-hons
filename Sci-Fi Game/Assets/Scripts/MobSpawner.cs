using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private float radius = 10.0f;
    [SerializeField] private int maxInstances = 10;
    [SerializeField] private float respawnDelay = 10.0f;
    [SerializeField] private GameObject mobPrefab;
    [SerializeField] private bool populateOnAwake = true;
    [SerializeField] private bool populateOnUpdate = true;

    private List<NPC> currentInstances = new List<NPC> ();
    private float currentRespawnDelay = 0.0f;
    Ray ray = new Ray ();
    RaycastHit hit;

    public float Radius { get => radius; set => radius = value; }
    public bool PopulateOnAwake { get => populateOnAwake; set => populateOnAwake = value; }
    public bool PopulateOnUpdate { get => populateOnUpdate; set => populateOnUpdate = value; }

    private void Awake ()
    {
        if (populateOnAwake)
        {
            SpawnMax ();
        }
    }

    public void SpawnSetAmount(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnInstance ();
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
        if (populateOnUpdate == false) return;
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

        Vector3 spawnPosition = this.transform.position + Random.insideUnitSphere * radius;

        NavMeshHit navMeshHit;
        bool sampledPositionSuccessfully = false;
        int sampleCount = 0;

        do
        {
            sampledPositionSuccessfully = NavMesh.SamplePosition ( spawnPosition, out navMeshHit, 8.0f, NPCNavMesh.WalkableAreaMask );
            sampleCount++;

            if (sampleCount >= 5)
                break;
        } while (sampledPositionSuccessfully == false);

        if(sampledPositionSuccessfully == false)
        {
            Destroy ( instance );
            return;
        }

        instance.transform.position = navMeshHit.position;
        instance.transform.localEulerAngles = new Vector3 ( 0.0f, Random.Range ( 0.0f, 360.0f ), 0.0f );
        instance.transform.SetParent ( this.transform );

        NPC npc = instance.GetComponent<NPC> ();
        npc.SetMobSpawner ( this );
        currentInstances.Add ( npc );
        npc.OnDeathAction += () => { OnInstanceDeath ( npc ); };
        npc.OnDeleteAction += () => { OnInstanceDeath ( npc ); };
        currentRespawnDelay = 0.0f;
    }

    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = new Color ( 0.0f, 1.0f, 0.0f, 0.5f );
        Gizmos.DrawSphere ( transform.position, radius );
    }
}
