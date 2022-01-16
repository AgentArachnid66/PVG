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
    
    
    
    public CustomEvents customEvents;
   //public 
    public void Update()
    {
        
        frame = controller.Frame();

        ProcessLeapData();
        
    }
    
    /*
     
    // I wanted to make this function have the ability to send out data along a LSL 
    // Network to a python script that could use AI to identify a gesture
    // then feedback it's decision back to the game
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


    
    // An optimisation for this function would be to move it away from 
    // Update and only call it when the frame data is ready but this 
    // difference would not be noticeable
    void ProcessLeapData()
    {
        foreach(Leap.Hand hand in frame.Hands){
            string id = hand.IsLeft ? "Left Hand Position: " : "Right Hand Position: ";

            customEvents.UpdateHandPosition.Invoke(hand.IsLeft, hand.PalmPosition, hand.PalmNormal);

        }

    }


}