using System;
using UnityEngine;

[DefaultExecutionOrder(1)]
[CreateAssetMenu(fileName = "ResourceManager", menuName = "ScriptableObjects/ResourceManager")]
public class ResourceManagerScriptableObject : ScriptableObject
{
    public float darkness;
    public float spoils;
    public float intel;
    public float reach;

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
