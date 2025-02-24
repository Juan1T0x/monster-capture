using UnityEngine;
using System.Collections.Generic;

public class LineRendererController : MonoBehaviour
{
    [Header("Line Renderer Settings")]
    public LineRenderer lineRenderer;
    public float pointThreshold = 0.1f;
    public int maxPoints = 20;

    private List<Vector2> points = new List<Vector2>();

    public IReadOnlyList<Vector2> Points => points;

    // TODO: estructura para guardar los segmentos de línea junto con su collider
    /*

    struct LineSegment
    {
        public Vector2 start;
        public Vector2 end;
        public BoxCollider2D collider;
    }

    */

    public void Awake()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    public void ResetLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    public void AddPoint(Vector2 newPoint)
    {
        // Solo agrega si es el primer punto o si se ha movido lo suficiente
        if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], newPoint) > pointThreshold)
        {
            // Si se alcanzó el máximo, elimina el más antiguo
            if (points.Count >= maxPoints && points.Count > 0)
            {
                points.RemoveAt(0);
            }

            points.Add(newPoint);
            UpdateLineRenderer();
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    // private void UpdateLineRendererCollider()
}
