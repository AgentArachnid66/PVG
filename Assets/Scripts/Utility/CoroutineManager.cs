using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance;
    public static CoroutineManager Instance
    {
        get
        {
            if (ReferenceEquals(_instance, null))
                _instance = GameObject.FindObjectOfType<CoroutineManager>();

            return _instance;
        }
    }

}
