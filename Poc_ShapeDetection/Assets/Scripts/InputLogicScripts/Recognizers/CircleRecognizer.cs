using System.Collections.Generic;
using UnityEngine;

public class CircleRecognizer : MonoBehaviour, IShapeRecognizer
{
    [Header("Circle Configuration")]
    [Tooltip("Minimum number of points required to recognize a circle.")]
    public int minimumPoints = 10;

    [Tooltip("Maximum distance between the first and last point to consider the drawing closed.")]
    public float closureThreshold = 0.5f;

    [Tooltip("Maximum variance to consider the drawing a circle.")]
    public float varianceThreshold = 0.5f;
    
    public bool Recognize(List<Vector2> points, out RecognitionResult result) {
        
        result = new RecognitionResult();
        
        // Verify minimum number of points
        if (points.Count < minimumPoints) return false;
        
        // Verify that the drawing is closed (first and last point are close)
        // TODO: better closure check
        if (Vector2.Distance(points[0], points[points.Count - 1]) > closureThreshold)
            return false;
        
        // Calculate the center of the drawing by averaging all points
        Vector2 center = Vector2.zero;
        foreach (Vector2 p in points) {
            center += p;
        }
        center /= points.Count;
        
        // Calculate the average radius and variance

        // Average radius is calculated as the average distance from the center to each point
        float sumRadius = 0f;
        List<float> radii = new List<float>();
        foreach (Vector2 p in points) {
            float d = Vector2.Distance(p, center);
            radii.Add(d);
            sumRadius += d;
        }
        float avgRadius = sumRadius / points.Count;
        
        // Variance is calculated as the average absolute difference between each radius and the average radius
        float variance = 0f;
        foreach (float r in radii) {
            variance += Mathf.Abs(r - avgRadius);
        }
        variance /= points.Count;
        
        // If the variance is lower than the threshold, consider it a circle
        if (variance < varianceThreshold) {
            result.shapeName = "Circle";
            result.score = 1 - (variance / varianceThreshold); // Score between 0 y 1
            return true;
        }
        
        return false;
    }
}
