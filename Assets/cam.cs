using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    [SerializeField] private Vector2 targetResolution;
    [SerializeField] private int ppu;
    private Camera camera;
    

    void Awake()
    {

        camera = GetComponent<Camera>();

        float targetAspect = targetResolution.x / targetResolution.y;
        float currentAspect = Screen.width / (float)Screen.height;

        if (currentAspect < targetAspect)
        {            

            float scalingWidth = Screen.width / targetResolution.x;           

            float camSize = ((Screen.height / 2) / scalingWidth) / ppu;
            camera.orthographicSize = camSize;
        }
    }
    
}
