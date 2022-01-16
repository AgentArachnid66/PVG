using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class DroneHands : MonoBehaviour
{

    public GameObject leftHand;
    public GameObject rightHand;
    public float distance;
    public Vector3 offset;
    private Transform _drone;

    


    // Start is called before the first frame update
    void Start()
    {
        CustomEvents.CustomEventsInstance.UpdateHandPosition.AddListener(MapHands);
        _drone = GameObject.Find("Drone").transform;

    }


    // Maps the hand data from teh Leap Motion to GameObjects
    void MapHands(bool isLeft, Leap.Vector localLocation, Leap.Vector orientation)
    {
        // Local variable to hold the correct location
       Vector3 pos = MapHand(localLocation);
       
       // Selects the correct hand to affect
        if (isLeft)
        {
            leftHand.transform.position = MapHand(localLocation);
            leftHand.transform.rotation = Quaternion.LookRotation(CustomUtility.LeapMapOrientation(orientation));
        }
        else
        {
            rightHand.transform.position = MapHand(localLocation);
            rightHand.transform.rotation = Quaternion.LookRotation(CustomUtility.LeapMapOrientation(orientation));
        }
        
        // Passes this data onwards to the relevant classes
        CustomEvents.CustomEventsInstance.UpdateLaser.Invoke(isLeft, pos, CustomUtility.LeapMapOrientation(orientation));
    }

    Vector3 MapHand(Vector location)
    {
        /* Steps taken in this function:
            * Convert the Leap Data from their coordinate system to Unity's
            * Multiply the data by the distance to put the hands a set distance away from the player
            * Add an offset to control where they appear relative to the drone
            * Rotate this by the rotation of the drone so they appear in front of the drone at all times
            * Finally translate by the position of the drone to convert from local to world space
         */
        Vector3 newPos = (_drone.rotation * ((CustomUtility.LeapMapHands(location) * distance) + offset)) + transform.position;
        return newPos;
    }

}
