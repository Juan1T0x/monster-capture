using UnityEngine;
using System.Collections.Generic;

public class LineRendererController : MonoBehaviour
{
    [Header("Line Renderer Settings")]
    public LineRenderer lineRenderer;
    public float pointThreshold = 0.1f;
    public int maxPoints = 20;

    [Header("Debug Settings")]
    public bool debugDrawGizmos = true;
    public Color debugColor = Color.yellow;
    public float gizmoSphereRadius = 0.05f;

    private List<Vector2> points = new List<Vector2>();
    public IReadOnlyList<Vector2> Points => points;

    private void Awake()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    public void ResetLine()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    public void AddPoint(Vector2 newPointCoordinates)
    {
        if (points.Count == 0 ||
            Vector2.Distance(points[points.Count - 1], newPointCoordinates) > pointThreshold)
        {
            if (points.Count >= maxPoints && points.Count > 0)
            {
                points.RemoveAt(0);
            }

            points.Add(newPointCoordinates);
            UpdateLineRenderer();
        }
    }

    private void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, 0f));
        }
    }


    private void OnDrawGizmos()
    {
        if (!debugDrawGizmos) return;

        Gizmos.color = debugColor;

        for (int i = 0; i < points.Count; i++)
        {

            Vector3 currentPoint = new Vector3(points[i].x, points[i].y, 0f);
            Gizmos.DrawSphere(currentPoint, gizmoSphereRadius);

            if (i > 0)
            {
                Vector3 previousPoint = new Vector3(points[i - 1].x, points[i - 1].y, 0f);
                Gizmos.DrawLine(previousPoint, currentPoint);
            }
        }
    }
}
