using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digging : MonoBehaviour
{

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

    public Transform leftHand;
    public Transform rightHand;
    public Transform rig;
    public CustomEvents customEvents;
    // The threshold value that if the player exceeds, the laser requires a cooldown before it can be used again
    public float maxBurnout;

    // What the laser is currently outputting. To be read by the samples to see how it compares to it's threshold values
    public float currOutput;

    // The vector between the hands, to measure the distance between them and to get the orientation
    private Vector3 handDist;


    // Enum that holds which hands are active in this mode
    private Hand hands;

    // Bool to determine if the player wants to use 2 different beams or combine them for the more powerful version
    private bool combine;

    private void Start()
    {
        
    }

    void UpdateBeam()
    {
        // If the hands are facing each other within a threshold, then the beams will combine
        // Otherwise they will act independently

        // Project the beam with falloff

        // If hits a sample, apply damage
    }
 

}
