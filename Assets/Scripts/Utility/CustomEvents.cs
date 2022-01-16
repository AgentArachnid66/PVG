using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class CustomEvents : MonoBehaviour
{
    private static CustomEvents customEvents;
    
    public static CustomEvents CustomEventsInstance
    {
        get
        {
            if (ReferenceEquals(customEvents, null))
                customEvents = GameObject.FindObjectOfType<CustomEvents>();

            return customEvents;
        }
    }

    
    public UnityEvent spawnItems = new UnityEvent();
    
    public UnityEventMode switchMode = new UnityEventMode();

    public UnityEventInt AddScore = new UnityEventInt();

    public UnityEventInt UpdateInventoryIndex = new UnityEventInt();

    public UnityEventInt UpdateActiveInventoryIndex = new UnityEventInt();

    public UnityEventLeapPosOrient UpdateHandPosition = new UnityEventLeapPosOrient();

    public UnityEventPosOrient UpdateLaser = new UnityEventPosOrient();
    
    private void Start()
    {
        switchMode.AddListener(testSwitch);
        spawnItems.Invoke();
    }

    void testSwitch(Mode mode, Hand hand)
    {
        Debug.LogWarning("Switched to " + mode.ToString() + " on the " + hand.ToString() + " hand");
    }


}

#region Custom Event Types  

[System.Serializable]
public class UnityEventBool : UnityEvent<bool>
{

}

public class UnityEventLeapPosOrient :UnityEvent<bool, Leap.Vector, Leap.Vector>
{

}

public class UnityEventMode : UnityEvent<Mode, Hand>
{
    // Mode for the type of equipment is active
    // the bool is for which hand it is for (not implemented yet)
}

public class UnityEventInt : UnityEvent<int>
{

}

public class UnityEventPosOrient : UnityEvent<bool, Vector3, Vector3>
{
}

public class UnityEventFloat : UnityEvent<float>
{
}

#endregion

#region Custom Enums

[System.Serializable]
public enum Mode
{
    Thruster,
    Hand,
    Collection,
    Menu
}

[System.Serializable]
public enum Hand
{
    None,
    Left,
    Right,
    Both
}

[System.Serializable]
public enum NodeType
{
    Unpure,
    Normal,
    Pure
}

#endregion

#region Custom Structs

[System.Serializable]
public struct SampleData
{
    public CollectableData collectableData;
    public int points;
    public int sampleCount;
    public int shatterCount;
    // Add in image at a later date

    public SampleData(CollectableData data, int points, int sample, int shatter)
    {
        this.collectableData = data;
        this.points = points;
        sampleCount = sample;
        shatterCount = shatter;
    }
}

[System.Serializable]
public struct CollectableData
{
    public string name;
    public int itemID;
    public int maxStackSize;

    public CollectableData(string itemName, int id, int mss)
    {
        name = itemName;
        itemID = id;
        maxStackSize = mss;
    }

}

[System.Serializable]
public struct MarketData
{
    public SampleData sampleData;
}

[System.Serializable]
public struct InventoryData
{
    public int item;
    public int amount;

    public InventoryData(int itemID = -1, int amount = 0)
    {
        item = itemID;
        this.amount = amount;
    }
}

[System.Serializable]
public struct HandData
{
    public bool isLeft;
    public Vector3 location;
    public Vector3 orientation;
}

[System.Serializable]
public struct NodeData
{
    public NodeType type;
}

#endregion