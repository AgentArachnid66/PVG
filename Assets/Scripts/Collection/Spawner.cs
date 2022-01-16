using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Sample objectToSpawn;

    public Terrain terrain;

    public int amountToSpawn;
    public float radius;
    public int itemID;

    // Start is called before the first frame update
    void Start()
    {
        CustomEvents.CustomEventsInstance.spawnItems.AddListener(Spawn);

    }

    [ContextMenu("Spawn Item")]
    public void Spawn()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 location = new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));
            location.x += transform.position.x;
            location.z += transform.position.z;
            location.y = terrain.SampleHeight((location));
            Quaternion rotation = Quaternion.Euler(location);

            GameObject newObject = GameObject.Instantiate(objectToSpawn.gameObject, location, rotation);
            Sample newSample = newObject.GetComponent<Sample>();
            newSample.itemID = itemID;
            
            newSample.OnSpawn();
            
            Debug.DrawLine(transform.position, location, Color.white, 200f);
        }
    }
}
