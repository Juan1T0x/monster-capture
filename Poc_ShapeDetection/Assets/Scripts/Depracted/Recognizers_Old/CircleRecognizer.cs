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

    public bool Recognize(List<Vector2> points, out RecognitionResult result)
    {
        result = new RecognitionResult();

        // Verificar cantidad mínima de puntos
        if (points.Count < minimumPoints) return false;

        // Verificar que el dibujo esté cerrado (primer y último punto cercanos)
        if (Vector2.Distance(points[0], points[points.Count - 1]) > closureThreshold)
            return false;

        // Calcular el centro del dibujo
        Vector2 center = Vector2.zero;
        foreach (Vector2 p in points)
        {
            center += p;
        }
        center /= points.Count;

        // Calcular el radio promedio y la varianza
        float sumRadius = 0f;
        List<float> radii = new List<float>();
        foreach (Vector2 p in points)
        {
            float d = Vector2.Distance(p, center);
            radii.Add(d);
            sumRadius += d;
        }
        float avgRadius = sumRadius / points.Count;

        float variance = 0f;
        foreach (float r in radii)
        {
            variance += Mathf.Abs(r - avgRadius);
        }
        variance /= points.Count;

        // Si la varianza es menor al umbral, se considera un círculo
        if (variance < varianceThreshold)
        {
            result.shapeName = "Circle";
            result.score = 1 - (variance / varianceThreshold);

            // Comprobar si en el interior del círculo hay algún GameObject con la tag "Enemy"
            Collider2D[] colliders = Physics2D.OverlapCircleAll(center, avgRadius);
            foreach (Collider2D col in colliders)
            {
                if (col.CompareTag("Enemy"))
                {
                    // Acceder al script de salud y aplicar daño
                    EnemyHealthLogicScript enemyHealth = col.GetComponent<EnemyHealthLogicScript>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.TakeDamage(10); // Por ejemplo, aplicar 10 puntos de daño
                        Debug.Log("Daño aplicado a " + col.name);
                    }
                }
            }
            return true;
        }

        return false;
    }

}
