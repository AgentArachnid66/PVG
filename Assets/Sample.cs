using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : Collectable
{


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

}
