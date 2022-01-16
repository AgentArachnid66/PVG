using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : Sample
{
    // This is a more simple class as it gives infinite resources of a specific type that is low in 
    // value on the market. For more information on functions, please see the parent script (Sample)
    
    public NodeData data;

    public override void OnSpawn()
    {

    }
    

    public override void OnBreak(float damage)
    {
        AddOreCount(this.itemID);
    }

    protected override void AddOreCount(int sample)
    {
        if (Player.PlayerInstance.dig.currOutput >= minPower)
        {
            if (Player.PlayerInstance.AddItemIDToInventory(itemID))
            {
                Debug.Log($"Added {itemID}, Sample: {sample}.");
            }
        }
    }


    protected override bool TakeDamage(float damage)
    {
        Debug.Log($"<color=#FF0000>DAMAGED</color>");

        OnBreak(damage);
        return true;
    }


}
