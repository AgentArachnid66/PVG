﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Player : MonoBehaviour
{
    private Mode currentMode;

    public int points;
    public CustomEvents customEvents;
    public GameObject menu;
    public Thruster thruster;
    public Digging dig;
    [SerializeField]
    public InventoryData[] inventory = new InventoryData[5];


    public float maxCollectionDistance;
    public float vacuumSpeed;
    public Transform collectionPoint;

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


        InvokeRepeating("AddNewVacuumItem", 0, 0.1f);
        
        // Leap Motion
        customEvents.UpdateHandPosition.AddListener(LeapUpdateThrusterPosition);


        customEvents.AddScore.AddListener(AddScore);
        customEvents.shootItem.AddListener(ShootItem);



    }


    void Update()
    {
        // If the dot product of the menu's forward vector and the player's forward vector is above 0.98 then open the menu
        //Debug.Log(Vector3.Dot(menu.transform.forward, transform.forward));

        VacuumItems();

    }

    #region Mode Mechanics

    void SwitchMode(Mode mode, Hand hand)
    {
        currentMode = mode;
        thruster.ToggleActive(false);

        switch (mode)
        {
            case Mode.None:
                dig.ToggleBeam(global::Hand.None);
                break;

            case Mode.Thruster:
                thruster.ToggleActive(true);
                break;

            case Mode.Weapon:
                dig.ToggleBeam(hand);
                break;

            case Mode.Hand:
                break;

            case Mode.Collection:
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
    }

    void LeapUpdateThrusterPosition(bool isLeft, Leap.Vector position)
    {
        Vector3 unityPosition = position.ToUnityVector3();
        GameObject active = isLeft ? thruster.leftThruster : thruster.rightThruster;

        active.transform.position = unityPosition;
    }

    void LeapUpdateLaser()
    {
        

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

    // The main way that the player will add items to the Inventory
    void VacuumItems()
    {
        List<GameObject> active = CollectionObjectPool.collectionsInstance.GetActiveObjects();
        if (active != null && active.Count>=1)
        {
            for (int i = 0; i < active.Count; i++)
            {
                Vector3 dir = (collectionPoint.transform.position - active[i].transform.position).normalized;
                active[i].transform.position = active[i].transform.position + (dir * vacuumSpeed);

                if (Vector3.Distance(active[i].transform.position, transform.position) <= 0.5)
                {
                    active[i].GetComponent<VacuumObject>().RetrieveObjects(this);
                }
            }
        }
    }

    [ContextMenu("Add New Vacuum Item")]
    void AddNewVacuumItem()
    {
        if (currentMode == Mode.Collection)
        {
            //Debug.Log("Adding new Vacuum Item");
            GameObject vacuumItem = CollectionObjectPool.collectionsInstance.GetPooledObject();
            if (vacuumItem != null)
            {
                vacuumItem.transform.position = collectionPoint.transform.position + (collectionPoint.transform.forward * maxCollectionDistance);
                vacuumItem.SetActive(true);
            }
        }
    }


    
    #endregion


}
