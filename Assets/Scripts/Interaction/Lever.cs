using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Lever : MonoBehaviour
{
    // Vectors control where everything is in the game
    public Vector3 min;
    public Vector3 max;
    // To control where the origin of the lever is
    public Vector3 originOffset;
    // Where the end of the lever is (what the player interacts with)
    public Transform position;

    // These two floats contain the current angle of the lever and the current percentage
    public float currAngle;
    private float maxAngle;
    public float currPercent;
    // Get the angle between 2 limiting vectors


    // Event Per lever instead of in the custom events.
    // Makes more sense to have separate call backs for each lever
    public UnityEventFloat OnUpdateLever = new UnityEventFloat();

    void Start()
    {
        
        maxAngle = Vector3.Angle(min, max);
        currAngle = Vector3.Angle(min, position.position - (transform.position + originOffset));
        currPercent = currAngle / maxAngle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + originOffset, position.position);
        Gizmos.DrawLine(transform.position + originOffset, transform.position + originOffset + min);
        Gizmos.DrawLine(transform.position + originOffset, transform.position + originOffset + max);
    }

    void Update()
    {
        currAngle = Vector3.Angle(min, position.position - transform.position);
        currPercent = currAngle / maxAngle;
        OnUpdateLever.Invoke(currPercent);
    }
}
