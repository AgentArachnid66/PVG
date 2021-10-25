using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thruster : MonoBehaviour
{
    public GameObject thrusterOne;
    public GameObject thrusterTwo;

    public float maxPower;

    private Rigidbody m_Rigidbody;


    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ideally would like to move away from doing this every frame
        UpdateThrusterPower();
    }

    [ContextMenu("Test Thrusters")]
    public void UpdateThrusterPower()
    {
        // Gets the distance from the thrusters to the main body
        Vector3 distanceA = thrusterOne.transform.position - transform.position;
        Vector3 distanceB = thrusterTwo.transform.position - transform.position;


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
