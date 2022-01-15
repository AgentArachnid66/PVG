using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    private static Market marketInstance;
    public static Market MarketInstance
    {
        get
        {
            if (ReferenceEquals(marketInstance, null))
                marketInstance = GameObject.FindObjectOfType<Market>();

            return marketInstance;
        }
    }

    // Dictionary that takes in an item ID as a key to access how much each is worth

    public Dictionary<int, SampleData> samples = new Dictionary<int, SampleData>();
    private int _active;
    
    [SerializeField]
    public List<MarketData> itemData = new List<MarketData>();

    public void Start()
    {
        for(int i =0; i <itemData.Count; i++)
        {
            MarketData item = itemData[i];
            samples.Add(item.sampleData.collectableData.itemID, item.sampleData);
        }

        CustomEvents.CustomEventsInstance.spawnItems.Invoke();
        CustomEvents.CustomEventsInstance.UpdateActiveInventoryIndex.AddListener(SetActiveIndex);
    }

    void SetActiveIndex(int index)
    {
        _active = index;
    }
    public MarketData GetDataFromID(int id)
    {
        for (int i = 0; i < itemData.Count; i++)
        {
            if (id == itemData[i].sampleData.collectableData.itemID)
            {
                return itemData[i];
            }
        }

        return new MarketData();
    }

    public void DepositSample(InventoryData item)
    {
        SampleData value;
        if (samples.TryGetValue(item.item, out value))
        {
            Debug.Log("Gotten Item and added score");
            Player.PlayerInstance.RemoveFromInventory(_active, item.amount);
            CustomEvents.CustomEventsInstance.AddScore.Invoke(value.points * item.amount);
        }
    }

}
