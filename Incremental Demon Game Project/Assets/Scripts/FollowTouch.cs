using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowTouch : MonoBehaviour
{
    private Vector2 pressPos;
    private bool isPressing;

    private void OnEnable()
    {
        InputManager.OnStartPrimaryPress += StartFollowing;
        InputManager.OnUpdatePressPos += UpdatePressPos;
        InputManager.OnEndPrimaryPress += StopFollowing;
    }

    private void OnDisable()
    {
        InputManager.OnStartPrimaryPress -= StartFollowing;
        InputManager.OnUpdatePressPos -= UpdatePressPos;
        InputManager.OnEndPrimaryPress -= StopFollowing;

    }

    private void StartFollowing(Vector2 pos, float time)
    {
        pressPos = pos;
        transform.position = pressPos;
        isPressing = true;
        StartCoroutine(FollowPressPos());
    }

    private void UpdatePressPos(Vector2 pos)
    {
        pressPos = pos;
    }

    private void StopFollowing(Vector2 pos, float time)
    {
        isPressing = false;
        StopCoroutine(FollowPressPos());
    }

    private IEnumerator FollowPressPos()
    {
        while (isPressing)
        {
            transform.position = pressPos;
            yield return null;
        }
    }
}
