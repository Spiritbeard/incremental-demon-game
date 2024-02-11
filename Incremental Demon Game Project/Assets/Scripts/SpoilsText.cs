using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.iOS;
using UnityEngine;

public class SpoilsText : MonoBehaviour
{
    private float spoils;
    private float previousSpoils;
    private TextMeshProUGUI spoilsText;
    public ResourceManagerScriptableObject resourceManager;

    void Start()
    {
        spoilsText = GetComponent<TextMeshProUGUI>();
        spoils = resourceManager.spoils;
        spoilsText.text = spoils.ToString();
    }

    private void OnEnable()
    {
        ResourceManagerScriptableObject.onIncrementSpoils += DisplayNewSpoils;
    }

    private void OnDisable()
    {
        ResourceManagerScriptableObject.onIncrementSpoils -= DisplayNewSpoils;
    }

    private void DisplayNewSpoils(float value)
    {
        previousSpoils = spoils;
        spoils = value;
        spoilsText.text = value.ToString();
    }
}
