﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity.Interaction;


/// <summary>
/// A controller for the Inventory UI
/// </summary>
public class UI_Inventory : MonoBehaviour
{
    private CustomEvents _customEvents;
    private Vector3 _sellObjectSpawnLocation;
    private Quaternion _sellObjectSpawnRotation;
    
    public UI_SellObject sellObjectSeed;
    public Rigidbody SellObjectSeedRigidbody;
    

    public InteractionSlider adjustStack;
    public TMPro.TextMeshPro textMesh;

    // The active inventory slot
    private int _activeIndex;

    // Array of slots that holds the UI elements
    public UI_InventorySlot[] slots = new UI_InventorySlot[5];

    private void Start()
    {
        Transform transform1 = sellObjectSeed.transform;
        _sellObjectSpawnLocation = transform1.position;
        _sellObjectSpawnRotation = transform1.rotation;

        _customEvents = GameObject.Find("Scene Manager").GetComponent<CustomEvents>();
        _customEvents.UpdateInventoryIndex.AddListener(UpdateInventorySlot);
        _customEvents.UpdateActiveInventoryIndex.AddListener(SetActiveIndex);
        _customEvents.AddScore.AddListener(UpdateSlider);

        adjustStack._horizontalSlideEvent.AddListener(AdjustStack);

    }

    void UpdateSlider(int score)
    {
        Debug.LogError("Updating slider");
        InventoryData slotData = GetInventorySlot(_activeIndex);
        adjustStack.horizontalSteps = slotData.amount;
        textMesh.text = GetMarketData(slotData.item).sampleData.collectableData.name + " - " +
                        (GetMarketData(slotData.item).sampleData.points * Mathf.RoundToInt(slotData.amount * adjustStack.HorizontalSliderValue)).ToString();

    }
    
    public void SetActiveIndex(int index)
    {
        _activeIndex = index;
        Debug.Log("Set Active Index: " + index.ToString());
        InventoryData slotData = GetInventorySlot(index);
        adjustStack.horizontalSteps = slotData.amount;

        UpdateSellObject(slotData);
        adjustStack.HorizontalSliderPercent = 1f;
        
        UpdateSlider(0);
    }
    
    
    /// <param name="index"></param>
    /// <returns>The data at the given slot</returns>
    private InventoryData GetInventorySlot(int index)
    {
        return Player.PlayerInstance.inventory[index];
    }

    private void AdjustStack(float percent)
    {
        Debug.Log(percent);
        Debug.Log(_activeIndex);
        InventoryData slotData = GetInventorySlot(_activeIndex);
        slotData.amount = Mathf.RoundToInt((int)slotData.amount * percent);
        UpdateSlider(0);
        UpdateSellObject(slotData);
    }

    /// <param name="itemID"></param>
    /// <returns>The market data at the given Item ID</returns>
    private MarketData GetMarketData(int itemID)
    {

        return Market.MarketInstance.GetDataFromID(itemID);

    }

    [ContextMenu("Update Inventory")]
    public void UpdateInventorySlot(int index)
    {
        Debug.Log(index);
        slots[index].UpdateSlot(GetMarketData(GetInventorySlot(index).item));
        UpdateSlider(0);
    }



    void UpdateSellObject(InventoryData data)
    {
        sellObjectSeed.inventoryData = data;
        SellObjectSeedRigidbody.velocity = Vector3.zero;
        SellObjectSeedRigidbody.angularVelocity = Vector3.zero;
        
        sellObjectSeed.transform.SetPositionAndRotation(_sellObjectSpawnLocation, _sellObjectSpawnRotation);
        sellObjectSeed.gameObject.SetActive(true);
    }

}
