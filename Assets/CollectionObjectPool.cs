using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectToPool;
    private readonly List<GameObject> inactivePool = new List<GameObject>();
    private readonly List<GameObject> activePool = new List<GameObject>();

    public GameObject GetObject()
    {
        GameObject obj = null;
        if (inactivePool.Count > 0)
        {
            obj = inactivePool[0];
            inactivePool.RemoveAt(0);
        }
        else
        {
            obj = Instantiate(objectToPool);
            activePool.Add(obj);
        }
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObjectToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        inactivePool.Add(objectToReturn);
        activePool.Remove(objectToReturn);
    }
}
