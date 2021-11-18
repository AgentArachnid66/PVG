using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;

    public Terrain terrain;

    //public int amountToSpawn;
    public float radius;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 location = new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));
        Debug.Log(terrain.SampleHeight((location + transform.position)));

    }

    // Update is called once per frame
    void Update()
    {

    }
}
