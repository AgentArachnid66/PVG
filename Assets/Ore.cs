using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    enum OreType { GOLD };

    [SerializeField] private int itemID;

    [SerializeField] private int _sampleCount;
    [SerializeField] private int _shatterCount;

    public void OnSpawn()
    {

    }

    [ContextMenu("On Break")]
    public void OnBreak()
    {
        if (_sampleCount <= _shatterCount)
        {
            int sampleCount = _sampleCount;
            for (int i = 0; i < sampleCount; i++)
            {
                CoroutineManager.Instance.StartCoroutine(AddOreCount(_sampleCount));
                _sampleCount--;
            }

            Destroy(gameObject);
        }
        else
        {
            CoroutineManager.Instance.StartCoroutine(AddOreCount(_sampleCount));
            _sampleCount--;
        }
    }

    private IEnumerator AddOreCount(int sample)
    {
        yield return new WaitForSeconds(1);

        if (Player.PlayerInstance.AddItemIDToInventory(itemID))
        {
            Debug.Log($"Added {itemID}, Sample: {sample}.");
        }

    }
}
