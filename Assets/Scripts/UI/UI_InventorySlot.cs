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
    public int index;
    private TextMesh _text;
    private CustomEvents _customEvents;

    void Start()
    {
        _customEvents = GameObject.Find("Scene Manager").GetComponent<CustomEvents>();
    }
    public void UpdateSlot(MarketData data)
    {
        itemName = data.sampleData.collectableData.name;
        _text.text = itemName;
    }

    public void SelectSlot()
    {
        // Need to feedback to the inventory controller which slot this is
        // to set it as the active slot
    }

}
