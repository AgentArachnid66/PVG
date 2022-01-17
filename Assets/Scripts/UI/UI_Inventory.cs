using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;


/// <summary>
/// A controller for the Inventory UI
/// </summary>
public class UI_Inventory : MonoBehaviour
{
    // Initial Transform of the Sell Object
    private Vector3 _sellObjectSpawnLocation;
    private Quaternion _sellObjectSpawnRotation;
    
    // The actual Sell Object and it's RigidBody
    public UI_SellObject sellObjectSeed;
    public Rigidbody SellObjectSeedRigidbody;
    

    // UI elements
    public InteractionSlider adjustStack;
    public TMPro.TextMeshPro textMesh;

    // The active inventory slot
    private int _activeIndex;

    
    // Array of slots that holds the UI elements
    public UI_InventorySlot[] slots = new UI_InventorySlot[5];

    private void Start()
    {
        // Sets the initial transform to the relevant values
        Transform transform1 = sellObjectSeed.transform;
        _sellObjectSpawnLocation = transform1.position;
        _sellObjectSpawnRotation = transform1.rotation;

        // Attaches functions to Events
        CustomEvents.CustomEventsInstance.UpdateInventoryIndex.AddListener(UpdateInventorySlot);
        CustomEvents.CustomEventsInstance.UpdateActiveInventoryIndex.AddListener(SetActiveIndex);
        CustomEvents.CustomEventsInstance.AddScore.AddListener(UpdateSlider);
        adjustStack._horizontalSlideEvent.AddListener(AdjustStack);

    }

    // Updates the Slider UI
    void UpdateSlider(int score)
    {
        Debug.LogError("Updating slider");
        InventoryData slotData = GetInventorySlot(_activeIndex);
        adjustStack.horizontalSteps = slotData.amount;
        textMesh.text = GetMarketData(slotData.item).sampleData.collectableData.name + " - " +
                        (GetMarketData(slotData.item).sampleData.points * Mathf.RoundToInt(slotData.amount * adjustStack.HorizontalSliderValue)).ToString();

    }

    // Sets the Currently Active Index
    public void SetActiveIndex(int index)
    {
        _activeIndex = index;
        Debug.Log("Set Active Index: " + index.ToString());
        InventoryData slotData = GetInventorySlot(_activeIndex);
        
        // Updates the Slider to match the new number of steps
        adjustStack.horizontalSteps = slotData.amount;


        UpdateSellObject(slotData);
        adjustStack.HorizontalSliderPercent = 1f;
        UpdateSlider(0);
    }


    // Gets the Inventory Slot given an index
    private InventoryData GetInventorySlot(int index)
    {
        return Player.PlayerInstance.inventory[index];
    }

    // Adjusts the stack given an percentage
    private void AdjustStack(float percent)
    {
        Debug.Log(percent);
        Debug.Log(_activeIndex);
        
        
        InventoryData slotData = GetInventorySlot(_activeIndex);
        slotData.amount = Mathf.RoundToInt((int)slotData.amount * percent);
        UpdateSlider(0);
        UpdateSellObject(slotData);
    }

    // Given an item ID, retrieves the corresponding MarketData
    private MarketData GetMarketData(int itemID)
    {

        return Market.MarketInstance.GetDataFromID(itemID);

    }

    // Updates the Inventory Slot
    public void UpdateInventorySlot(int index)
    {
        Debug.Log(index);
        slots[index].UpdateSlot(GetMarketData(GetInventorySlot(index).item));
        UpdateSlider(0);
    }
    
    // Updates the Sell Object to match the input InventoryData
    void UpdateSellObject(InventoryData data)
    {
        sellObjectSeed.inventoryData = data;
        
        // Resets the physics
        SellObjectSeedRigidbody.velocity = Vector3.zero;
        SellObjectSeedRigidbody.angularVelocity = Vector3.zero;
        
        // Resets the transform and makes it visible again
        sellObjectSeed.transform.SetPositionAndRotation(_sellObjectSpawnLocation, _sellObjectSpawnRotation);
        sellObjectSeed.gameObject.SetActive(true);
    }

}
