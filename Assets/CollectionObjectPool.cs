using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionObjectPool : MonoBehaviour
{
    public static CollectionObjectPool collectionsInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    private void Awake()
    {
        collectionsInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        Debug.Log("Retrieving Pooled Object");
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }

    public List<GameObject> GetActiveObjects()
    {
        List<GameObject> active = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            if (pooledObjects[i].activeInHierarchy)
            {
                active.Add(pooledObjects[i]);
            }
        }

        return active;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
