using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.iOS;
using UnityEngine;

public class IntelText : MonoBehaviour
{
    private float intel;
    private float previousIntel;
    private TextMeshProUGUI intelText;
    public ResourceManagerScriptableObject resourceManager;

    void Start()
    {
        intelText = GetComponent<TextMeshProUGUI>();
        intel = resourceManager.intel;
        intelText.text = intel.ToString();
    }

    private void OnEnable()
    {
        ResourceManagerScriptableObject.onIncrementIntel += DisplayNewIntel;
    }

    private void OnDisable()
    {
        ResourceManagerScriptableObject.onIncrementIntel -= DisplayNewIntel;
    }

    private void DisplayNewIntel(float value)
    {
        previousIntel = intel;
        intel = value;
        intelText.text = value.ToString();
    }
}
