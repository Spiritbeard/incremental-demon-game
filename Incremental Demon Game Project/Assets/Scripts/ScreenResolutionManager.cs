using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionManager : MonoBehaviour
{
    public float latestWidth;
    public float latestHeight;

    public static event Action<float, float> OnResolutionChange;
    
    public void Awake()
    {
        latestWidth = Screen.width;
        latestHeight = Screen.height;
        OnResolutionChange?.Invoke(latestWidth, latestHeight);
    }
    
    public void OnEnable()
    {
        TimeTickManager.OnTick += CheckResolution;
    }

    public void OnDisable()
    {
        TimeTickManager.OnTick -= CheckResolution;
    }

    private void CheckResolution()
    {
        if (Screen.width != latestWidth || Screen.height != latestHeight)
        {
            latestWidth = Screen.width;
            latestHeight = Screen.height;
            OnResolutionChange?.Invoke(latestWidth, latestHeight);
            Debug.Log("Resolution changed to " + latestWidth + "x" + latestHeight);
        }
    }
}
