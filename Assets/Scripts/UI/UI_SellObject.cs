using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SellObject : MonoBehaviour
{
    // The object that is responsible for holding the inventory data
    // Player picks this object up from the inventory screen

    public InventoryData inventoryData;

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning("Trigger Entered");
        Market market = other.gameObject.GetComponent<Market>();

        if (market != null)
        {
            Debug.LogError("Entered Market");
            market.DepositSample(inventoryData);
            gameObject.SetActive(false);
        }
    }

}
