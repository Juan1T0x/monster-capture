using System.Collections.Generic;
using UnityEngine;

public class CircleRecognizer : MonoBehaviour, IShapeRecognizer
{
    [Header("Circle Configuration")]
    public int minimumPoints = 10;
    public float closureThreshold = 0.5f;
    public float varianceThreshold = 0.5f;
    
    public bool Recognize(List<Vector2> points, out RecognitionResult result) {
        result = new RecognitionResult();
        
        // Verify minimum number of points
        if (points.Count < minimumPoints) return false;
        
        // Verify that the drawing is closed
        if (Vector2.Distance(points[0], points[points.Count - 1]) > closureThreshold)
            return false;
        
        // Calculate the center of the drawing
        Vector2 center = Vector2.zero;
        foreach (Vector2 p in points) {
            center += p;
        }
        center /= points.Count;
        
        // Calculate the average radius and variance
        float sumRadius = 0f;
        List<float> radii = new List<float>();
        foreach (Vector2 p in points) {
            float d = Vector2.Distance(p, center);
            radii.Add(d);
            sumRadius += d;
        }
        float avgRadius = sumRadius / points.Count;
        
        float variance = 0f;
        foreach (float r in radii) {
            variance += Mathf.Abs(r - avgRadius);
        }
        variance /= points.Count;
        
        // If the variance is low, it is assumed to be a circle
        if (variance < varianceThreshold) {
            result.shapeName = "Circle";
            result.score = 1 - (variance / varianceThreshold); // Score entre 0 y 1
            result.circleCenter = center;
            result.circleRadius = avgRadius;
            return true;
        }
        
        return false;
    }
}
