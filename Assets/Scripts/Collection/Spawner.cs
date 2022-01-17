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
        // Attaches the Spawn function to the appropriate Event call
        CustomEvents.CustomEventsInstance.spawnItems.AddListener(Spawn);

    }

    [ContextMenu("Spawn Item")]
    public void Spawn()
    {
        
        for (int i = 0; i < amountToSpawn; i++)
        {
            // Generates a new location to spawn this instance
            Vector3 location = new Vector3(Random.Range(-radius, radius), 0f, Random.Range(-radius, radius));
            // Adds on the X and Z components of it's own position to translate it from local space to world space
            location.x += transform.position.x;
            location.z += transform.position.z;
            // The Y component is now equal to the height of the terrain at that point
            location.y = terrain.SampleHeight((location));

            // To add some variation in the resources appearance, rotate it randomly
            Quaternion rotation =Random.rotation;

            // Actually spawn in the new resource and give it the appropriate data
            GameObject newObject = GameObject.Instantiate(objectToSpawn.gameObject, location, rotation);
            Sample newSample = newObject.GetComponent<Sample>();
            newSample.itemID = itemID;
            
            newSample.OnSpawn();
            
            // A quick debug to visually see where they all went
            Debug.DrawLine(transform.position, location, Color.white, 200f);
        }
    }
}
