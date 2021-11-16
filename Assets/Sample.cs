using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Sample : Collectable
{

    public float health;

    void OnCollisionEnter(Collision other)
    {
        Market market = other.gameObject.GetComponent<Market>();
        Player player = other.gameObject.GetComponent<Player>();

        if (player != null)
        {
            player.AddItemToInventory(data);
        }

        if(market != null)
        {
            market.DepositSample(data.itemID);

        }

    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        throw new System.NotImplementedException();
    }

}
