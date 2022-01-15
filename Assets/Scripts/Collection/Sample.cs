using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Sample : Collectable
{
    public int itemID;

    public int healthSegments = 5;

    private bool canBeDamaged = true;

    // Minimum amount of power required to break this sample
    public float minPower;


    void OnCollisionEnter(Collision other)
    {


    }

    private void Awake()
    {
        CustomEvents.CustomEventsInstance.spawnItems.AddListener(OnSpawn);
    }
    public virtual void OnSpawn()
    {
        Debug.Log("Parent Spawn");
    }

    [ContextMenu("On Break")]
    public virtual void OnBreak(float damage)
    {
        Debug.Log("Parent Break");
    }

    //private IEnumerator AddOreCount(int sample)
    protected virtual void AddOreCount(int sample)
    {
        //Start 0.1 second animation of collection

        //yield return new WaitForSeconds(0.1f);

        if (Player.PlayerInstance.AddItemIDToInventory(itemID))
        {
            Debug.Log($"Added {itemID}, Sample: {sample}.");
            //_sampleCount--;
        }


    }
    
    protected virtual bool TakeDamage(float damage)
    {
        Debug.Log($"<color=#FF0000>DAMAGED</color>");

        OnBreak(damage);
        return true;
    }

    public bool PlayerDamage(float damage)
    {
        return TakeDamage(damage);
    }

}
