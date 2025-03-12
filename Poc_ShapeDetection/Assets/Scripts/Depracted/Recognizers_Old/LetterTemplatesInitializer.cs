using System.Collections.Generic;
using UnityEngine;

public class LetterTemplatesInitializer : MonoBehaviour
{
    public LetterRecognizer letterRecognizer; // A reference to the LetterRecognizer component

    void Start()
    {
        if(letterRecognizer == null)
            letterRecognizer = FindFirstObjectByType<LetterRecognizer>();

        // Letter J
        LetterRecognizer.LetterTemplate letterJ = new LetterRecognizer.LetterTemplate();
        letterJ.letter = "J";
        letterJ.points = new List<Vector2>() {
            new Vector2(1f, 1f),
            new Vector2(1f, 0.8f),
            new Vector2(1f, 0.6f),
            new Vector2(1f, 0.4f),
            new Vector2(1f, 0.2f),
            new Vector2(1f, 0f),
            new Vector2(0.8f, -0.2f),
            new Vector2(0.6f, -0.3f),
            new Vector2(0.4f, -0.3f),
            new Vector2(0.2f, -0.2f),
            new Vector2(0f, -0.1f)
        };

        // Add the template to the recognizer
        letterRecognizer.templates.Add(letterJ);
    }
}
