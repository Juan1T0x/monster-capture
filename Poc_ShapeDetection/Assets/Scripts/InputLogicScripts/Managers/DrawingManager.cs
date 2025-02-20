using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class DrawingManager : MonoBehaviour
{
    [Header("Drawing Settings")]
    public LineRenderer lineRenderer;
    public GameObject pointer;
    public GameObject pointPrefab;
    public bool debugPoints = true;

    [Tooltip("Minimum distance between points to add a new one, can also represent the 'resolution' of the line")]
    public float pointThreshold = 0.1f;

    [Header("Gameplay Settings")]
    [Tooltip("Maximum points allowed in a single drawing")]
    public float maxPoints = 20;

    private List<Vector2> points = new List<Vector2>();
    private GameObject firstPointInstance;
    private bool isDrawing = false;

    private RecognitionManager recognitionManager;

    // InputActions for the new Input System
    private InputAction drawAction;           // Detects press and release
    private InputAction pointerPositionAction; // Provides the position of the pointer

    void Awake()
    {
        // Configuration of the "draw" action
        drawAction = new InputAction("Draw", binding: "<Pointer>/press");
        drawAction.performed += ctx => StartDrawing();
        drawAction.canceled += ctx => EndDrawing();

        // Configuration of the "pointer position" action
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

    void Start()
    {
        recognitionManager = FindFirstObjectByType<RecognitionManager>();
        if (recognitionManager == null)
        {
            Debug.LogError("RecognitionManager not in scene.");
        }
    }

    void Update()
    {
        if (isDrawing)
        {
            UpdateDrawing();
        }
    }

    void StartDrawing()
    {
        isDrawing = true;
        points.Clear();
        lineRenderer.positionCount = 0;

        // Clear the first point prefab
        if (firstPointInstance != null)
        {
            Destroy(firstPointInstance);
            firstPointInstance = null;
        }
    }

    void UpdateDrawing()
    {
        // We get the position of the pointer and convert it to world coordinates
        Vector2 screenPos = pointerPositionAction.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

        // Update the position of the pointer
        pointer.transform.position = new Vector3(worldPos.x, worldPos.y, pointer.transform.position.z);
        if (!pointer.activeSelf)
            pointer.SetActive(true);

        // Add a new point if the distance between the last point and the new one is greater than the threshold
        // Or if there are no points yet
        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], worldPos) > pointThreshold)
        {
            // If we reached the maximum number of points, remove the oldest one
            if (points.Count >= maxPoints && points.Count > 0)
            {
                points.RemoveAt(0);
            }

            // Add the new point to the list
            points.Add(worldPos);

            // Add the new point to the LineRenderer
            lineRenderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }

        // Always make visible the first point of the drawing
        if (points.Count > 0)
        {
            // If the first point prefab doesn't exist, create it
            if (firstPointInstance == null)
            {
                firstPointInstance = Instantiate(pointPrefab, new Vector3(points[0].x, points[0].y, 0), Quaternion.identity);
                firstPointInstance.GetComponent<SpriteRenderer>().color = Color.red;
            }

            // Update the position of the first point prefab
            else
            {
                firstPointInstance.transform.position = new Vector3(points[0].x, points[0].y, 0);
            }
        }
    }

    void EndDrawing()
    {
        isDrawing = false;
        if (pointer.activeSelf)
            pointer.SetActive(false);

        // Send the list of points to the RecognitionManager
        if (recognitionManager != null)
        {
            recognitionManager.RecognizeShape(points);
        }

        // Clear the list of points and the LineRenderer
        points.Clear();
        lineRenderer.positionCount = 0;

        // Destroy the first point prefab
        if (firstPointInstance != null)
        {
            Destroy(firstPointInstance);
            firstPointInstance = null;
        }
    }
}
