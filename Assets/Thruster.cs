using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public GameObject thrusterOne;
    public GameObject thrusterTwo;

    public float maxPower;

    private Rigidbody m_Rigidbody;

    
    private bool activate;
    public bool overrideActive;
    public bool isActive;


    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ideally would like to move away from doing this every frame
        UpdateThrusterPower();
        Debug.Log("Activate: " + activate.ToString() + " and isActive: " + isActive.ToString());

    }

    public void UpdateThrusterPower()
    {
        if ((activate && isActive) || overrideActive)
        {
            // Gets the distance from the thrusters to the main body
            Vector3 distanceA = thrusterOne.activeSelf ? thrusterOne.transform.position - transform.position : new Vector3(0f, 0f, 0f);

            Vector3 distanceB = thrusterTwo.activeSelf ? thrusterTwo.transform.position - transform.position : new Vector3(0f, 0f, 0f);


            // Projects the distance of the thrusters onto the orientation of the thrusters
            Vector3 thrusterA = Vector3.Project(distanceA, thrusterOne.transform.up);
            Vector3 thrusterB = Vector3.Project(distanceB, thrusterTwo.transform.up);


            // Sums up the resultant vectors to give me the correct output
            Vector3 result = thrusterA + thrusterB;
            if (result.magnitude > maxPower)
            {
                result = result.normalized * maxPower;
            }

            Debug.Log(result);

            // Affects the velocity of the main body
            m_Rigidbody.velocity = result;
        }

    }

    public void ToggleThrusters(bool activateThrusters)
    {
        activate = activateThrusters;
        Debug.Log("Toggle Thrusters: " + activateThrusters.ToString());
    }
    
    public void ToggleActive(bool active)
    {
        isActive = active;

    }

    
}
