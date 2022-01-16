using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : Sample
{
    // This is the more complex class as it gives finite resources
    
    
    // How many this class gives in total
    [SerializeField] protected int _sampleCount;
    // The value of _sampleCount required for the Ore to shatter and give the remaining
    // resources in a massive collection.
    [SerializeField] protected int _shatterCount;




    void Awake()
    {
        CustomEvents.CustomEventsInstance.spawnItems.AddListener(OnSpawn);
    }
    public override void OnSpawn()
    {
        SampleData data;

        if (Market.MarketInstance.samples.TryGetValue(itemID, out data))
        {
            
            _sampleCount = data.sampleCount;
            _shatterCount = data.shatterCount;

        }
    }

    [ContextMenu("On Break")]
    public override void OnBreak(float damage)
    {
        
        // If the player is attempting to shatter the Ore
        if (_sampleCount <= _shatterCount)
        {
            Debug.Log($"<color=#00FFFF>SHATTER ATTEMPT: {_sampleCount}</color>");
            
            int sampleCount = _sampleCount;
            // For the remaining sample count
            for (int i = 0; i < sampleCount; i++)
            {
                // Attempt to get a valid inventory slot ID
                if (Player.PlayerInstance.GetItemIndexFromID(itemID) <= -1)
                {
                    // If no slot exists, break out of the loop
                    Debug.Log("Ran out of space");
                    break;
                }
                // If a valid slot exists, add the Ore
                AddOreCount(_sampleCount);
            }
            
            // If the sample count is greater than 0 then there were some resources left
            // so the game object remains active
            gameObject.SetActive(_sampleCount > 0);

        }
        
        // Player is trying to sample the Ore
        else
        {
            
            Debug.Log($"<color=#00FFFF>TESTING INVENTORY</color>");
            
            // Attempt to get a valid inventory slot ID
            if (Player.PlayerInstance.GetItemIndexFromID(itemID) > -1)
            {
                Debug.Log($"<color=#0000FF>ADDED</color>");
                // If slot exists, add the Ore
                AddOreCount(_sampleCount);

            }
        }
    }

    protected override void AddOreCount(int sample)
    {
        // If the power output from the player is great enough
        if (Player.PlayerInstance.dig.currOutput >= minPower)
        {
            // Add the Ore to the inventory
            if (Player.PlayerInstance.AddItemIDToInventory(itemID))
            {
                Debug.Log($"Added {itemID}, Sample: {sample}.");
                _sampleCount--;
            }
        }

    }

    protected override bool TakeDamage(float damage)
    {
        Debug.Log($"<color=#FF0000>DAMAGED</color>");

        OnBreak(damage);
        return _sampleCount > 0;
    }
}
