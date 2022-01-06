using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Thruster : MonoBehaviour
{
 
    public float maxPower;
    public float rotationMultiplier;

    private Rigidbody m_Rigidbody;

    
    private bool activate;
    public bool isActive;

    // For Testing Purposes to ensure that this is working
    public bool overrideActive;

    private Hand hands;


    public Lever leftControl;
    public Lever rightControl;

    // Only dealing with rotation around the Y axis in the
    // demo to keep things simple
    private float currRotation;
    private float currSum;
    private Vector3 rot;

    private Vector3 clockwiseVector;
    private Vector3 anticlockwiseVector;


    public float testRot;
    public float testSum;
    public float dot;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        
          leftControl.OnUpdateLever.AddListener((float result) =>
        {
            UpdateThrusterValues(result, true);
        });

        rightControl.OnUpdateLever.AddListener((float result) =>
        {
            UpdateThrusterValues(result, false);
        });
        
    }

    void Update()
    {
        // Ideally would like to move away from doing this every frame
        UpdateThrusters();
        //Debug.Log("Activate: " + activate.ToString() + " and isActive: " + isActive.ToString());

    }

    public void UpdateThrusterPower()
    {
        if ((activate && isActive) || overrideActive)
        {
            // Gets the distance from the thrusters to the main body
            //Vector3 distanceA = hands.Equals(Hand.Both) | hands.Equals(Hand.Left) ? leftThruster.transform.position - transform.position : new Vector3(0f, 0f, 0f);

            //Vector3 distanceB = hands.Equals(Hand.Both) | hands.Equals(Hand.Right) ? rightThruster.transform.position - transform.position : new Vector3(0f, 0f, 0f);


            

            // Get the distance from their original positions and use that in the power calculations
            //Vector3 distanceA = hands.Equals(Hand.Both) | hands.Equals(Hand.Left) ? leftThruster.transform.position - leftOrigin : Vector3.zero;
            //Vector3 distanceB = hands.Equals(Hand.Both) | hands.Equals(Hand.Right) ? rightThruster.transform.position - rightOrigin : Vector3.zero;
        

            // Convert the distance vector to a rotation using the min and max values to lerp between a min and max rotation


            // Projects the distance of the thrusters onto the orientation of the thrusters
            //Vector3 thrusterA = Vector3.Project(distanceA, leftThruster.transform.up);
            //Vector3 thrusterB = Vector3.Project(distanceB, rightThruster.transform.up);


            // Sums up the resultant vectors to give me the correct output
            //Vector3 result = Vector3.ClampMagnitude((thrusterA + thrusterB), maxPower);
            

            //Debug.Log(result);

            // Affects the velocity of the main body
            //m_Rigidbody.velocity = result;
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


            //rot = m_Rigidbody.transform.rotation.eulerAngles;
            //rot.y += clockwise ? rotationPower : (rotationPower * -1);
            //currRotation = rot.y;

            // Alternative Method
            // Get the Dot product between the current forward vector 
            // and the vector with the rotation applied
            // Use the product of this and a multiplier as the Delta rotation 

            Debug.Log(clockwise.ToString() + " Rotation: " + Mathf.Lerp(-90, 90, rotationPower).ToString());

            // Rotates the forward vector by the current Rotation
            if (clockwise)
            {
                clockwiseVector = Quaternion.Euler(0,Mathf.Lerp(-90, 90, rotationPower), 0) * m_Rigidbody.transform.forward;
                //Debug.DrawLine(m_Rigidbody.transform.position, m_Rigidbody.transform.position + clockwiseVector * 500);
            }
            else
            {
                anticlockwiseVector = Quaternion.Euler(0,Mathf.Lerp(-90, 90, 1-rotationPower), 0) * m_Rigidbody.transform.forward;
                //Debug.DrawLine(m_Rigidbody.transform.position, m_Rigidbody.transform.position + anticlockwiseVector * 500);
            }



            currSum += rotationPower;
            currSum = Mathf.Clamp(currSum, 0, maxPower);

        }
    }

    void UpdateThrusters()
    {
        // Finds the Dot Product between the sum of the rotation vectors and the transform's forward vector
        dot = Vector3.Dot((clockwiseVector + anticlockwiseVector).normalized, m_Rigidbody.transform.forward);

        // Since the Dot Product would be 1 when they are parallel (ie player doesn't want to rotate)
        // and 0 when they are at 90 degrees (ie player wants to do a sharper turn)
        // take 1 minus the dot to get the intended results then multiply by a float to easily control the output
        currRotation =(1- dot) * rotationMultiplier;
        rot = m_Rigidbody.rotation.eulerAngles;
        rot.y += currRotation;
        m_Rigidbody.MoveRotation(Quaternion.Euler(rot));

        m_Rigidbody.MovePosition(m_Rigidbody.position + (m_Rigidbody.transform.forward * currSum));

        // Resets the currSum at the end of the frame, ready for 
        // the next update
        currSum = 0f;
        
    }

    private void OnDrawGizmos()
    {

    }
}
