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
    private Vector3 leftOrigin;
    private Vector3 rightOrigin;


    public Lever leftControl;
    public Lever rightControl;

    // Only dealing with rotation around the Y axis in the
    // demo to keep things simple
    public float currRotation;
    public float currSum;
    private Vector3 rot;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        /*
          leftControl.OnUpdateLever.AddListener((float result) =>
        {
            UpdateThrusterValues(result, true);
        });

        rightControl.OnUpdateLever.AddListener((float result) =>
        {
            UpdateThrusterValues(result, false);
        });
        */
    }

    void Update()
    {
        // Ideally would like to move away from doing this every frame
        UpdateThrusters();
        Debug.Log("Activate: " + activate.ToString() + " and isActive: " + isActive.ToString());

    }

    public void UpdateThrusterPower()
    {
        if ((activate && isActive) || overrideActive)
        {
            // Gets the distance from the thrusters to the main body
            //Vector3 distanceA = hands.Equals(Hand.Both) | hands.Equals(Hand.Left) ? leftThruster.transform.position - transform.position : new Vector3(0f, 0f, 0f);

            //Vector3 distanceB = hands.Equals(Hand.Both) | hands.Equals(Hand.Right) ? rightThruster.transform.position - transform.position : new Vector3(0f, 0f, 0f);


            // NEW METHOD
            // Controlling clockwise and anti clockwise rotation, with the sum of the distance vectors as the power behind it
            // So the player can turn around and immediately go forward with intuitive ease
            

            // Get the distance from their original positions and use that in the power calculations
            Vector3 distanceA = hands.Equals(Hand.Both) | hands.Equals(Hand.Left) ? leftThruster.transform.position - leftOrigin : Vector3.zero;
            Vector3 distanceB = hands.Equals(Hand.Both) | hands.Equals(Hand.Right) ? rightThruster.transform.position - rightOrigin : Vector3.zero;
        

            // Convert the distance vector to a rotation using the min and max values to lerp between a min and max rotation


            // Projects the distance of the thrusters onto the orientation of the thrusters
            Vector3 thrusterA = Vector3.Project(distanceA, leftThruster.transform.up);
            Vector3 thrusterB = Vector3.Project(distanceB, rightThruster.transform.up);


            // Sums up the resultant vectors to give me the correct output
            Vector3 result = Vector3.ClampMagnitude((thrusterA + thrusterB), maxPower);
            

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




    // Retrieves the relevant values in use for the thrusters
    void UpdateThrusterValues(float rotationPower, bool clockwise)
    {
        if ((activate && isActive) || overrideActive)
        {
            // So it'll add a certain rotation to the vehicle, but based on the percentage
            // it'll also move it forward at a certain rate.

            // Rotation = Sum of clockwise and anticlockwise
            // Forward Velocity = Sum of Rotation Power (clamped between 0 and max power)

            // This method allows for better maneuverability
            rot = m_Rigidbody.transform.rotation.eulerAngles;
            rot.y += clockwise ? rotationPower : (rotationPower * -1);
            currRotation = rot.y;
            currSum += rotationPower;
            currSum = Mathf.Clamp(currSum, 0, maxPower);

        }
    }

    void UpdateThrusters()
    {
        rot = m_Rigidbody.transform.rotation.eulerAngles;
        rot.y += currRotation;
        m_Rigidbody.MoveRotation(Quaternion.Euler(rot));

        m_Rigidbody.MovePosition(m_Rigidbody.position + (m_Rigidbody.transform.forward * currSum));

        // Resets the currSum at the end of the frame, ready for 
        // the next update
        currSum = 0;
        
    }
}
