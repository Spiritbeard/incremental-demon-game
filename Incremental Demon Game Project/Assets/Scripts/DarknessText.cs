using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.iOS;
using UnityEngine;

public class DarknessText : MonoBehaviour
{
    private float darkness;
    private float previousDarkness;
    private TextMeshProUGUI darknessText;
    public ResourceManagerScriptableObject resourceManager;

    void Start()
    {
        darknessText = GetComponent<TextMeshProUGUI>();
        darkness = resourceManager.darkness;
        darknessText.text = darkness.ToString();
    }

    private void OnEnable()
    {
        ResourceManagerScriptableObject.onIncrementDarkness += DisplayNewDarkness;
    }

    private void OnDisable()
    {
        ResourceManagerScriptableObject.onIncrementDarkness -= DisplayNewDarkness;
    }

    private void DisplayNewDarkness(float value)
    {
        previousDarkness = darkness;
        darkness = value;
        darknessText.text = value.ToString();
    }
}
