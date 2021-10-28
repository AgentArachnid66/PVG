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


}
