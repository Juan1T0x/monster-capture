using System.Collections.Generic;
using UnityEngine;

public struct RecognitionResult {
    public string shapeName;
    public float score;

    // Info about the recognized shape

    // Circle
    public Vector2 circleCenter;
    public float circleRadius;
}

public interface IShapeRecognizer
{
    // Tries to recognize the shape from the list of points.
    // Returns true if it is recognized and assigns information in 'result'.
    bool Recognize(List<Vector2> points, out RecognitionResult result);
}
