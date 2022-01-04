using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SellObject : MonoBehaviour
{
    // The object that is responsible for holding the inventory data
    // Player picks this object up from the inventory screen 
    // They then can either deposit this item into the market or
    // dispose of it as they wish

    public InventoryData inventoryData;

    private void OnTriggerEnter(Collider other)
    {

        Market market = other.gameObject.GetComponent<Market>();

        if (market != null)
        {
            market.DepositSample(inventoryData.item);

        }
    }

}
