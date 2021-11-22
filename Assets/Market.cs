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
    }



    public void DepositSample(int itemID)
    {
        SampleData value;
        if (samples.TryGetValue(itemID, out value))
        {

            CustomEvents.CustomEventsInstance.AddScore.Invoke(value.points);
        }
    }

}
