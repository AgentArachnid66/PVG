using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class VacuumObject : MonoBehaviour
{
    public GameObject gatheredObject;
    private Sample sample;

    private bool hasObject;
    
    private void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Sample") && gatheredObject == null)
        {
            
            //joint.
            hasObject = true;
        }
    }

    public void RetrieveObjects(Player player)
    {
        if (hasObject)
        {
            player.AddItemToInventory(sample.data);
            sample = null;
            hasObject = false;
        }
    }
}
