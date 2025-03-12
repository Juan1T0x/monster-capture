using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    // Primary finger events
    public event Action<Vector2> OnStartDrawing;
    public event Action<Vector2> OnUpdateDrawing;
    public event Action OnEndDrawing;

    // Secondary finger events
    public event Action<Vector2> OnStartDrawingSecondary;
    public event Action<Vector2> OnUpdateDrawingSecondary;
    public event Action OnEndDrawingSecondary;

    [SerializeField] private InputActionAsset inputActions;

    // Primary finger actions
    private InputAction firstFingerContactAction;
    private InputAction firstFingerPositionAction;
    private bool isDrawingPrimary = false;

    // Secondary finger actions
    private InputAction secondaryFingerContactAction;
    private InputAction secondaryFingerPositionAction;
    private bool isDrawingSecondary = false;

    private const string MAP_NAME = "Touch";
    private const string FIRST_FINGER_CONTACT_ACTION = "FirstFingerContact";
    private const string FIRST_FINGER_POSITION_ACTION = "FirstFingerPosition";
    private const string SECONDARY_FINGER_CONTACT_ACTION = "SecondaryFingerContact";
    private const string SECONDARY_FINGER_POSITION_ACTION = "SecondaryFingerPosition";

    private void Awake()
    {
        var drawingMap = inputActions.FindActionMap(MAP_NAME);

        // Primary finger setup
        firstFingerContactAction = drawingMap.FindAction(FIRST_FINGER_CONTACT_ACTION);
        firstFingerPositionAction = drawingMap.FindAction(FIRST_FINGER_POSITION_ACTION);

        firstFingerContactAction.started += ctx =>
        {
            isDrawingPrimary = true;
            OnStartDrawing?.Invoke(GetWorldPositionCoordinates(firstFingerPositionAction));
            Debug.Log("First finger touched screen on position: " + GetWorldPositionCoordinates(firstFingerPositionAction));
        };
        firstFingerContactAction.canceled += ctx =>
        {
            isDrawingPrimary = false;
            OnEndDrawing?.Invoke();
            Debug.Log("First finger released screen");
        };

        // Secondary finger setup
        secondaryFingerContactAction = drawingMap.FindAction(SECONDARY_FINGER_CONTACT_ACTION);
        secondaryFingerPositionAction = drawingMap.FindAction(SECONDARY_FINGER_POSITION_ACTION);

        secondaryFingerContactAction.started += ctx =>
        {
            isDrawingSecondary = true;
            OnStartDrawingSecondary?.Invoke(GetWorldPositionCoordinates(secondaryFingerPositionAction));
            Debug.Log("Second finger touched screen on position: " + GetWorldPositionCoordinates(secondaryFingerPositionAction));
        };
        secondaryFingerContactAction.canceled += ctx =>
        {
            isDrawingSecondary = false;
            OnEndDrawingSecondary?.Invoke();
            Debug.Log("Second finger touched screen");
        };
    }

    private void OnEnable()
    {
        firstFingerContactAction?.Enable();
        firstFingerPositionAction?.Enable();
        secondaryFingerContactAction?.Enable();
        secondaryFingerPositionAction?.Enable();
    }

    private void OnDisable()
    {
        firstFingerContactAction?.Disable();
        firstFingerPositionAction?.Disable();
        secondaryFingerContactAction?.Disable();
        secondaryFingerPositionAction?.Disable();
    }

    private void Update()
    {
        if (isDrawingPrimary)
        {
            OnUpdateDrawing?.Invoke(GetWorldPositionCoordinates(firstFingerPositionAction));
            Debug.Log("First finger moving on position: " + GetWorldPositionCoordinates(firstFingerPositionAction));
        }
        if (isDrawingSecondary)
        {
            OnUpdateDrawingSecondary?.Invoke(GetWorldPositionCoordinates(secondaryFingerPositionAction));
            Debug.Log("Second finger moving on position: " + GetWorldPositionCoordinates(secondaryFingerPositionAction));
        }
    }

    private Vector2 GetWorldPositionCoordinates(InputAction positionAction)
    {
        Vector2 screenCoordinates = positionAction.ReadValue<Vector2>();
        return Camera.main.ScreenToWorldPoint(screenCoordinates);
    }
}
