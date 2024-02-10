using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.iOS;
using UnityEngine;

public class ReachText : MonoBehaviour
{
    private float reach;
    private float previousReach;
    private TextMeshProUGUI reachText;
    public ResourceManagerScriptableObject resourceManager;

    void Start()
    {
        reachText = GetComponent<TextMeshProUGUI>();
        reach = resourceManager.reach;
        reachText.text = reach.ToString();
    }

    private void OnEnable()
    {
        ResourceManagerScriptableObject.onIncrementReach += DisplayNewReach;
    }

    private void OnDisable()
    {
        ResourceManagerScriptableObject.onIncrementReach -= DisplayNewReach;
    }

    private void DisplayNewReach(float value)
    {
        previousReach = reach;
        reach = value;
        reachText.text = value.ToString();
    }
}
