using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public GameObject leftThruster;
    public GameObject rightThruster;

    public float maxPower;

    private Rigidbody m_Rigidbody;

    
    private bool activate;
    public bool overrideActive;
    public bool isActive;

    private Hand hands;

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
            Vector3 distanceA = hands.Equals(Hand.Both) | hands.Equals(Hand.Left) ? leftThruster.transform.position - transform.position : new Vector3(0f, 0f, 0f);

            Vector3 distanceB = hands.Equals(Hand.Both) | hands.Equals(Hand.Right) ? rightThruster.transform.position - transform.position : new Vector3(0f, 0f, 0f);


            // Projects the distance of the thrusters onto the orientation of the thrusters
            Vector3 thrusterA = Vector3.Project(distanceA, leftThruster.transform.up);
            Vector3 thrusterB = Vector3.Project(distanceB, rightThruster.transform.up);


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
