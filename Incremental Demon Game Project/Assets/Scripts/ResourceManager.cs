using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private float darkness;
    [SerializeField]
    private float spoils;
    [SerializeField]
    private float intel;
    [SerializeField]
    private float reach;

    public static event Action<float> onIncrementDarkness;
    public static event Action<float> onIncrementSpoils;
    public static event Action<float> onIncrementIntel;
    public static event Action<float> onIncrementReach;

    public void OnEnable()
    {
        Action1Manager.OnAllCandlesLit += IncrementDarkness;
    }

    public void OnDisable()
    {
        Action1Manager.OnAllCandlesLit -= IncrementDarkness;
    }

    public void IncrementDarkness(float changeAmount)
    {
        darkness += changeAmount;
        onIncrementDarkness?.Invoke(darkness);
    }

    public void IncrementSpoils(float changeAmount)
    {
        spoils += changeAmount;
        onIncrementSpoils?.Invoke(spoils);
    }

    public void IncrementIntel(float changeAmount)
    {
        intel += changeAmount;
        onIncrementIntel?.Invoke(intel);
    }

    public void IncrementReach(float changeAmount)
    {
        reach += changeAmount;
        onIncrementReach?.Invoke(reach);
    }
}
