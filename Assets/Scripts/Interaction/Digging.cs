using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digging : MonoBehaviour
{
    [SerializeField] private float _cooldown = 0.25f;
    [SerializeField] private bool _isInCooldown;

    [SerializeField] private LayerMask mask;
    /*
     * <summary>
     * This class is in charge of the digging portion of the gameplay loop
     * 
     * If the player chooses to use both hands
     * 
        * The mining laser will be controlled by the distance between the two hands and a burn out meter
     * 
     * If the player chooses to use only one hand
     * 
        * The mining laser will be controlled by the grab strength of the hand and the burn out meter, but it won't ever be as strong
        *  as the hands together
     * 
     * 
     * You'd have a quicker time digging the closer your hands are, but the burn out meter will expend itself more for the piece
     * you are trying to collect
     * 
     * Some samples are too delicate to be taken by the higher power laser, whilst others are unable to be dug out by the lower
     * power laser. 
     * </summary>
     */




    public GameObject combineTest;
    public GameObject[] hitTests = new GameObject[2];
    
    public bool active;

    public Transform rig;
    public CustomEvents customEvents;
    // The threshold value that if the player exceeds, the laser requires a cooldown before it can be used again
    public float maxBurnout;

    // What the laser is currently outputting. To be read by the samples to see how it compares to it's threshold values
    public float currOutput;

    // The vector between the hands, to measure the distance between them and to get the orientation
    private Vector3 handDist;

    private Vector3[] laserDir = new Vector3[2];
    private Vector3[] laserPos = new Vector3[2];

    // Enum that holds which hands are active in this mode
    public Hand hands;

    // Bool to determine if the player wants to use 2 different beams or combine them for the more powerful version
    public bool combine;

    public HandData leftHand;
    public HandData rightHand;

    public Sample activeSample;

    public float debugLineLength;
    public float testDot;

    private void Start()
    {
        customEvents.UpdateLaser.AddListener(UpdateBeamInfo);
    }

    void UpdateBeam()
    {
        // If the hands are facing each other within a threshold, then the beams will combine
        // Otherwise they will act independently

        // First check if the laser is active
        if (active)
        {
            laserDir[1] = Vector3.zero;
            laserPos[1] = Vector3.zero;

            // Then check if the player has both hands as lasers
            if (hands == Hand.Both)
            {
                Vector3 pointDist = (rightHand.location + (rightHand.orientation * debugLineLength)) - (leftHand.location +
                    (leftHand.orientation * debugLineLength));
                // If the dot is close to -1 then the hands are facing each other
                combine = Vector3.Dot(leftHand.orientation.normalized, rightHand.orientation.normalized) <= -0.75;// && pointDist.magnitude < handDist.magnitude;
                testDot = Vector3.Dot(leftHand.orientation.normalized, rightHand.orientation.normalized);
                //Debug.DrawLine(rightHand.location, rightHand.location + (rightHand.orientation * debugLineLength), Color.black); 
                //Debug.DrawLine(leftHand.location, leftHand.location + (leftHand.orientation * debugLineLength), Color.black); 
                // Add a scalar*direction of the hands to the position of the hand to each one
                // If the distance between these points is less than the distance between the hands
                // then the hands are facing each other .
                // Else the hands are in an incorrect orientation.

                //Debug.LogWarning(leftHand.location.ToString() + " : " + debugLineLength);
                //Debug.LogWarning(rightHand.location);
                combineTest.SetActive(combine);
                if (combine)
                {
                    // If we want to combine, then we can calculate the power using the distance between the hands
                    handDist = leftHand.location - rightHand.location;
                    currOutput = 1 / handDist.magnitude;

                    laserDir[0] = ((leftHand.location - rig.position) + (rightHand.location - rig.position)) / 2;
                    laserDir[0].y *= -1;
                    // Debug
                    laserPos[0] = (leftHand.location + rightHand.location) / 2;
                    combineTest.transform.position = laserPos[0];
                    //Debug.DrawLine(laserPos[0],laserPos[0] + laserDir[0] * 500);
                    //Debug.Log($"The laser starts at {laserPos}");

                }
                else
                {
                    for (int i = 0; i < laserDir.Length; i++)
                    {
                        currOutput = maxBurnout / 2;

                        laserDir[i] = i == 0 ? leftHand.orientation : rightHand.orientation;
                        laserPos[i] = i == 0 ? leftHand.location : rightHand.location;
                    }
                }
            }
            else if (hands == Hand.Left || hands == Hand.Right)
            {

                handDist = hands == Hand.Left ? leftHand.location : rightHand.location;
                currOutput = maxBurnout / 2;

                laserDir[0] = hands == Hand.Left ? leftHand.orientation : rightHand.orientation;

                laserPos[0] = hands == Hand.Left ? leftHand.location : rightHand.location;
            }

            // If hits a sample, apply damage
            RaycastHit hit;
            for (int i = 0; i < laserDir.Length; i++)
            {
                Ray ray = new Ray(laserPos[i], laserDir[i]);
                Debug.DrawLine(laserPos[i], laserDir[i] * 500f);


                if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
                {
                    hitTests[i].transform.position = hit.point;

                    //Debug.Log($"Object Hit: {hit.collider.gameObject.name}");

                    if (activeSample == null || activeSample.gameObject.GetInstanceID() != hit.collider.gameObject.GetInstanceID())
                    {
                        activeSample = hit.collider.gameObject.GetComponent<Sample>();
                    }

                    if (!_isInCooldown)
                    {
                        Debug.LogWarning("Hit Sample");
                        _isInCooldown = true;
                        if (!activeSample.PlayerDamage(currOutput))
                        {
                            activeSample = null;
                        }
                        StartCoroutine(ResetCooldown());
                    }

                }
                else
                {
                    activeSample = null;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {

        //(rightHand.location + (rightHand.orientation * 0.25f) -
        //   leftHand.location + (leftHand.orientation * 0.25f)).magnitude < handDist.magnitude;
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(_cooldown);
        _isInCooldown = false;
    }

    void UpdateBeamInfo(bool isLeft, Vector3 handPosition , Vector3 handOrientation)
    {
        if (hands != Hand.None)
        {
            ;
            if (isLeft)
            {
                leftHand.location = handPosition;
                leftHand.orientation = handOrientation;

            }
            else
            {
                rightHand.location = handPosition;
                rightHand.orientation = handOrientation;
            }
            Color colour = hands == (isLeft ? Hand.Left : Hand.Right) || hands == Hand.Both ? Color.green : Color.red;
            Debug.DrawLine(handPosition, handPosition + (handOrientation * 500), colour);


        }
        else
        {
            active = false;
        }

        UpdateBeam();
    }


    public void ToggleBeam(Hand hand, bool newActive)
    {
        Debug.LogError("Toggle Beam: " + hand.ToString());
        hands = hand;
        active = newActive;
    }

}

