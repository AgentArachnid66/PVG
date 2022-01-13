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


    // Start is called before the first frame update
    void Start()
    {
        customEvents.UpdateHandPosition.AddListener(MapHands);
    }

    private void Update()
    {
    }

    void MapHands(bool isLeft, Leap.Vector localLocation, Leap.Vector orientation)
    {
        Vector3 pos = MapHand(localLocation, orientation);

        Debug.Log("Mapping Hands");

        if (isLeft)
        {
            leftHand.transform.position = MapHand(localLocation, orientation);
        }
        else
        {
            rightHand.transform.position = MapHand(localLocation, orientation);
        }
        customEvents.UpdateLaser.Invoke(isLeft, pos, CustomUtility.LeapVectorToUnityVector3(orientation));
    }

    Vector3 MapHand(Vector location, Vector orientation)
    {
        Vector3 newPos = CustomUtility.LeapVectorToUnityVector3(location * distance);
        return (newPos + transform.position);
    }

}
