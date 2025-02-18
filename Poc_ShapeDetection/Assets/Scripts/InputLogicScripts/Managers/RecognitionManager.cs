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
    
    // Method invoked when drawing is finished
    public void RecognizeShape(List<Vector2> points) {
        foreach (IShapeRecognizer recognizer in recognizers) {
            RecognitionResult result;
            if (recognizer.Recognize(points, out result)) {
                Debug.Log("Shape recognized: " + result.shapeName + " (score: " + result.score + ")");
                if (result.shapeName == "Circle") {
                    CheckEnemiesInsideCircle(result.circleCenter, result.circleRadius);
                }
                return;
            }
        }
        Debug.Log("Shape not recognized.");
    }

    private void CheckEnemiesInsideCircle(Vector2 center, float radius) {
        // Draw the circle for debugging purposes
        Debug.DrawLine(center, center + Vector2.right * radius, Color.red, 2f);
        
        // Check all colliders inside the circle
        Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius);
        foreach (Collider2D col in colliders) {
            if (col.CompareTag("Enemy")) {
                Debug.Log("The enemy " + col.name + " is inside the circle.");
                // Action to take when an enemy is inside the circle
                // col.GetComponent<EnemyScript>().TakeDamage(10);
            }
        }
    }
}
