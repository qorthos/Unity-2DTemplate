using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizing : MonoBehaviour
{
    public int SmallResScale;
    public int LargeResScale;
    public float PixelsPerUnit = 12;

    [ReadOnly] public int Scale;
    [ReadOnly] public float WorldPixelUnit;

    

    // Start is called before the first frame update
    void Awake()
    {
        Application.targetFrameRate = 60;

        if (Screen.width < 1580)
        {
            Scale = SmallResScale;
        }
        else if (Screen.width < 3000)
        {
            Scale = LargeResScale;
        }
        else
        {
            Scale = LargeResScale * 2;
        }

        WorldPixelUnit = 1f / 16f / Scale;
    }

    private void Start()
    {
        var mainCamera = Camera.main;
        mainCamera.orthographicSize = (mainCamera.pixelHeight / 2f / PixelsPerUnit / Scale);

        var camera = GetComponent<Camera>();
        if (camera == null)
            return;
        camera.orthographicSize = (camera.pixelHeight / 2f / PixelsPerUnit / Scale);

    }
}
