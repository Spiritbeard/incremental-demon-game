using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeTickManager : MonoBehaviour
{
    private const float TickTimerMax = 0.2f;
    

    private int tick;
    private float tickTimer;

    //smallest tick interval is 5 times per second
    public static event Action OnTick;

    //this action is called 1 time per second
    public static event Action OnTick_5;


    private void Awake()
    {
        tick = 0;
    }
   

    private void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= TickTimerMax)
        {
            tickTimer -= TickTimerMax;
            tick++;
            OnTick?.Invoke();
        }
        if (tick%5 == 0)
        {
            OnTick_5?.Invoke();
        }
    }
}
