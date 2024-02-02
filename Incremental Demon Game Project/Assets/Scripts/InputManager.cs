
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{    
    private PlayerInput playerInput;

    private InputAction pressPositionAction;
    private InputAction primaryPressAction;

    private Vector2 tempPos;
    private Vector3 screenPos;
    public Vector3 pressPos;
    public Camera mainCamera;

    public static event Action<Vector2> OnUpdatePressPos;
    public static event Action<Vector2, float> OnStartPrimaryPress;
    public static event Action<Vector2, float> OnEndPrimaryPress;

    private bool isPressing;

    private void Awake()
    {
        mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        pressPositionAction = playerInput.actions.FindAction("PressPosition");
        primaryPressAction = playerInput.actions.FindAction("PrimaryPress");
        isPressing = false;
    }

    private void OnEnable()
    {
        primaryPressAction.started += ctx => StartPrimaryPress(ctx);
        primaryPressAction.canceled += ctx => EndPrimaryPress(ctx);
    }

    private void OnDisable()
    {
        primaryPressAction.started -= ctx => StartPrimaryPress(ctx);
        primaryPressAction.canceled -= ctx => EndPrimaryPress(ctx);
    }

    public void StartPrimaryPress(InputAction.CallbackContext context)
    {
        isPressing = true;
        tempPos = pressPositionAction.ReadValue<Vector2>();
        screenPos = new Vector3(tempPos.x, tempPos.y, mainCamera.nearClipPlane);
        pressPos = Utils.ScreenToWorld(mainCamera, screenPos);
        OnStartPrimaryPress?.Invoke(pressPos, (float)context.time);
        StartCoroutine(UpdatePressPoint(context));
    }

    public void EndPrimaryPress(InputAction.CallbackContext context)
    {
        isPressing = false;
        tempPos = pressPositionAction.ReadValue<Vector2>();
        screenPos = new Vector3(tempPos.x, tempPos.y, mainCamera.nearClipPlane);
        pressPos = Utils.ScreenToWorld(mainCamera, screenPos);
        OnEndPrimaryPress?.Invoke(pressPos, (float)context.time);
        StopCoroutine(UpdatePressPoint(context));
    }

    public IEnumerator UpdatePressPoint(InputAction.CallbackContext context)
    {
        while(isPressing == true)
        {
            tempPos = pressPositionAction.ReadValue<Vector2>();
            screenPos = new Vector3(tempPos.x, tempPos.y, mainCamera.nearClipPlane);
            pressPos = Utils.ScreenToWorld(mainCamera, screenPos);
            OnUpdatePressPos?.Invoke(pressPos);
            yield return null;
        }
    }
}
