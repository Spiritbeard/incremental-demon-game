
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{    
    private PlayerInput playerInput;

    private InputAction pressPostionAction;
    private InputAction primaryPressAction;

    public Vector3 screenPos;
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
        pressPostionAction = playerInput.actions.FindAction("PressPosition");
        primaryPressAction = playerInput.actions.FindAction("PrimaryPress");
        isPressing = false;
    }

    private void OnEnable()
    {
        primaryPressAction.started += ctx => StartPrimaryPress(ctx);
        primaryPressAction.performed += ctx => EndPrimaryPress(ctx);
    }

    private void OnDisable()
    {
        primaryPressAction.started -= ctx => StartPrimaryPress(ctx);
        primaryPressAction.performed -= ctx => EndPrimaryPress(ctx);
    }

    public void StartPrimaryPress(InputAction.CallbackContext context)
    {
        OnStartPrimaryPress?.Invoke(Utils.ScreenToWorld(mainCamera, pressPostionAction.ReadValue<Vector2>()), (float)context.time);
        isPressing = true;
        StartCoroutine(UpdatePressPoint(context));
    }

    public void EndPrimaryPress(InputAction.CallbackContext context)
    {
        OnEndPrimaryPress?.Invoke(Utils.ScreenToWorld(mainCamera, pressPostionAction.ReadValue<Vector2>()), (float)context.time);
        isPressing = false;
        StopCoroutine(UpdatePressPoint(context));
    }

    public IEnumerator UpdatePressPoint(InputAction.CallbackContext context)
    {
        while(isPressing == true)
        {
            screenPos = pressPostionAction.ReadValue<Vector2>();
            screenPos.z = 0;
            pressPos = Utils.ScreenToWorld(mainCamera, pressPostionAction.ReadValue<Vector2>());
            OnUpdatePressPos?.Invoke(pressPos);
            yield return null;
        }
    }
}
