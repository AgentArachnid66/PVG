using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    // The slot display itself, it will take it's 
    // direction from the UI_Inventory class
    // to retrieve it's data

    public string itemName;
    private Text text;

    public void UpdateSlot(MarketData data)
    {
        itemName = data.sampleData.collectableData.name;
        text.text = itemName;
    }
}
