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
    public float pointThreshold = 0.1f;
    
    private List<Vector2> points = new List<Vector2>();
    private List<GameObject> pointList = new List<GameObject>();
    private bool isDrawing = false;
    
    private RecognitionManager recognitionManager;
    
    // InputActions para el nuevo Input System
    private InputAction drawAction;           // Para detectar presión (inicio y fin del trazo)
    private InputAction pointerPositionAction; // Para obtener la posición del puntero

    void Awake()
    {
        // Configuramos la acción de "draw" (se dispara al presionar o soltar el dedo/mouse)
        drawAction = new InputAction("Draw", binding: "<Pointer>/press");
        drawAction.performed += ctx => StartDrawing();
        drawAction.canceled  += ctx => EndDrawing();

        // Configuramos la acción para obtener la posición del puntero
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
        // Actualizamos el trazo mientras se esté dibujando
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
        
        // Limpiar puntos de debug anteriores
        foreach (var p in pointList)
        {
            Destroy(p);
        }
        pointList.Clear();
    }
    
    void UpdateDrawing()
    {
        // Obtenemos la posición del puntero usando el nuevo Input System
        Vector2 screenPos = pointerPositionAction.ReadValue<Vector2>();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        
        // Actualizar la posición del puntero visual
        pointer.transform.position = new Vector3(worldPos.x, worldPos.y, pointer.transform.position.z);
        if (!pointer.activeSelf)
            pointer.SetActive(true);
        
        // Agregar un punto si es el primero o se ha movido lo suficiente
        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], worldPos) > pointThreshold)
        {
            points.Add(worldPos);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, worldPos);
            
            if (debugPoints)
            {
                GameObject pointInstance = Instantiate(pointPrefab, new Vector3(worldPos.x, worldPos.y, 0), Quaternion.identity);
                pointList.Add(pointInstance);
            }
        }
    }
    
    void EndDrawing()
    {
        isDrawing = false;
        if (pointer.activeSelf)
            pointer.SetActive(false);
        
        // Enviar los puntos al RecognitionManager
        if (recognitionManager != null)
        {
            recognitionManager.RecognizeShape(points);
        }
        
        // Limpiar puntos y línea
        points.Clear();
        lineRenderer.positionCount = 0;
        foreach (var p in pointList)
        {
            Destroy(p);
        }
        pointList.Clear();
    }
}
