using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;
    [SerializeField]
    private float minimumDistance = 0.1f;
    [SerializeField, Range(0f, 1f)]
    private float maximumTime = 4f;

    [SerializeField]
    private float directionThreshold = 0.9f;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        InputManager.OnStartPrimaryPress += SwipeStart;
        InputManager.OnEndPrimaryPress += SwipeEnd;
    }

    private void OnDisable()
    {
        InputManager.OnStartPrimaryPress -= SwipeStart;
        InputManager.OnEndPrimaryPress -= SwipeEnd;
    }

    private void SwipeStart(Vector2 pos, float time)
    {
        startPosition = pos;
        startTime = time;
    }

    private void SwipeEnd(Vector2 pos, float time)
    {
        endPosition = pos;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if ((Vector3.Distance(startPosition, endPosition) >= minimumDistance) && ((endTime - startTime) <= maximumTime))
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Swipe Up");
        }
        else if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Swipe Down");
        }
        else if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Swipe Left");
        }
        else if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Swipe Right");
        }

    }
}
