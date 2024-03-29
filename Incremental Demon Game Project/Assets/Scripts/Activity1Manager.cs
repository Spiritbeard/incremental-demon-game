using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Activity1Manager : MonoBehaviour
{
    [SerializeField]
    private int litCandles;
    private int action1DarknessGain;
    public static event Action<float> OnAllCandlesLit;

    private void Awake()
    {
        litCandles = 0;
        action1DarknessGain = 1;
    }

    private void OnEnable()
    {
        CandleFlame.OnCandleLit += IncrementCandleCount;
    }

        private void OnDisable()
    {
        litCandles = 0;
        CandleFlame.OnCandleLit -= IncrementCandleCount;
    }

    private void IncrementCandleCount(Vector2 pos)
    {
        litCandles++;
        if (litCandles >=6)
        {
            OnAllCandlesLit?.Invoke(action1DarknessGain);
            litCandles = 0;
        }
    }

}
