using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Controller : MonoBehaviour
{
    // This class will control the UI elements depending on 
    // voice commands and leap motion input
    // It will also handle the upgrades on the Buy Menu
    
    private CustomEvents _customEvents;
    public Thruster thrusters;

    void Start()
    {
        _customEvents = GameObject.Find("Scene Manager").GetComponent<CustomEvents>();
    }
    
    #region Upgrade Screen


    
    // Increases the max speed of the thrusters
    public void IncreaseSpeed(float percentageIncrease)
    {
        thrusters.maxPower *= 1 + (percentageIncrease/100f);
    }
    
    
    
    #endregion

}