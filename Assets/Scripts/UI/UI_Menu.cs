using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    // The base class for the hand anchored UI

    // Transform to check against, typically the camera but I want the flexibility to include other objects
    public Transform view;

    // The minimum value needed to open or close the menu
    public float threshold;

    // The current dot product
    private float _currDot = 0f;

    // Whether or not that this code should open when looking at the view object
    public bool canOpen;

    // Which hand this menu object is attached to
    public Hand activeHand;

    void Update()
    {
        CheckView();
    }


    void CheckView()
    {
        // If the dot product of the menu's forward vector and the player's forward vector is above threshold then open the menu.
        // This means that the player is likely looking at the menu
        _currDot = Vector3.Dot(view.transform.forward, transform.forward);
        transform.gameObject.SetActive(_currDot >= threshold);

    }
    
}
