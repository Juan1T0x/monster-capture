using UnityEngine;
using System.Collections.Generic;

public class DrawingManager : MonoBehaviour
{
    [Header("Drawing Settings")]
    public LineRenderer lineRenderer;
    public GameObject pointer;
    public GameObject pointPrefab;
    public bool debugPoints = true;
    
    private List<Vector2> points = new List<Vector2>();
    private List<GameObject> pointList = new List<GameObject>();
    private bool isDrawing = false;
    
    private RecognitionManager recognitionManager;

    void Start() {
        recognitionManager = FindFirstObjectByType<RecognitionManager>();
        if (recognitionManager == null) {
            Debug.LogError("RecognitionManager not in scene.");
        }
    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartDrawing();
        }
        if (isDrawing) {
            UpdateDrawing();
        }
        if (Input.GetMouseButtonUp(0) && isDrawing) {
            EndDrawing();
        }
    }
    
    void StartDrawing() {
        isDrawing = true;
        points.Clear();
        lineRenderer.positionCount = 0;
        
        // Clear previous points
        foreach (var p in pointList) {
            Destroy(p);
        }
        pointList.Clear();
    }
    
    void UpdateDrawing() {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Update pointer position
        pointer.transform.position = new Vector3(mousePos.x, mousePos.y, pointer.transform.position.z);
        if (!pointer.activeSelf)
            pointer.SetActive(true);
        
        // Add a point if it's the first one or if it has moved enough within a threshold
        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], mousePos) > 0.1f) {
            points.Add(mousePos);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, mousePos);
            
            if (debugPoints) {
                GameObject pointInstance = Instantiate(pointPrefab, new Vector3(mousePos.x, mousePos.y, 0), Quaternion.identity);
                pointList.Add(pointInstance);
            }
        }
    }
    
    void EndDrawing() {
        isDrawing = false;
        if (pointer.activeSelf)
            pointer.SetActive(false);
        
        // Send the points to the recognition manager
        if (recognitionManager != null) {
            recognitionManager.RecognizeShape(points);
        }
        
        // Clear the points
        points.Clear();
        lineRenderer.positionCount = 0;
        foreach (var p in pointList) {
            Destroy(p);
        }
        pointList.Clear();
    }
}
