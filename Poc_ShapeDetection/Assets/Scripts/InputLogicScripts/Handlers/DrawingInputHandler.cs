using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DrawingInputHandler : MonoBehaviour
{
    public event Action<Vector2> OnStartDrawing;
    public event Action<Vector2> OnUpdateDrawing;
    public event Action OnEndDrawing;

    private InputAction drawAction;
    private InputAction pointerPositionAction;

    void Awake()
    {
        drawAction = new InputAction("Draw", binding: "<Pointer>/press");
        drawAction.performed += ctx => OnStartDrawing?.Invoke(GetWorldPosition());
        drawAction.canceled += ctx => OnEndDrawing?.Invoke();

        pointerPositionAction = new InputAction("PointerPosition", binding: "<Pointer>/position");
    }

    void OnEnable()
    {
        drawAction.Enable();
        pointerPositionAction.Enable();
    }

    void OnDisable()
    {
        drawAction.Disable();
        pointerPositionAction.Disable();
    }

    void Update()
    {
        // Mientras se esté presionando, se notifica la actualización del dibujo.
        if (Mouse.current.press.isPressed || Touchscreen.current?.primaryTouch.press.isPressed == true)
        {
            OnUpdateDrawing?.Invoke(GetWorldPosition());
        }
    }

    Vector2 GetWorldPosition()
    {
        Vector2 screenPos = pointerPositionAction.ReadValue<Vector2>();
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
