using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DrawingInputHandler : MonoBehaviour
{
    public event Action<Vector2> OnStartDrawing;
    public event Action<Vector2> OnUpdateDrawing;
    public event Action OnEndDrawing;

    [SerializeField] private InputActionAsset inputActions;

    private InputAction drawAction;
    private InputAction pointerPositionAction;

    private bool isDrawing = false; // Indica si se está dibujando

    void Awake()
    {
        // Buscar el action map llamado "Drawing" en el asset
        var drawingMap = inputActions.FindActionMap("Drawing");
        if (drawingMap == null)
        {
            Debug.LogError("No se encontró el Action Map 'Drawing' en el InputActionAsset.");
            return;
        }

        drawAction = drawingMap.FindAction("Draw");
        pointerPositionAction = drawingMap.FindAction("PointerPosition");

        if (drawAction == null)
            Debug.LogError("No se encontró la acción 'Draw' en el Action Map 'Drawing'.");
        if (pointerPositionAction == null)
            Debug.LogError("No se encontró la acción 'PointerPosition' en el Action Map 'Drawing'.");

        // Cuando se inicia el dibujo, marcamos isDrawing en true y enviamos la posición inicial.
        drawAction.started += ctx => {
            isDrawing = true;
            OnStartDrawing?.Invoke(GetWorldPosition());
        };

        // Cuando se cancela (se levanta el dedo o se suelta el botón), marcamos isDrawing en false.
        drawAction.canceled += ctx => {
            isDrawing = false;
            OnEndDrawing?.Invoke();
        };
    }

    void OnEnable()
    {
        drawAction?.Enable();
        pointerPositionAction?.Enable();
    }

    void OnDisable()
    {
        drawAction?.Disable();
        pointerPositionAction?.Disable();
    }

    void Update()
    {
        // Mientras estemos en modo dibujo (manteniendo pulsado) se actualiza continuamente la posición.
        if (isDrawing)
        {
            OnUpdateDrawing?.Invoke(GetWorldPosition());
        }
    }

    private Vector2 GetWorldPosition()
    {
        Vector2 screenPos = pointerPositionAction.ReadValue<Vector2>();
        if (Camera.main == null)
        {
            Debug.LogError("No se encontró la Main Camera. Asegúrate de tener una cámara con la etiqueta 'MainCamera'.");
            return Vector2.zero;
        }
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
}
