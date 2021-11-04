using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using LSL;

public class DataHandler : MonoBehaviour
{
    private Controller controller = new Controller();

    public Leap.Frame frame;

    void Update()
    {
        frame = controller.Frame();
    }

    [SerializeField] private Transform _orbitingTransform;

    private string StreamName = "FrameData";
    private string StreamType = "";

    private StreamOutlet outlet;


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
        Vector3 headsetPosition = _orbitingTransform.position;
        float[] positionData = { headsetPosition.x, headsetPosition.y, headsetPosition.z };

        outlet.push_sample(positionData);
    }
    */

    void Awake()
    {
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 10, Time.fixedDeltaTime * 1000, channel_format_t.cf_float32);
        XMLElement channels = streamInfo.desc().append_child("channels");

        channels.append_child("channel").append_child_value("label", "A");
        channels.append_child("channel").append_child_value("label", "B");
        channels.append_child("channel").append_child_value("label", "C");
        channels.append_child("channel").append_child_value("label", "D");
        channels.append_child("channel").append_child_value("label", "E");
        channels.append_child("channel").append_child_value("label", "F");
        channels.append_child("channel").append_child_value("label", "G");
        channels.append_child("channel").append_child_value("label", "H");
        channels.append_child("channel").append_child_value("label", "I");
        channels.append_child("channel").append_child_value("label", "J");

        outlet = new StreamOutlet(streamInfo);
    }

    void FixedUpdate()
    {
        Vector3 headsetPosition = _orbitingTransform.position;
        float[] positionData = { headsetPosition.x, headsetPosition.y, headsetPosition.z };

        outlet.push_sample(positionData);
    }
}
