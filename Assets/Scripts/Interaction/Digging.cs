using System;
using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using UnityEngine;
using VolumetricLines;

public class Digging : MonoBehaviour
{
    // How quickly a sample is taken
    [SerializeField] private float _cooldown = 0.25f;
    // If a sample can be taken
    [SerializeField] private bool _isInCooldown;
    // Which layer the laser should test for a sample
    [SerializeField] private LayerMask mask;
    /*
     * This class is in charge of the digging portion of the gameplay loop
     * 
     * If the player chooses to use both hands
     * 
        * The mining laser will be controlled by the distance between the two hands
     * 
     * If the player chooses to use only one hand
     * 
        * The mining laser outputs a constant amount that could be too low for some resources
     * 
     */

    // Objects to display this code working in the demo
    public VolumetricLineBehavior combineTest;
    public VolumetricLineBehavior[] hitTests = new VolumetricLineBehavior[2];
    

    // Whether or not this code is active
    public bool active;

    
    // The transform of the origin
    public Transform rig;
    
    // The threshold value that if the player exceeds, the laser requires a cooldown before it can be used again
    public float maxBurnout;

    // What the laser is currently outputting. To be read by the samples to see how it compares to it's threshold values
    public float currOutput;


    // The vector between the hands, to measure the distance between them and to get the orientation
    private Vector3 _handDist;

    
    // Direction and Position of the lasers
    private Vector3[] _laserDir = new Vector3[2];
    private Vector3[] _laserPos = new Vector3[2];

    // Enum that holds which hands are active in this mode
    public Hand hands;

    // Bool to determine if the player wants to use 2 different beams or combine them for the more powerful version
    private bool _combine;

    // Holds the data for each hand
    public HandData leftHand;
    public HandData rightHand;

    // Currently Active Sample
    private Sample activeSample;

   
    private void Start()
    {
        CustomEvents.CustomEventsInstance.UpdateLaser.AddListener(UpdateBeamInfo);
    }

    void UpdateBeam()
    {
        // If the hands are facing each other within a threshold, then the beams will combine
        // Otherwise they will act independently

        // First check if the laser is active
        if (active)
        {
            // Then check if the player has both hands as lasers
            if (hands == Hand.Both)
            {
                // Add a scalar*direction of the hands to the position of the hand to each one
                // If the distance between these points is less than the distance between the hands
                // then the hands are facing each other .
                // Else the hands are in an incorrect orientation.
                //Vector3 pointDist = (rightHand.location + (rightHand.orientation * debugLineLength)) - (leftHand.location +
                //    (leftHand.orientation * debugLineLength));
                
                
                // If the dot is close to -1 then the hands are facing each other
                _combine = Vector3.Dot(leftHand.orientation.normalized, rightHand.orientation.normalized) <= -0.75;// && pointDist.magnitude < handDist.magnitude;
                


               
                combineTest.gameObject.SetActive(_combine);
                if (_combine)
                {
                    // If we want to combine, then we can calculate the power using the distance between the hands
                    _handDist = leftHand.location - rightHand.location;
                    // The output of the combined laser is inversely proportional to the magnitude of the hand Dist
                    // vector so that the smaller the distance, the bigger the output
                    currOutput = 1 / _handDist.magnitude;

                    // The direction vector is the average direction vector from the rig to each hand
                    _laserDir[0] = ((leftHand.location - rig.position) + (rightHand.location - rig.position)) / 2;
                    // Directs the laser downwards as otherwise it wouldn't hit anything
                    // as the hands are always above the rig
                    _laserDir[0].y *= -1;
                    
                    // Starting point of the laser is the midpoint between the two 
                    // position vectors
                    _laserPos[0] = (leftHand.location + rightHand.location) / 2;
                    
                    // Finally Update the lasers VFX
                    combineTest.transform.position = _laserPos[0];
                    combineTest.SetStartAndEndPoints(Vector3.zero, _laserDir[0]*500f);

                    foreach (VolumetricLineBehavior hitTest in hitTests)
                    {
                        hitTest.gameObject.SetActive(false);
                    }

                }
                else
                {
                    // If we are not combining, then iterate over the hands and set the appropriate values
                    for (int i = 0; i < _laserDir.Length; i++)
                    {
                        currOutput = maxBurnout / 2;

                        _laserDir[i] = i == 0 ? leftHand.orientation : rightHand.orientation;
                        _laserPos[i] = i == 0 ? leftHand.location : rightHand.location;
                        
                        hitTests[i].transform.position = _laserPos[i];
                        hitTests[i].SetStartAndEndPoints(Vector3.zero, _laserDir[i]*500f);
                        hitTests[i].gameObject.SetActive(true);
                    }
                }
            }
            
            // If it's only individual hands that are active
            else if (hands == Hand.Left || hands == Hand.Right)
            {

                _handDist = hands == Hand.Left ? leftHand.location : rightHand.location;
                
                // In the case it's individual hands, the output is constant
                currOutput = maxBurnout / 2;

                _laserDir[0] = hands == Hand.Left ? leftHand.orientation : rightHand.orientation;

                _laserPos[0] = hands == Hand.Left ? leftHand.location : rightHand.location;

                hitTests[0].transform.position = _laserPos[0];
                hitTests[0].SetStartAndEndPoints(Vector3.zero, _laserDir[0]*500f);
            }

            // If hits a sample, apply damage
            RaycastHit hit;
            
            if (hands != Hand.Both)
            {
                _laserDir[1] = Vector3.zero;
                _laserPos[1] = Vector3.zero;
                
            }
            
            // Iterates over the number of lasers currently active
            for (int i = 0; i < _laserDir.Length; i++)
            {


                Ray ray = new Ray(_laserPos[i], _laserDir[i]);
                Debug.DrawLine(_laserPos[i], _laserDir[i] * 500f);
                
                // Casts a ray using the laser information. The resources are on a separate 
                // physics layer so that this will either hit a resource or nothing
                if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
                {
                    hitTests[i].SetStartAndEndPoints(Vector3.zero, hit.point);

                    // If there are no active samples or this is hitting a different sample then the 
                    // currently active one then it gets the relevant component. This prevents 
                    // it from calling GetComponent except when it needs to.
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

        }
        else
        {
            active = false;
        }

        UpdateBeam();
    }


    public void ToggleBeam(Hand hand, bool newActive)
    {
        Debug.LogError("Toggle Beam: " + hand.ToString() +(newActive?"On": "Off"));
        hands = hand;
        active = newActive;
        combineTest.gameObject.SetActive(newActive);
        for (int i = 0; i < hitTests.Length; i++)
        {
            hitTests[i].gameObject.SetActive(newActive);
        }

    }

}

