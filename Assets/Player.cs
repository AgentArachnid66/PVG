using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;

public class Player : MonoBehaviour
{
    private Mode currentMode;
    [SerializeField] private CollectionObjectPool objectpoolVacuum;
    
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
            //thruster.ToggleThrusters(result);
        });


        InvokeRepeating("AddNewVacuumItem", 0, 0.2f);
        
        // Leap Motion
        customEvents.UpdateHandPosition.AddListener(LeapUpdateThrusterPosition);


        customEvents.AddScore.AddListener(AddScore);
        customEvents.shootItem.AddListener(ShootItem);



    }


    void Update()
    {
        // If the dot product of the menu's forward vector and the player's forward vector is above 0.98 then open the menu
        //Debug.Log(Vector3.Dot(menu.transform.forward, transform.forward));

        

    }

    #region Mode Mechanics

    void SwitchMode(Mode mode, Hand hand)
    {
        currentMode = mode;
        //thruster.ToggleActive(false);

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
                Debug.Log("Saved first empty slot: " +index.ToString());
            }

            // Checks if a stack of the relevant items exists already inventory[i].amount > 1 && 
            if (inventory[i].item == data.itemID)
            {
                Debug.Log("Stack with relevant item at index: " +i.ToString());
                
                // Checks to see if there is any space left in the stack
                if (inventory[i].amount <= data.maxStackSize)
                {
                    index = i;
                    found = true;
                    Debug.Log(("Stack with relevant item with space left in stack at index: " +i.ToString()));
                }
            }
        }
        
        Debug.Log(("Added to index: " +index.ToString()));
        return index;
    }

    public bool AddItemToInventory(CollectableData data)
    {
        return AddToInventory(data, GetItemIndex(data));
    }

    // Adds an item to the player inventory
    bool AddToInventory(CollectableData data, int index)
    {
        
        Debug.Log(inventory[index].amount.ToString() +" in Inventory at slot " +index.ToString());
        Debug.Log("Max Stack Size for item: " +data.maxStackSize.ToString());
        
        // Double checks that it's adding to the correct stack, and that the addition won't make the stack overflow
        if (data.itemID == inventory[index].item && inventory[index].amount < data.maxStackSize)
        {
            // Just adds one to the amount of a certain item in the inventory
            inventory[index].amount += 1;
            Debug.Log("Just added " + data.name + " to the inventory in slot: " +index.ToString());
            
            return true;
        }

        else if(inventory[index].amount <= 0)
        {
            inventory[index].item = data.itemID;
            inventory[index].amount += 1;
            Debug.Log("Just added " + data.name + " to the inventory in slot: " +index.ToString());
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


    
    IEnumerator BeginVacuumItem(GameObject item , Vector3 spawn , float waitTime)
    {
        float elapsedTime = 0;
        item.transform.position = spawn;
 
        while (elapsedTime < waitTime)
        {
            Vector3 target = collectionPoint.transform.position;
            item.transform.position = Vector3.Lerp(spawn, target, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
 
            // Yield here
            yield return null;
        }  
        // Make sure we got there
        item.transform.position = collectionPoint.transform.position;

        item.GetComponent<VacuumObject>().RetrieveObjects(this);
        
        objectpoolVacuum.ReturnObjectToPool(item);
        
        yield return null;
    }
    
    //
    [ContextMenu("Add New Vacuum Item")]
    void AddNewVacuumItem()
    {
        if (currentMode == Mode.Collection)
        {
            //Debug.Log("Adding new Vacuum Item");
            GameObject vacuumItem = objectpoolVacuum.GetObject();
            StartCoroutine(BeginVacuumItem( vacuumItem,
                collectionPoint.transform.position + (collectionPoint.transform.forward * maxCollectionDistance),
                vacuumSpeed));
           
            
        }
    }


    
    #endregion


}
