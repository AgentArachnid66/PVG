using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public CustomEvents customEvents;

    public GameObject objectToSpawn;

    public Terrain terrain;

    //public int amountToSpawn;
    public float radius;

    public int itemID;

    // Start is called before the first frame update
    void Start()
    {
        customEvents.spawnItems.AddListener(Spawn);

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Spawn Item")]
    public void Spawn()
    {
        Vector3 location = new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));
        Debug.Log(terrain.SampleHeight((location + transform.position)));
    }
}
