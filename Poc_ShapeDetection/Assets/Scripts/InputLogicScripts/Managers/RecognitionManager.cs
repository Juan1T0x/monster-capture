using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class RecognitionManager : MonoBehaviour
{
    [Header("Shape Recognizers")]
    [Tooltip("List of shape recognizers to use (must be scripts that implement IShapeRecognizer)")]
    private List<IShapeRecognizer> recognizers = new List<IShapeRecognizer>();

    [Header("UI Elements")]
    [Tooltip("Text to display when a shape is recognized")]
    public TMP_Text shapeOkText;

    // Lanzar evento cuando se reconozca una forma
    public event Action<RecognitionResult> OnShapeRecognized;

    void Awake()
    {
        // Look for all components that implement IShapeRecognizer in this GameObject or its children
        IShapeRecognizer[] foundRecognizers = GetComponentsInChildren<IShapeRecognizer>();
        if (foundRecognizers != null && foundRecognizers.Length > 0)
        {
            recognizers.AddRange(foundRecognizers);
        }
        else
        {
            Debug.LogWarning("No shape recognizers found.");
        }
        shapeOkText.gameObject.SetActive(false);

    }

    // Method invoked when drawing is finished
    public void RecognizeShape(List<Vector2> points)
    {
        foreach (IShapeRecognizer recognizer in recognizers)
        {
            RecognitionResult result;
            if (recognizer.Recognize(points, out result))
            {
                Debug.Log("Shape recognized: " + result.shapeName + " (score: " + result.score + ")");
                displayShapeRecognizedText(result.shapeName);
                // Send shape recognized event to subscribers
                OnShapeRecognized?.Invoke(result);
                return;

            }
        }
        Debug.Log("Shape not recognized.");
    }

    public void displayShapeRecognizedText(String shapeName)
    {
        shapeOkText.text = shapeName;
        // Display it for 2 seconds then make it inactive again
        shapeOkText.gameObject.SetActive(true);
        Invoke("hideShapeRecognizedText", 1);

    }

    public void hideShapeRecognizedText()
    {
        shapeOkText.gameObject.SetActive(false);
    }
}
