using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int points;
    public CustomEvents customEvents;
    public GameObject menu;

    void Start()
    {
        customEvents.engageThrusters.AddListener(EngageThrusters);
        customEvents.disengageThrusters.AddListener(DisengageThrusters);
        customEvents.hand.AddListener(Hand);
        customEvents.weapon.AddListener(Weapon);

    }


    void Update()
    {


        // If the dot product of the menu's forward vector and the player's forward vector is above 0.98 then open the menu
        Debug.Log(Vector3.Dot(menu.transform.forward, transform.forward));


    }

    void OnCollisionEnter(Collision other)
    {
        Collectable collect = other.gameObject.GetComponent<Collectable>(); 
        if (collect != null)
        {
            points += collect.points;
        }
    }

    void EngageThrusters()
    {
        Debug.Log("Engage Thrusters");
    }

    void DisengageThrusters()
    {
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



}
