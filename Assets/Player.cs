﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class Player : MonoBehaviour
{
    public int points;
    public CustomEvents customEvents;
    public GameObject menu;
    public Thruster thruster;
    [SerializeField]
    public InventoryData[] inventory = new InventoryData[5];
    public float ejectionMultiplier;
    public Rigidbody test;

    void Start()
    {
        // Set up so that when the player presses the right button, it will activate the thrusters and when they release it, they will stop.
        //customEvents.engageThrusters.AddListener(EngageThrusters);
        //customEvents.disengageThrusters.AddListener(DisengageThrusters);


        customEvents.switchMode.AddListener(SwitchMode);

        customEvents.toggleThrusters.AddListener((bool result) =>
        {
            thruster.ToggleThrusters(result);
        });


        // Leap Motion
        //customEvents.UpdateHandPosition.AddListener(LeapUpdateThrusterPosition);


        customEvents.AddScore.AddListener(AddScore);
        customEvents.shootItem.AddListener(ShootItem);
    }


    void Update()
    {
        // If the dot product of the menu's forward vector and the player's forward vector is above 0.98 then open the menu
        //Debug.Log(Vector3.Dot(menu.transform.forward, transform.forward));


    }

    #region Mode Mechanics

    void SwitchMode(Mode mode, bool hand)
    {
        thruster.ToggleActive(false);

        switch (mode)
        {
            case Mode.None:
                break;

            case Mode.Thruster:
                thruster.ToggleActive(true);
                break;

            case Mode.Weapon:
                break;

            case Mode.Hand:
                break;
        }
    }

    void EngageThrusters()
    {
        thruster.ToggleThrusters(true);
        Debug.Log("Engage Thrusters");
    }

    void DisengageThrusters()
    {
        thruster.ToggleThrusters(false);
        Debug.Log("Disengage Thrusters");
    }

    void Hand()
    {
        Debug.Log("Hand");
    }

    void Weapon()
    {
        Debug.Log("Weapon");
    }

    void LeapUpdateThrusterPosition(bool isLeft, Leap.Vector position)
    {
        Vector3 unityPosition = new Vector3(position.x, position.y, position.z);
        GameObject active = isLeft ? thruster.leftThruster : thruster.rightThruster;

        active.transform.position = unityPosition;
    }

    #endregion

    #region Movement








    #endregion

    #region Score System

    void DepositSample()
    {
    }

    void AddScore(int deltaScore)
    {
        points += deltaScore;
    }

    #endregion

    #region Inventory

    void OnCollisionEnter(Collision other)
    {
        Collectable collect = other.gameObject.GetComponent<Collectable>();


        if (collect != null)
        {

        }
    }

    // Gets the first available index for an item if there isn't a stack already in the inventory. If there is a stack then it returns that
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
            }

            // Checks if a stack of the relevant items exists already
            if (inventory[i].amount > 1 && inventory[i].item == data.itemID)
            {
                // Checks to see if there is any space left in the stack
                if (inventory[i].amount <= data.maxStackSize)
                {
                    index = i;
                    found = true;
                }
            }
        }

        return index;
    }

    public bool AddItemToInventory(CollectableData data)
    {
        return AddToInventory(data, GetItemIndex(data));
    }

    // Adds an item to the player inventory
    bool AddToInventory(CollectableData data, int index)
    {

        // Double checks that it's adding to the correct stack, and that the addition won't make the stack overflow
        if (data.itemID == inventory[index].item && inventory[index].amount <= data.maxStackSize)
        {
            // Just adds one to the amount of a certain item in the inventory
            inventory[index].amount += 1;
            Debug.Log("Just added " + data.name + " to the inventory");
            return true;
        }

        else
        {
            return false;
        }
    }


    
    // Removes items from the player inventory
    bool RemoveFromInventory(int index, int amount)
    {
        // Checks if there is enough of the item to remove from the inventory to prevent negative amounts of an item
        if(inventory[index].amount >= amount)
        {
            inventory[index].amount -= amount;
            return true;
        }

        else
        {
            return false;
        }
    }

    void ShootItem()
    {
        int index = 0;
        if(inventory[index].amount >= 1)
        {
            Vector3 dir = transform.forward * ejectionMultiplier;
            test.gameObject.SetActive(true);
            test.AddForce(dir);
            RemoveFromInventory(index, 1);

        }

    }

    #endregion


}
