using System.Collections.Generic;
using UnityEngine;

public class RecognitionManager : MonoBehaviour
{
    [Header("Shape Recognizers")]
    [Tooltip("List of shape recognizers to use (must be scripts that implement IShapeRecognizer)")]
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
    
    // Method invoked when drawing is finished
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
