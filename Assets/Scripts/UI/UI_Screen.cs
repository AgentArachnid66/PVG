using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Screen : MonoBehaviour
{
    // This is responsible for displaying a camera view onto a mesh to act as a
    // camera feed. 

    public Camera source;


    private RenderTexture target;
    private Renderer renderer;

    private void Start()
    {
        target = new RenderTexture(1024, 1024, 16);
        if (target.IsCreated())
        {
            target.Create();
        }

        renderer = GetComponent<Renderer>();

        source.targetTexture = target;
        renderer.material.mainTexture = target;
    }


}
