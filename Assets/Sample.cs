using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Sample : Collectable
{
    public int itemID;
    [SerializeField] private int _sampleCount;
    [SerializeField] private int _shatterCount;

    public int healthSegments = 5;

    private bool canBeDamaged = true;
    void OnCollisionEnter(Collision other)
    {
        Market market = other.gameObject.GetComponent<Market>();

        if(market != null)
        {
            market.DepositSample(itemID);

        }

    }

    private void Awake()
    {
        CustomEvents.CustomEventsInstance.spawnItems.AddListener(OnSpawn);
    }
    public void OnSpawn()
    {
        SampleData data;

        if(Market.MarketInstance.samples.TryGetValue(itemID, out data))
        {
            _sampleCount = data.sampleCount;
            _shatterCount = data.shatterCount;

        }
    }

    [ContextMenu("On Break")]
    public void OnBreak(float damage)
    {
        if (_sampleCount <= _shatterCount)
        {
            Debug.Log($"<color=#00FFFF>SHATTER ATTEMPT</color>");
            int sampleCount = _sampleCount;
            for (int i = 0; i < sampleCount; i++)
            {
                if (Player.PlayerInstance.GetItemIndexFromID(itemID) <= -1)
                {
                    Debug.Log("Ran out of space");
                    break;
                }
                //CoroutineManager.Instance.StartCoroutine(AddOreCount(_sampleCount));
                AddOreCount(_sampleCount);
                _sampleCount--;
            }


            gameObject.SetActive(_sampleCount > 0);
        }
        else
        {
            Debug.Log($"<color=#00FFFF>TESTING INVENTORY</color>");
            if (Player.PlayerInstance.GetItemIndexFromID(itemID) > -1)
            {
                Debug.Log($"<color=#0000FF>ADDED</color>");
                //CoroutineManager.Instance.StartCoroutine(AddOreCount(_sampleCount));
                AddOreCount(_sampleCount);
                _sampleCount--;
            }
        }
    }

    //private IEnumerator AddOreCount(int sample)
    private void AddOreCount(int sample)
    {


        //yield return new WaitForSeconds(0.1f);

        if (Player.PlayerInstance.AddItemIDToInventory(itemID))
        {
            Debug.Log($"Added {itemID}, Sample: {sample}.");
           // canBeDamaged = true;
        }
        else
        {
           // canBeDamaged = false;
        }
    }
    
    public bool TakeDamage(float damage)
    {
        Debug.Log($"<color=#FF0000>DAMAGED</color>");
        //if (canBeDamaged)
        {
            OnBreak(damage);
            //canBeDamaged = false;
            
        }

        Debug.Log($"<color=#00FF00>Health: {_sampleCount}</color>");
        return _sampleCount > 0;
    }

}
