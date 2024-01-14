using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DarknessText : MonoBehaviour
{
    private float darkness;
    private float previousDarkness;
    private TextMeshProUGUI darknessText;

    void Start()
    {
        darknessText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        ResourceManager.onIncrementDarkness += DisplayNewDarkness;
    }

    private void OnDisable()
    {
        ResourceManager.onIncrementDarkness -= DisplayNewDarkness;
    }

    private void DisplayNewDarkness(float value)
    {
        previousDarkness = darkness;
        darkness = value;
        darknessText.text = value.ToString();
    }
}
