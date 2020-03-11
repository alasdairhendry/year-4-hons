using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransformBasedPrefabSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject> ();
    [SerializeField] private List<Transform> spawnPositions = new List<Transform> ();
    [Space]
    [SerializeField] private int spawnLimit = 10;
    [SerializeField] private bool spawnLimitOnAwake = true;
    [SerializeField] private float spawnDelay = 5;

    protected float currentSpawnCounter = 0.0f;
    protected List<GameObject> spawnedObjects = new List<GameObject> ();

    protected List<Transform> availableSpawnPositions
    {
        get
        {
            return spawnPositions.Where ( x => x.childCount == 0 ).ToList ();
        }
    }

    private void Awake ()
    {
        if (spawnLimitOnAwake)
        {
            for (int i = 0; i < spawnLimit; i++)
            {
                Spawn ();
            }
        }
    }

    private void Update ()
    {
        CheckNullObjects ();
        CheckNextSpawn ();
    }

    protected virtual void CheckNullObjects ()
    {
        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt ( i );
            }
        }
    }

    protected virtual void CheckNextSpawn ()
    {
        if (spawnedObjects.Count < spawnLimit)
        {
            currentSpawnCounter += Time.deltaTime;

            if(currentSpawnCounter >= spawnDelay)
            {
                Spawn ();
            }
        }
    }

    public virtual void Spawn ()
    {
        currentSpawnCounter = 0.0f;

        GameObject spawnedObject = Instantiate ( prefabs.GetRandom () );
        spawnedObject.transform.SetParent ( availableSpawnPositions.GetRandom () );
        spawnedObject.transform.parent.SetAsFirstSibling ();

        spawnedObject.transform.localPosition = Vector3.zero;
        spawnedObject.transform.localRotation = Quaternion.identity;
        spawnedObject.transform.localScale = Vector3.one;

        spawnedObjects.Add ( spawnedObject );
    }
}
