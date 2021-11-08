using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{

    // Dictionary that takes in an item ID as a key to access how much each is worth

    public Dictionary<int, int> samples = new Dictionary<int, int>();


    public CustomEvents customEvents;

    public void Start()
    {
        samples.Add(0, 10);

    }



    public void DepositSample(int itemID)
    {
        int value;
        if (samples.TryGetValue(itemID, out value))
        {

            customEvents.AddScore.Invoke(value);
        }
    }

}
