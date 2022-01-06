using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : Sample
{
    [SerializeField] protected int _sampleCount;
    [SerializeField] protected int _shatterCount;


    void OnCollisionEnter(Collision other)
    {
        Market market = other.gameObject.GetComponent<Market>();

        if (market != null)
        {
            market.DepositSample(itemID);

        }


    }


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

        if (_sampleCount <= _shatterCount)
        {
            Debug.Log($"<color=#00FFFF>SHATTER ATTEMPT: {_sampleCount}</color>");
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

            }
        }
    }

    //private IEnumerator AddOreCount(int sample)
    protected override void AddOreCount(int sample)
    {
        //Start 0.1 second animation of collection

        //yield return new WaitForSeconds(0.1f);
        if (Player.PlayerInstance.dig.currOutput >= minPower)
        {
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
