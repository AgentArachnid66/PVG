using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class DroneHands : MonoBehaviour
{

    public CustomEvents customEvents;
    public GameObject leftHand;
    public GameObject rightHand;
    public float distance;
    public Vector3 offset;
    public Transform _drone;

    public Vector3 testRight;
    public Vector3 testLeft;
    public Vector vectorLeft;
    public Vector vectorRight;
    
    


    // Start is called before the first frame update
    void Start()
    {
        customEvents.UpdateHandPosition.AddListener(MapHands);
        _drone = GameObject.Find("Drone").transform;

    }

    private void Update()
    {
        testRight = rightHand.transform.position;
        testLeft = leftHand.transform.position;
    }

    void MapHands(bool isLeft, Leap.Vector localLocation, Leap.Vector orientation)
    {
       Vector3 pos = MapHand(localLocation);

        //Debug.Log(orientation);

        if (isLeft)
        {
            vectorLeft = localLocation;
            leftHand.transform.position = MapHand(localLocation);
            leftHand.transform.rotation = Quaternion.LookRotation(CustomUtility.LeapMapOrientation(orientation));
        }
        else
        {
            vectorRight = localLocation;
            rightHand.transform.position = MapHand(localLocation);
            rightHand.transform.rotation = Quaternion.LookRotation(CustomUtility.LeapMapOrientation(orientation));
        }
        customEvents.UpdateLaser.Invoke(isLeft, pos, CustomUtility.LeapMapOrientation(orientation));
    }

    Vector3 MapHand(Vector location)
    {
        Vector3 newPos = (_drone.rotation * ((CustomUtility.LeapMapHands(location) * distance) + offset)) + transform.position;
        return newPos;
    }

}
