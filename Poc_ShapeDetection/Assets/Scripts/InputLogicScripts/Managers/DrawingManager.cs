using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    public LineRendererController lineController;
    public PointerController pointerController;
    public DebugPointController debugPointController;

    private DrawingInputHandler inputHandler;
    private RecognitionManager recognitionManager;

    void Awake()
    {

        inputHandler = FindFirstObjectByType<DrawingInputHandler>();
        recognitionManager = FindFirstObjectByType<RecognitionManager>();

        if (inputHandler == null)
            Debug.LogError("DrawingInputHandler is missing!");

        if (recognitionManager == null)
            Debug.LogError("RecognitionManager not found in scene!");
    }

    void OnEnable()
    {
        inputHandler.OnStartDrawing += HandleStartDrawing;
        inputHandler.OnUpdateDrawing += HandleUpdateDrawing;
        inputHandler.OnEndDrawing += HandleEndDrawing;
    }

    void OnDisable()
    {
        inputHandler.OnStartDrawing -= HandleStartDrawing;
        inputHandler.OnUpdateDrawing -= HandleUpdateDrawing;
        inputHandler.OnEndDrawing -= HandleEndDrawing;
    }

    void HandleStartDrawing(Vector2 startPos)
    {
        lineController.ResetLine();
        debugPointController.Clear();
        lineController.AddPoint(startPos);
        pointerController.UpdatePointerPosition(startPos);
    }

    void HandleUpdateDrawing(Vector2 currentPos)
    {
        lineController.AddPoint(currentPos);
        pointerController.UpdatePointerPosition(currentPos);
        debugPointController.UpdateFirstPoint(lineController.Points);
    }

    void HandleEndDrawing()
    {
        pointerController.HidePointer();
        recognitionManager.RecognizeShape(lineController.Points as System.Collections.Generic.List<Vector2>);
        lineController.ResetLine();
        debugPointController.Clear();
    }
}
