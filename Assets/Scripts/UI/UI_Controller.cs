using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_Controller : MonoBehaviour
{
    // This class will control the UI elements depending on 
    // voice commands and leap motion input
    // It will also handle the upgrades on the Buy Menu
    
    public Thruster thrusters;
    public GameObject leftThrusterUI;
    public GameObject rightThrusterUI;
    public GameObject leftMarketUI;
    public GameObject rightMarketUI;
    public TextMesh scoreDisplay;
    
    void Start()
    {
        // Immediately turn off all UI that are active
        ToggleMarketUI(Hand.Both, false);
        ToggleThrusterUI(Hand.Both, false);
    }
    
    #region Upgrade Screen
    
    // This is where I will keep the Upgrades



    // Increases the max speed of the thrusters
    public void IncreaseSpeed(GameObject button)
    {
        if (Player.PlayerInstance.SpendPoints(10))
        {
            Debug.Log("Increase Speed of Drone");
            thrusters.maxPower *= 1 + (50 / 100f);
            button.SetActive(false);
        }
    }
    
    
    
    #endregion

    
    #region UI Events

    // This is where I put the UI related functionality
    
    // Updates the Points UI
    public void UpdateScore(int newScore)
    {
        scoreDisplay.text = "Current Points: " + newScore.ToString();
    }
    
    // Toggles the Thruster UI based on which hand and if it's closing or opening
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

    // Toggles the Market UI based on which hand and if it's closing or opening
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