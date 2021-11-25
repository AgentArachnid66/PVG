using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class HeightMapGenerator : MonoBehaviour
{
    private Texture2D texture;
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float offset_X;
    public float offset_Y;
    public float lowerBound;
    public float higherBound;
    public float offsetMidpoint;


    private Renderer _renderer;
    private void Awake()
    {
        texture = new Texture2D(width, height, TextureFormat.RGB24, true);
        texture.name = "Procedural Texture";
        GetComponent<MeshRenderer>().material.mainTexture = texture;
    }

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();

    }

    [ContextMenu("Update Texture")]
    void UpdateTexture()
    {

        _renderer.material.mainTexture = GenerateTexture();
        Debug.Log("Update Texture");
    }

    [ContextMenu("Randomise Texture")]
    void RandomiseValues()
    {
        scale = UnityEngine.Random.Range(5, 10);
        offset_X = UnityEngine.Random.Range(-999f, 999f);
        offset_Y = UnityEngine.Random.Range(-999f, 999f);

    }

    [ContextMenu("Update All Render Textures")]
    void UpdateRenderTextures()
    {
        RandomiseValues();
        UpdateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Color colour = CalculateColour(i, j);
                texture.SetPixel(i, j, colour);
            }

        }
        texture.Apply();
        return texture;

    }

    Color CalculateColour(int x, int y)
    {
        float xCoord = (float)x / width * scale + offset_X;
        float yCoord = (float)y / height * scale + offset_X;

        float sample = Step(Mathf.PerlinNoise(xCoord, yCoord));


        return new Color(sample, 0f, 0f);
    }

    float Step(float sample)
    {
        /*
         * If value is within bounds, return midpoint of the bands
         * Else return value
         */

        float returnVal = sample >= lowerBound && sample <= higherBound ? ((lowerBound + higherBound) / 2) + offsetMidpoint : sample;
        return returnVal;
    }
    string SaveRenderTexture(RenderTexture renderTexture, string fileName)
    {
        // Stores the active Render Texture
        RenderTexture oldRT = RenderTexture.active;

        // Sets up a Texture2D object the same dimensions as the input render texture
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height);
        RenderTexture.active = renderTexture;

        // Reads the pixels of the render texture into the local Texture2D variable
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

        // Applies those pixels to the texture
        tex.Apply();

        string dir = Application.dataPath + "/../SavedImages/";

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        fileName = dir + fileName + System.DateTime.Now.ToShortTimeString() + ".png";


        // Encodes the PNG file
        File.WriteAllBytes(fileName, tex.EncodeToPNG());

        RenderTexture.active = oldRT;

        return fileName;
    }

}