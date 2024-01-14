
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions playerInput;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        playerInput.Enable();

    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        playerInput.NavigateDefaultUI.ButtonPress.started += ctx => StartPress(ctx);
        playerInput.NavigateDefaultUI.ButtonPress.canceled += ctx => EndPress(ctx);
    }

    private void StartPress(InputAction.CallbackContext context)
    {
        Debug.Log("Press started");
    }

    private void EndPress(InputAction.CallbackContext context)
    {
        Debug.Log("Press ended");
    }
}
