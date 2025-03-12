using UnityEngine;

public class EnemyCollideLogicScript : MonoBehaviour
{
    // Reference to the LogicScript
    private LogicScript logicScript;

    // Reference to LineRendererController
    private LineRendererController lineRendererController;

    void Start()
    {
        // Find the LogicScript in the scene
        logicScript = FindFirstObjectByType<LogicScript>();
        lineRendererController = FindFirstObjectByType<LineRendererController>();
        if (logicScript == null)
        {
            Debug.LogError("LogicScript not in scene.");
        }
        if (lineRendererController == null)
        {
            Debug.LogError("LineRendererController not in scene.");
        }
    }

    // Check for collision with the pointer or the line renderer in GameController gameobject
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Pointer") || other.gameObject.CompareTag("DrawLine"))
        {
            // Call the TakeDamage method from the LogicScript
            logicScript.TakeDamage(10);
            // Call the ResetLineRenderer method from the LineRendererController
            lineRendererController.ResetLine();
        }
    }
}
