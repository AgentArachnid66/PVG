using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    public List<SampleData> samples = new List<SampleData>();
    public CustomEvents customEvents;



    void DepositSample(SampleData data)
    {

        if (samples.Contains(data))
        {
            SampleData active = samples[data.collectableData.itemID];

            customEvents.AddScore.Invoke(active.points);
        }
    }

}
