using System.Collections.Generic;
using UnityEngine;

public class RecognitionManager : MonoBehaviour
{
    private List<IShapeRecognizer> recognizers = new List<IShapeRecognizer>();

    void Awake() {
        // Look for all components that implement IShapeRecognizer in this GameObject or its children
        IShapeRecognizer[] foundRecognizers = GetComponentsInChildren<IShapeRecognizer>();
        if (foundRecognizers != null && foundRecognizers.Length > 0) {
            recognizers.AddRange(foundRecognizers);
        } else {
            Debug.LogWarning("No shape recognizers found.");
        }
    }
    
    // Method that iterates through all recognizers to recognize the shape and print the result
    public void RecognizeShape(List<Vector2> points) {
        foreach (IShapeRecognizer recognizer in recognizers) {
            RecognitionResult result;
            if (recognizer.Recognize(points, out result)) {
                Debug.Log("Shape recognized: " + result.shapeName + " (score: " + result.score + ")");
                return;
            }
        }
        Debug.Log("Shape not recognized.");
    }
}
