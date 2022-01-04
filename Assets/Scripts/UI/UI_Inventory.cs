using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller for the Inventory UI
/// </summary>
public class UI_Inventory : MonoBehaviour
{
    public CustomEvents customEvents;
    public Transform sellObjectSpawnLocation;
    public GameObject sellObjectSeed;


    // The active inventory slot
    private int activeIndex;

    // Array of slots that holds the UI elements
    public UI_InventorySlot[] slots = new UI_InventorySlot[5];

    private void Start()
    {
        customEvents.UpdateInventoryIndex.AddListener(UpdateInventorySlot);
    }

    /// <param name="index"></param>
    /// <returns>The data at the given slot</returns>
    private InventoryData GetInventorySlot(int index)
    {
        return Player.PlayerInstance.inventory[index];
    }


    /// <param name="itemID"></param>
    /// <returns>The market data at the given Item ID</returns>
    private MarketData GetMarketData(int itemID)
    {
        return Market.MarketInstance.itemData[itemID];
    }

    [ContextMenu("Update Inventory")]
    /// <summary>
    /// Given the index, it will update the information associated with it
    /// in the relevant UI component
    /// </summary>
    /// <param name="index"></param>
    public void UpdateInventorySlot(int index)
    {
        slots[index].UpdateSlot(GetMarketData(GetInventorySlot(index).item));
    }

    public void DisplayActiveInventorySlot()
    {
       
    }

    [ContextMenu("Test Generate Sell Object")]
    public void TestGenerateSellObject()
    {
        Debug.Log("Generated Test Sell Object");
        GenerateSellObject(new InventoryData());
    }

    /// <summary>
    /// This will generate a selling object that can be picked up by the player and then deposited into
    /// the market object to generate an income
    /// </summary>
    /// <param name="data"></param>
    public void GenerateSellObject(InventoryData data)
    {
        // Ideally would like this to be an object pool but to get 
        // it working at this stage, instantiating as needed is alright

        GameObject newObject = Instantiate(sellObjectSeed, sellObjectSpawnLocation);
        newObject.GetComponent<UI_SellObject>().inventoryData = data;
    }

}
