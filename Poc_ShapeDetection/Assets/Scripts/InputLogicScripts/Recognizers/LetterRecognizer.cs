using System.Collections.Generic;
using UnityEngine;

public class LetterRecognizer : MonoBehaviour, IShapeRecognizer
{
    [Header("Letter Recognition Configuration")]
    [Tooltip("Amount of points to resample the candidate shape.")]
    public int resamplePointsCount = 64;
    
    [Tooltip("Minimum score required to recognize a letter.")]
    public float recognitionThreshold = 0.75f;
    
    [Tooltip("List of letter templates.")]
    public List<LetterTemplate> templates;

    [System.Serializable]
    public class LetterTemplate
    {
        public string letter;          // Letter name
        public List<Vector2> points;   // Points that define the letter
    }

    public bool Recognize(List<Vector2> points, out RecognitionResult result)
    {
        result = new RecognitionResult();

        // Verify minimum number of points
        if (points.Count < 10)
            return false;

        // Preprocess the candidate shape: resample and normalize
        List<Vector2> candidate = Resample(points, resamplePointsCount);
        candidate = Normalize(candidate);

        float bestDistance = float.MaxValue;
        string bestLetter = "";

        // Compare the candidate shape with each template
        foreach (LetterTemplate template in templates)
        {
            // Preprocess the template shape: resample and normalize
            List<Vector2> templatePoints = Resample(template.points, resamplePointsCount);
            templatePoints = Normalize(templatePoints);

            // Calculate the distance between the candidate and the template
            float distance = PathDistance(candidate, templatePoints);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestLetter = template.letter;
            }
        }

        // Calculate the score based on the best distance
        // (1 - bestDistance) is a value between 0 and 1
        float score = Mathf.Max(0, 1 - bestDistance);

        if (score >= recognitionThreshold)
        {
            result.shapeName = bestLetter;
            result.score = score;
            return true;
        }

        return false;
    }

    // Resample the points to a fixed number of points
    private List<Vector2> Resample(List<Vector2> points, int n)
    {
        float I = PathLength(points) / (n - 1); // Interval length
        float D = 0;
        List<Vector2> newPoints = new List<Vector2>();
        newPoints.Add(points[0]);

        for (int i = 1; i < points.Count; i++)
        {
            float d = Vector2.Distance(points[i - 1], points[i]);
            if ((D + d) >= I)
            {
                float t = (I - D) / d;
                Vector2 newPoint = Vector2.Lerp(points[i - 1], points[i], t);
                newPoints.Add(newPoint);
                points.Insert(i, newPoint); // Insert newPoint at position i
                D = 0;
            }
            else
            {
                D += d;
            }
        }

        // If we didn't add the last point, add it now
        if (newPoints.Count == n - 1)
            newPoints.Add(points[points.Count - 1]);

        return newPoints;
    }

    // Calculate the length of a path (list of points)
    private float PathLength(List<Vector2> points)
    {
        float length = 0;
        for (int i = 1; i < points.Count; i++)
        {
            length += Vector2.Distance(points[i - 1], points[i]);
        }
        return length;
    }

    // Normalize the points to a common scale and position
    private List<Vector2> Normalize(List<Vector2> points)
    {
        // Move the points so that the centroid is at the origin
        Vector2 centroid = Centroid(points);
        List<Vector2> newPoints = new List<Vector2>();
        foreach (Vector2 p in points)
        {
            newPoints.Add(p - centroid);
        }

        // Scale the points so that the bounding box is of size 1
        float scale = 1f / BoundingBoxSize(newPoints);
        for (int i = 0; i < newPoints.Count; i++)
        {
            newPoints[i] *= scale;
        }

        return newPoints;
    }

    // Calculate the centroid of a list of points
    private Vector2 Centroid(List<Vector2> points)
    {
        Vector2 sum = Vector2.zero;
        foreach (Vector2 p in points)
        {
            sum += p;
        }
        return sum / points.Count;
    }

    // Calculate the size of the bounding box of a list of points
    private float BoundingBoxSize(List<Vector2> points)
    {
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        foreach (Vector2 p in points)
        {
            if (p.x < minX) minX = p.x;
            if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y;
            if (p.y > maxY) maxY = p.y;
        }

        return Mathf.Max(maxX - minX, maxY - minY);
    }

    // Calculate the average distance between two paths
    private float PathDistance(List<Vector2> a, List<Vector2> b)
    {
        float distance = 0;
        for (int i = 0; i < a.Count; i++)
        {
            distance += Vector2.Distance(a[i], b[i]);
        }
        return distance / a.Count;
    }
}
