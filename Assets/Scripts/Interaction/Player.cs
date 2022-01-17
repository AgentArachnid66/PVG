using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Player : MonoBehaviour
{

    private static Player playerInstance;
    public static Player PlayerInstance
    {
        get
        {
            if (ReferenceEquals(playerInstance, null))
                playerInstance = GameObject.FindObjectOfType<Player>();

            return playerInstance;
        }
    }

    private Mode currentMode;
    
    public int points;
    public CustomEvents customEvents;


    public UI_Controller uiController;
    public Thruster thruster;
    public Digging dig;


    public Market market;
    [SerializeField]
    public InventoryData[] inventory = new InventoryData[5];
    
    void Start()
    {

        customEvents.switchMode.AddListener(SwitchMode);
        customEvents.AddScore.AddListener(AddScore);

        SwitchMode(Mode.Hand, Hand.Both);

    }

    #region Mode Mechanics


    // Switches the given hand's active mode. 
    // It's independent so that either hand can be given a different task depending on the player's decisions
    void SwitchMode(Mode mode, Hand hand)
    {
        currentMode = mode;

        switch (mode)
        {
            case Mode.Thruster:
                // Switch off UI for other modes
                uiController.ToggleMarketUI(hand, false);
                
                // Switch on UI for this mode
                uiController.ToggleThrusterUI(hand, true);
                
                // Turn off Laser on the hand/s
                dig.ToggleBeam(hand, false);
                
                // Enable the thrusters
                thruster.ToggleThrusters(true);

                break;
                
            case Mode.Hand:
                // Turn off everything
                dig.ToggleBeam(hand, false);
                thruster.ToggleThrusters(false);
                
                uiController.ToggleMarketUI(hand, false);
                uiController.ToggleThrusterUI(hand, false);
                break;

            case Mode.Collection:
                
                // Turn off Thruster
                thruster.ToggleThrusters(false);
                
                // Turn off Thruster UI
                uiController.ToggleThrusterUI(hand, false);
                
                // Turn off Market UI
                uiController.ToggleMarketUI(hand, false);
                
                // Turn on Laser
                dig.ToggleBeam(hand, true);
                
                break;

            case Mode.Menu:
                
                // Turn off Thruster
                thruster.ToggleThrusters(false);
                
                // Turn off Laser
                dig.ToggleBeam(hand, false);
                
                // Turn off thruster UI
                uiController.ToggleThrusterUI(hand, false);
                
                // Turn on Buy/Sell Menu
                uiController.ToggleMarketUI(hand, true);
                
                break;
        }
    }



    #endregion



    public bool SpendPoints(int price)
    {
        bool success = points >= price;
        if (success)
        {
            points -= price;
        }
        
        uiController.UpdateScore(points);
        return success;
    }

    void AddScore(int deltaScore)
    {
        points += deltaScore;
        uiController.UpdateScore(points);
    }


    #region Inventory


    // Gets the first available index for an item if there isn't a stack already in the inventory.
    // If there is a stack then it returns that
    int GetItemIndex(CollectableData data)
    {
        int index = -1;
        bool found = false;
        for (int i = 0; i < inventory.Length; i++)
        {
            // If the slot is empty then it saves the index and continues with the loop in case a stack with the right items exists
            if (inventory[i].amount == 0 && !found)
            {
                index = i;
                found = true;
                //Debug.Log("Saved first empty slot: " +index.ToString());
            }

            // Checks if a stack of the relevant items exists already inventory[i].amount > 1 && 
            if (inventory[i].item == data.itemID && inventory[i].amount  < data.maxStackSize)
            {
                index = i;
                found = true;
                //Debug.Log(("Stack with relevant item with space left in stack at index: " +i.ToString()));
                
            }
        }
        
        //Debug.Log(("Saved index: " +index.ToString()));
        return index;
    }

    
    // Using the item id, this will get an index for the inventory.
    public int GetItemIndexFromID(int id)
    {
        SampleData info;
        Market.MarketInstance.samples.TryGetValue(id, out info);
        return GetItemIndex(info.collectableData);

    }
    
    // Given an ItemID, adds a single instance of it to the inventory
    public bool AddItemIDToInventory(int itemID)
    {
        if (market.samples.TryGetValue(itemID, out SampleData dataOut)){
            Debug.Log(dataOut.ToString());
            return AddItemToInventory(dataOut.collectableData);
        }
        else
        {
            return false;
        }
         
    }


    // Given some CollectableData, add it to the inventory
    public bool AddItemToInventory(CollectableData data)
    {
        return AddToInventory(data, GetItemIndex(data));
    }

    // Adds an item to the player inventory
    bool AddToInventory(CollectableData data, int index)
    {
        if(index < 0)
        {
            return false;
        }
        
        Debug.Log(inventory[index].amount.ToString() +" in Inventory at slot " +index.ToString());
        Debug.Log("Max Stack Size for item: " +data.maxStackSize.ToString());
        
        // Double checks that it's adding to the correct stack, and that the addition won't make the stack overflow
        if (data.itemID == inventory[index].item && inventory[index].amount  < data.maxStackSize)
        {
            // Just adds one to the amount of a certain item in the inventory
            inventory[index].amount += 1;
            Debug.Log("Just added " + data.name + " to the inventory in slot: " +index.ToString());
            customEvents.UpdateInventoryIndex.Invoke(index);
            return true;
        }

        else if(inventory[index].amount <= 0)
        {
            inventory[index].item = data.itemID;
            inventory[index].amount += 1;
            Debug.Log("Just added " + data.name + " to the inventory in slot: " +index.ToString());
            customEvents.UpdateInventoryIndex.Invoke(index);
            
            return true;
        }

        else
        {
            return false;
        }
    }
    
    // Removes items from the player inventory
    public bool RemoveFromInventory(int index, int amount)
    {
        if (index < 0)
        {
            return false;
        }
        // Checks if there is enough of the item to remove from the inventory to prevent negative amounts of an item
        if(inventory[index].amount >= amount)
        {
            customEvents.UpdateInventoryIndex.Invoke(index);
            inventory[index].amount -= amount;
            inventory[index].item = inventory[index].amount > 0 ? inventory[index].item : 0;
            return true;
        }

        else
        {
            return false;
        }
    }


    #endregion


}
