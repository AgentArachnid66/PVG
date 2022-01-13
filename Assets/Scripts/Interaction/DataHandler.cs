using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using LSL;

public class DataHandler : MonoBehaviour
{
    private Controller controller = new Controller();

    public Leap.Frame frame;
    public Transform origin;
    public float radius = 5;
    
    
    
    public CustomEvents customEvents;
   //public 
    public void Update()
    {
        //Debug.Log("Controller is " + (controller.IsConnected ? "Connected" : "not Connected"));
        
        frame = controller.Frame();

        ProcessLeapData();
        //controller.FrameReady += ProcessLeapData;

    }
    
    /*
    void Awake()
    {
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 3, Time.fixedDeltaTime * 1000, channel_format_t.cf_float32);
        XMLElement channels = streamInfo.desc().append_child("channels");

        channels.append_child("channel").append_child_value("label", "X");
        channels.append_child("channel").append_child_value("label", "Y");
        channels.append_child("channel").append_child_value("label", "Z");

        outlet = new StreamOutlet(streamInfo);
    }

    void FixedUpdate()
    {
        float[] positionData = { headsetPosition.x, headsetPosition.y, headsetPosition.z };

        outlet.push_sample(positionData);
    }
    */

    void Awake()
    {
    }

    void FixedUpdate()
    {
    }

    
    void ProcessLeapData()
    {
        // So this function will take the Leap Frame data and convert it into an array of floats to be sent over
        // the LSL streams and to be used in my other scripts.

        Debug.Log("Processing Leap Data");

        foreach(Leap.Hand hand in frame.Hands){
            string id = hand.IsLeft ? "Left Hand Position: " : "Right Hand Position: ";

            customEvents.UpdateHandPosition.Invoke(hand.IsLeft, hand.PalmPosition, hand.PalmNormal);

            //customEvents.UpdateLaser.Invoke(hand.IsLeft, CustomUtility.LeapVectorToUnityVector3(hand.PalmNormal), (CustomUtility.LeapVectorToUnityVector3(hand.PalmPosition) + origin.position));


            //Debug.Log(origin.position + hand.PalmPosition.ToVector3());
            
        }

    }


}