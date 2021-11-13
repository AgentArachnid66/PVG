using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;




public class CustomEvents : MonoBehaviour
{
    public UnityEvent engageThrusters = new UnityEvent();

    public UnityEvent disengageThrusters = new UnityEvent();

    public UnityEvent hand = new UnityEvent();

    public UnityEvent weapon = new UnityEvent();

    public UnityEvent shootItem = new UnityEvent();



    public UnityEventBool toggleThrusters = new UnityEventBool();

    public UnityEventMode switchMode = new UnityEventMode();

    public UnityEventInt AddScore = new UnityEventInt();

    public UnityEventLeapVector UpdateHandPosition = new UnityEventLeapVector();

    public UnityEventLeapTransform UpdateLaser = new UnityEventLeapTransform();



    public bool test;

    [ContextMenu("Test Toggle Thrusters")]
    public void TestToggle()
    {
        toggleThrusters.Invoke(test);
    }



    // This is where I will check to see if the player has interacted with any controls
    void Update()
    {
        // Switch to different modes
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Debug.Log("Thrusters");
            switchMode.Invoke(Mode.Thruster, false);
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            Debug.Log("Clear");
            switchMode.Invoke(Mode.None, false);
        }


        // Use those modes
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Activate Thrusters");
            toggleThrusters.Invoke(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Deactivate Thrusters");
            toggleThrusters.Invoke(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Fire Inventory Item");
            shootItem.Invoke();
        }
    }
}
  

[System.Serializable]
public class UnityEventBool : UnityEvent<bool>
{

}

[System.Serializable]
public enum Mode
{
    None,
    Thruster,
    Weapon,
    Hand
}

[System.Serializable]
public enum Hand
{
    None,
    Left,
    Right,
    Both
}

public class UnityEventMode : UnityEvent<Mode, bool>
{
    // Mode for the type of equipment is active
    // the bool is for which hand it is for (not implemented yet)
}

public class UnityEventInt : UnityEvent<int>
{

}

public class UnityEventLeapVector : UnityEvent<bool ,Leap.Vector>
{

}

public class UnityEventLeapTransform : UnityEvent<bool, Transform>
{
    
}

public struct SampleData
{
    public CollectableData collectableData;
    public int points;

    // Add in image at a later date
}

[System.Serializable]
public struct CollectableData
{
    public string name;
    public int itemID;
    public int maxStackSize;

}

[System.Serializable]
public struct InventoryData
{
    public int item;
    public int amount;

}