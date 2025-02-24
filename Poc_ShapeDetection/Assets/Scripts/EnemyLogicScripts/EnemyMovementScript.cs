using UnityEngine;
using System.Collections;

public class EnemyMovementScript : MonoBehaviour
{
    [Header("Perlin Noise Settings")]
    [Tooltip("Velocidad del ruido Perlin para el movimiento.")]
    public float noiseSpeed = 1f;
    [Tooltip("Offset para variar la señal de ruido en el eje X.")]
    public float offsetX = 0f;
    [Tooltip("Offset para variar la señal de ruido en el eje Y.")]
    public float offsetY = 10f;

    private float noiseTime = 0f;
    private Vector2 centerPosition; // Centro de movimiento: queremos que sea (0,0)
    private float maxRadius;       // Radio máximo determinado por los bordes de la cámara

    void Start()
    {
        // Establecemos el centro de movimiento en (0,0)
        centerPosition = Vector2.zero;

        // Posicionamos al enemigo en el centro al iniciar.
        transform.position = centerPosition;

        // Calculamos el radio máximo basado en la cámara principal (asumiendo que es ortográfica).
        Camera cam = Camera.main;
        if (cam != null && cam.orthographic)
        {
            // La mitad del alto de la cámara
            float halfHeight = cam.orthographicSize;
            // La mitad del ancho se obtiene multiplicando por el aspect ratio
            float halfWidth = cam.orthographicSize * cam.aspect;
            // Elegimos el menor para asegurar que nos quedamos dentro de la vista
            maxRadius = Mathf.Min(halfWidth, halfHeight);
        }
        else
        {
            // Valor de respaldo en caso de que la cámara no esté configurada o no sea ortográfica.
            maxRadius = 5f;
        }
    }

    void Update()
    {
        // Incrementamos el tiempo de ruido.
        noiseTime += Time.deltaTime * noiseSpeed;

        // Obtenemos un valor de ruido para el ángulo y lo mapeamos a [0, 2π].
        float noiseValAngle = Mathf.PerlinNoise(noiseTime + offsetX, offsetY);
        float angle = noiseValAngle * 2 * Mathf.PI;

        // Obtenemos un valor de ruido para el radio y lo interpolamos de 0 a maxRadius.
        float noiseValRadius = Mathf.PerlinNoise(offsetX, noiseTime + offsetY);
        float targetRadius = Mathf.Lerp(0f, maxRadius, noiseValRadius);

        // Calculamos el offset a partir del ángulo y el radio.
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * targetRadius;

        // Establecemos la posición del enemigo: centro + offset.
        transform.position = centerPosition + offset;
    }
}
