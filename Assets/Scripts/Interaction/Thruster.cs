using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Thruster : MonoBehaviour
{
 
    // Max Speed of the thrusters
    public float maxPower;
    // How quickly the drone rotates
    public float rotationMultiplier;
    // Rigidbody of the drone
    private Rigidbody m_Rigidbody;
    // If the thrusters are active
    private bool _activate;

    // For Testing Purposes to ensure that this is working
    public bool overrideActive;

    // Which Thrusters are active
    private Hand hands;

    // The UI related to the thrusters
    public Leap.Unity.Interaction.InteractionSlider leftControl;
    public Leap.Unity.Interaction.InteractionSlider rightControl;
    [SerializeField]
    private TextMesh _leftThrusterUI;
    [SerializeField]
    private TextMesh _rightThrusterUI;

    // Only dealing with rotation around the Y axis in the
    // demo to keep things simple which is held in this float
    private float _currRotation;
    // The movement multiplier applied to the forward vector to generate the movement target
    private float _currSum;
    // The target rotation
    private Vector3 _rot;

    // Two vectors that control 
    private Vector3 _clockwiseVector = Vector3.zero;
    private Vector3 _anticlockwiseVector = Vector3.zero;
   
    // Current dot product
    private float _dot;

    // Variables required to keep the drone a constant distance from the ground
    // and to teleport it back to where it came from if it goes beyond a certain distance
    // Terrain of the scene
    public Terrain terrain;
    // The original world position of the drone
    private Vector3 _originalPos;
    // The max distance away from _originalPos that the drone can go
    private float _distanceThreshold = 250f;
    // How far off the ground the drone hovers
    private float _floatingOffset = 5f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        _originalPos = transform.position;
        
        // When the slider has a horizontal event, update the UI
        leftControl._horizontalSlideEvent.AddListener((result =>
                {
                    _leftThrusterUI.text = Mathf.RoundToInt(result * 100f).ToString() + "%";
                }
                ));
        
        rightControl._horizontalSlideEvent.AddListener((result =>
                {
                    _rightThrusterUI.text = Mathf.RoundToInt(result * 100f).ToString() + "%";
                }
            ));
    }

    public void UpdateVector(bool isClockwise)
    {
        // Updates the Thruster values
        UpdateThrusterValues(isClockwise ? leftControl.HorizontalSliderValue : rightControl.HorizontalSliderValue,
            isClockwise);
    }

    void Update()
    {
        bool withinBounds = Vector3.Distance(transform.position, _originalPos) < _distanceThreshold;
        // Ideally would like to move away from doing this every frame
        if (_activate && withinBounds)
        {
            UpdateThrusters();
        }
        // If it is too far away from the original position it gets reset so that the drone doesn't
        // go out of bounds
        else if (!withinBounds)
        {
            transform.position = _originalPos;
        }
        
    }



    public void ToggleThrusters(bool activateThrusters)
    {
        // Toggles the Thruster On/Off
        
        Debug.Log("Thrusters are now " +(activateThrusters? "Active":"Inactive"));
        _activate = activateThrusters;
        
        // Stretch Goal 
        // Get the script to be able to handle turning on and off independent thrusters
    }

    // Retrieves the relevant values in use for the thrusters
    void UpdateThrusterValues(float rotationPower, bool clockwise)
    {
        

        if (_activate)
        {
            // So it'll add a certain rotation to the vehicle, but based on the percentage
            // it'll also move it forward at a certain rate.

            // Rotation = Sum of clockwise and anticlockwise
            // Forward Velocity = Sum of Rotation Power (clamped between 0 and max power)

            // This method allows for better maneuverability, though it does take a bit of getting used to

            // Rotate the forward vector by the rotation derived from the slider
            if (clockwise)
            {
                _clockwiseVector = Quaternion.Euler(0,Mathf.Lerp(-90, 90, rotationPower) , 0) * m_Rigidbody.transform.forward * rotationPower;
                _leftThrusterUI.text = Mathf.RoundToInt(leftControl.HorizontalSliderValue * 100f).ToString() + "%";
            }
            else
            {
                _anticlockwiseVector = Quaternion.Euler(0, Mathf.Lerp(-90, 90, 1-rotationPower), 0) * m_Rigidbody.transform.forward * rotationPower;
                _rightThrusterUI.text = Mathf.RoundToInt(rightControl.HorizontalSliderValue * 100f).ToString() + "%";
            }




        }
    }

    void UpdateThrusters()
    {
        if (_clockwiseVector != Vector3.zero || _anticlockwiseVector != Vector3.zero)
        {
            // Finds the Dot Product between the sum of the rotation vectors and the transform's forward vector
            _dot = Vector3.Dot((_clockwiseVector + _anticlockwiseVector).normalized, m_Rigidbody.transform.forward);

            // Since the Dot Product would be 1 when they are parallel (ie player doesn't want to rotate)
            // and 0 when they are at 90 degrees (ie player wants to do a sharper turn)
            // take 1 minus the dot to get the intended results then multiply by a float to easily control the output
            
            // Also to determine the direction of rotation, compare the magnitudes and see which one is greater and 
            // multiply by 1 or -1 to get the desired direction of rotation
            _currRotation = (1 - _dot) * rotationMultiplier * (_clockwiseVector.magnitude > _anticlockwiseVector.magnitude ? 1f : -1f);
            
            // Convert a Quaternion to Euler to apply the correct delta angle then convert it back
            _rot = m_Rigidbody.rotation.eulerAngles;
            _rot.y += _currRotation;
            m_Rigidbody.MoveRotation(Quaternion.Euler(_rot));

            // The average of the magnitudes * the max power to find the power applied to the drone 
            _currSum = Mathf.Clamp(((_clockwiseVector.magnitude + _anticlockwiseVector.magnitude)/2) * maxPower, 0, maxPower);
            // If this sum is less than 0.05 then treat it as if there is no input as the player is likely trying
            // to stop the drone
            _currSum = _currSum <= 0.05 * maxPower ? 0f : _currSum;

            // Target Position
            Vector3 targetPos = m_Rigidbody.position + (m_Rigidbody.transform.forward * _currSum);
            // Calculate the height from the Terrain and the offset
            float height = terrain.SampleHeight(targetPos);
            targetPos.y = height + _floatingOffset;
            Debug.Log($"Target Position {targetPos}");
            
            // Apply the movement by multiplying the current forward vector by the sum generated and 
            // offsetting it by the current position in world space to get the position in world space
            m_Rigidbody.MovePosition(targetPos);


        }
    }

}
