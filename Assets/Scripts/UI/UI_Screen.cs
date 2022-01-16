using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Screen : MonoBehaviour
{
    // This is responsible for displaying a camera view onto a mesh to act as a
    // camera feed. 

    public Camera source;


    private RenderTexture _target;
    private Renderer _renderer;

    private void Start()
    {
        // Creates the render texture
        _target = new RenderTexture(1024, 1024, 16);
        if (_target.IsCreated())
        {
            _target.Create();
        }
        
        //  Gets the renderer component
        _renderer = GetComponent<Renderer>();

        // Sets the target texture of the source camera to the newly created render texture
        source.targetTexture = _target;
        // Then sets the main texture of the screen object to the render texture
        _renderer.material.mainTexture = _target;
    }

}
