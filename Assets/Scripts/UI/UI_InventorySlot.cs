using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;

public class UI_InventorySlot : MonoBehaviour
{
    // The slot display itself, it will take it's 
    // direction from the UI_Inventory class
    // to retrieve it's data

    public string itemName;
    public TextMesh text;
    
    public void UpdateSlot(MarketData data)
    {
        itemName = data.sampleData.collectableData.name;
        text.text = itemName;
    }

}
