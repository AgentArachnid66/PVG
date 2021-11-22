using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class VacuumObject : MonoBehaviour
{

    public Vector3 collectionPoint;
    public float vacSpeed;
    public float collectionThreshold;

    public Player player;
    private void Start()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Sample"))
        {
            player.VacuumItem(other.gameObject, other.transform.position);
        }
    }
    public void RetrieveObject()
    {
    }
}
