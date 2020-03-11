using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PoolManager
{
    private static Dictionary<int, Queue<PooledObject>> poolDictionary = new Dictionary<int, Queue<PooledObject>> ();

    public static List<PooledObject> CreatePool (GameObject prefab, int poolSize, string objectName = "Pooled Object", Transform parent = null)
    {
        int poolKey = prefab.GetInstanceID ();

        if (!poolDictionary.ContainsKey ( poolKey ))
        {
            List<PooledObject> pooledObjects = new List<PooledObject> ();

            poolDictionary.Add ( poolKey, new Queue<PooledObject> () );

            for (int i = 0; i < poolSize; i++)
            {
                PooledObject pooledObject = new PooledObject ( GameObject.Instantiate ( prefab ) );
                poolDictionary[poolKey].Enqueue ( pooledObject );
                pooledObject.gameObject.name = objectName;

                if (parent != null)
                {
                    pooledObject.gameObject.transform.SetParent ( parent );
                }

                pooledObjects.Add ( pooledObject );
            }

            return pooledObjects;
        }
        else
        {
            Debug.LogError ( "Instance ID " + poolKey + " already exists" );
            return new List<PooledObject> ();
        }
    }

    public static GameObject Instantiate (GameObject prefab, Vector3 position, Quaternion rotation, bool isMandatory = false)
    {
        int poolKey = prefab.GetInstanceID ();

        if (poolDictionary.ContainsKey ( poolKey ))
        {
            PooledObject pooledObject = poolDictionary[poolKey].Dequeue ();
            poolDictionary[poolKey].Enqueue ( pooledObject );
            pooledObject.OnInstantiate ( position, rotation );
            return pooledObject.gameObject;
        }
        else
        {
            Debug.LogError ( "Instance ID " + poolKey + " does not exist" );
            return null;
        }
    }

    public static void Destroy (GameObject gameObject)
    {
        gameObject.SetActive ( false );
    }

    public class PooledObject
    {
        public GameObject gameObject;
        public IPoolable[] iPoolables = new IPoolable[0];

        public PooledObject (GameObject gameObject)
        {
            this.gameObject = gameObject;
            this.iPoolables = gameObject.GetComponentsInChildren<IPoolable> ();
            gameObject.SetActive ( false );
        }

        public void OnInstantiate (Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive ( true );
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;

            for (int i = 0; i < iPoolables.Length; i++)
            {
                iPoolables[i].OnInstantiated ();
            }
        }
    }
}
