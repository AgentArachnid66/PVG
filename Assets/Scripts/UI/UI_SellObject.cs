using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SellObject : MonoBehaviour
{
    // The object that is responsible for holding the inventory data
    // Player picks this object up from the inventory screen

    public InventoryData inventoryData;

    
    // When it collides with the market object, deposit the inventory data
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Trigger Entered");
        Market market = other.gameObject.GetComponent<Market>();

        if (market != null)
        {
            Debug.LogError("Entered Market");
            market.DepositSample(inventoryData);
            inventoryData = new InventoryData();
            gameObject.SetActive(false);
        }
    }

}
