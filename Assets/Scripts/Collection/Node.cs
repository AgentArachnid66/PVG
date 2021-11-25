using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : Sample
{
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
        if (Player.PlayerInstance.AddItemIDToInventory(itemID))
        {
            Debug.Log($"Added {itemID}, Sample: {sample}.");
        }
    }


    protected override bool TakeDamage(float damage)
    {
        Debug.Log($"<color=#FF0000>DAMAGED</color>");

        OnBreak(damage);
        return true;
    }


}
