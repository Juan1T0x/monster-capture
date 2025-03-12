using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternDisplayManager : MonoBehaviour
{
    // Classes to deserialize the JSON
    [System.Serializable]
    public class PatternStep
    {
        public string gesture;
        public string parametersJson; // Stored as a JSON string
    }

    [System.Serializable]
    public class RecognitionPattern
    {
        public string patternName;
        public List<PatternStep> steps;
    }

    // Helper class for converting a JSON array using JsonUtility
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }

    // Array of patterns loaded from JSON
    public RecognitionPattern[] recognitionPatterns;

    // The currently selected pattern and its current step index
    private RecognitionPattern currentPattern;
    private int currentStepIndex = 0;

    // GUI customization options
    [Header("GUI Settings")]
    [Tooltip("Font size for the pattern text")]
    public int fontSize = 20;
    [Tooltip("Text color for the pattern display")]
    public Color textColor = Color.white;
    [Tooltip("Margin from the bottom left of the screen")]
    public Vector2 margin = new Vector2(10, 10);
    [Tooltip("Size of the pattern display label")]
    public Vector2 labelSize = new Vector2(500, 100);

    private GUIStyle guiStyle;

    private void Start()
    {
        LoadPatterns();
        ChooseRandomPattern();
        DisplayCurrentStep();
    }

    // Loads the JSON file from Resources and deserializes it
    void LoadPatterns()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("RecognitionPatterns");
        if (jsonText != null)
        {
            recognitionPatterns = JsonHelper.FromJson<RecognitionPattern>(jsonText.text);
        }
        else
        {
            Debug.LogError("Could not load RecognitionPatterns.json from Resources.");
        }
    }

    // Randomly selects a pattern and resets the step index
    void ChooseRandomPattern()
    {
        if (recognitionPatterns != null && recognitionPatterns.Length > 0)
        {
            int index = Random.Range(0, recognitionPatterns.Length);
            currentPattern = recognitionPatterns[index];
            currentStepIndex = 0;
            Debug.Log("Selected Pattern: " + currentPattern.patternName);
        }
        else
        {
            Debug.LogError("No patterns loaded.");
        }
    }

    // Displays the current step of the selected pattern in the console
    void DisplayCurrentStep()
    {
        if (currentPattern != null)
        {
            if (currentStepIndex < currentPattern.steps.Count)
            {
                PatternStep step = currentPattern.steps[currentStepIndex];
                Debug.Log("Current Step " + (currentStepIndex + 1) + ": " + step.gesture + " | Parameters: " + step.parametersJson);
            }
            else
            {
                Debug.Log("pattern recognized");
            }
        }
    }

    // Function that can be called from the Inspector to advance to the next step
    [ContextMenu("Advance Step")]
    public void AdvanceStep()
    {
        if (currentPattern == null)
        {
            Debug.LogError("No current pattern selected.");
            return;
        }

        currentStepIndex++;
        if (currentStepIndex >= currentPattern.steps.Count)
        {
            Debug.Log("pattern recognized");
        }
        else
        {
            DisplayCurrentStep();
        }
    }

    // Function to change the pattern and reset the progress (callable from the Inspector)
    [ContextMenu("Change Pattern")]
    public void ChangePattern()
    {
        ChooseRandomPattern();
        DisplayCurrentStep();
    }

    // Display the pattern and current step on screen using OnGUI (positioned at bottom left)
    private void OnGUI()
    {
        if (currentPattern != null)
        {
            // Initialize the GUIStyle if needed
            if (guiStyle == null)
            {
                guiStyle = new GUIStyle(GUI.skin.label);
                guiStyle.fontSize = fontSize;
                guiStyle.normal.textColor = textColor;
            }

            string displayText = "Pattern: " + currentPattern.patternName + "\n";
            if (currentStepIndex < currentPattern.steps.Count)
            {
                PatternStep step = currentPattern.steps[currentStepIndex];
                displayText += "Step " + (currentStepIndex + 1) + ": " + step.gesture + "\n";
                displayText += "Parameters: " + step.parametersJson;
            }
            else
            {
                displayText += "pattern recognized";
            }
            // Position at the bottom left (x = margin.x, y = Screen.height - labelSize.y - margin.y)
            GUI.Label(new Rect(margin.x, Screen.height - labelSize.y - margin.y, labelSize.x, labelSize.y), displayText, guiStyle);
        }
    }
}
