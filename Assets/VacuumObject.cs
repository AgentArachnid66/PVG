using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumObject : MonoBehaviour
{
    public List<Sample> gatheredObjects = new List<Sample>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            other.transform.parent = transform;
            other.attachedRigidbody.isKinematic = false;
            gatheredObjects.Add(other.gameObject.GetComponent<Sample>());
        }
    }



    public void RetrieveObjects(Player player)
    {
        Debug.Log("Collided with Player");
        foreach (Sample objects in gatheredObjects)
        {
            if (player.AddItemToInventory(objects.data))
            {
                objects.transform.parent = null;
                objects.gameObject.SetActive(false);
            }
        }
        transform.gameObject.SetActive(false);
    }
}
