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
    public GameObject leftThrusterUI;
    public GameObject rightThrusterUI;
    public GameObject leftMarketUI;
    public GameObject rightMarketUI;
    void Start()
    {
        _customEvents = GameObject.Find("Scene Manager").GetComponent<CustomEvents>();
        ToggleMarketUI(Hand.Both, false);
        ToggleThrusterUI(Hand.Both, false);
    }
    
    #region Upgrade Screen


    
    // Increases the max speed of the thrusters
    public void IncreaseSpeed(float percentageIncrease)
    {
        thrusters.maxPower *= 1 + (percentageIncrease/100f);
    }
    
    
    
    #endregion

    
    #region UI Events

    
    
    public void ToggleThrusterUI(Hand hand, bool active)
    {
        Debug.Log("Thruster UI is" +active.ToString() + " " +hand.ToString());
        switch (hand)
        {
            case(Hand.Both):
                leftThrusterUI.SetActive(active);
                rightThrusterUI.SetActive(active);
                break;
            
            case(Hand.Left):
                leftThrusterUI.SetActive(active);
                break;
            
            case(Hand.Right):
                rightThrusterUI.SetActive(active);
                break;
            
            case(Hand.None):
                break;
        }
    }

    public void ToggleMarketUI(Hand hand, bool active)
    {
        Debug.Log("Market UI is" +active.ToString() + " " +hand.ToString());
        switch (hand)
        {
            case(Hand.Both):
                leftMarketUI.SetActive(active);
                rightMarketUI.SetActive(active);
                break;
            
            case(Hand.Left):
                leftMarketUI.SetActive(active);
                break;
            
            case(Hand.Right):
                rightMarketUI.SetActive(active);
                break;
            
            case(Hand.None):
                break;
        }
    }
    
    #endregion
}