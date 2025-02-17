using UnityEngine;
using System.Collections;

public class EnemyMovementScript : MonoBehaviour
{
    [Header("Movement Limits")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Perlin Noise")]
    public float noiseSpeed = 1f;
    public float offsetX = 0f;
    public float offsetY = 10f;

    [Header("Pause Settings")]
    public float minPauseTime = 0.5f;
    public float maxPauseTime = 2f;
    public float pauseFrequency = 5f; 

    private float noiseTime = 0f;
    private bool isPaused = false;  

    void Start()
    {
        
        StartCoroutine(RandomPauseRoutine());
    }

    void Update()
    {
        // If paused, don't move
        if (isPaused) return;

        // We use Perlin Noise to generate a smooth random movement
        noiseTime += Time.deltaTime * noiseSpeed;

        float noiseValX = Mathf.PerlinNoise(noiseTime + offsetX, offsetY);
        float noiseValY = Mathf.PerlinNoise(offsetX, noiseTime + offsetY);

        float targetX = Mathf.Lerp(minX, maxX, noiseValX);
        float targetY = Mathf.Lerp(minY, maxY, noiseValY);

        // Smoothly move towards the target position
        Vector2 currentPos = transform.position;
        Vector2 targetPos = new Vector2(targetX, targetY);

        // Interpolate to the target position
        Vector2 newPos = Vector2.Lerp(currentPos, targetPos, 0.05f);

        // Clamp the position to the limits
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = newPos;
    }

    // Coroutine to generate random pauses during the game
    IEnumerator RandomPauseRoutine()
    {
        while (true)
        {
            // We wait a random time before the next pause
            float timeToNextPause = Random.Range(pauseFrequency * 0.5f, pauseFrequency * 1.5f);
            yield return new WaitForSeconds(timeToNextPause);

            // We pause the movement of the enemy
            isPaused = true;

            // Wait a random time before resuming movement
            float pauseDuration = Random.Range(minPauseTime, maxPauseTime);
            yield return new WaitForSeconds(pauseDuration);

            // Resume movement
            isPaused = false;
        }
    }
}
