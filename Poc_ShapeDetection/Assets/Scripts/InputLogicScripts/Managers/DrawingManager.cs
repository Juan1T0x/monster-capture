using UnityEngine;

public class DrawingManager : MonoBehaviour
{
    [Header("Lines Drawn Logic")]
    [SerializeField] private LineRenderer lineRendererPrefab;

    [Header("References to Controllers")]
    public LineRendererController lineRendererController;
    public StylusController stylusController;
    public VisualPointsController visualPointsController;
    
    [Header("References to Managers")]
    public LineStorageManager lineStorageManager; // Nueva referencia

    private InputHandler inputHandler;

    private void Awake()
    {
        inputHandler = FindFirstObjectByType<InputHandler>();
    }

    private void OnEnable()
    {
        inputHandler.OnStartDrawing += HandleStartDrawing;
        inputHandler.OnUpdateDrawing += HandleUpdateDrawing;
        inputHandler.OnEndDrawing += HandleEndDrawing;
    }

    private void OnDisable()
    {
        inputHandler.OnStartDrawing -= HandleStartDrawing;
        inputHandler.OnUpdateDrawing -= HandleUpdateDrawing;
        inputHandler.OnEndDrawing -= HandleEndDrawing;
    }

    private void HandleStartDrawing(Vector2 startCoordinates)
    {
        lineRendererController.ResetLine();
        visualPointsController.Clear();

        stylusController.UpdatePointerPosition(startCoordinates);
        lineRendererController.AddPoint(startCoordinates);
        visualPointsController.CreateFirstPoint(startCoordinates);
    }

    private void HandleUpdateDrawing(Vector2 currentCoordinates)
    {
        stylusController.UpdatePointerPosition(currentCoordinates);
        lineRendererController.AddPoint(currentCoordinates);
        visualPointsController.UpdateFirstPoint(lineRendererController.Points);
    }

    private void HandleEndDrawing()
    {
        LineRenderer newLine = CopyCurrentLineRenderer();
        lineStorageManager.AddLine(newLine);
        stylusController.HidePointer();
        lineRendererController.ResetLine();
        visualPointsController.Clear();
    }

    private LineRenderer CopyCurrentLineRenderer()
    {
        LineRenderer newLineRenderer = Instantiate(lineRendererPrefab, lineRendererController.transform.parent);
        newLineRenderer.positionCount = lineRendererController.lineRenderer.positionCount;
        for (int i = 0; i < newLineRenderer.positionCount; i++)
        {
            newLineRenderer.SetPosition(i, lineRendererController.lineRenderer.GetPosition(i));
        }
        return newLineRenderer;
    }
}
