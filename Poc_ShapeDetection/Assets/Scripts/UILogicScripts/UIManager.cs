using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject mainScreen;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

    void OnEnable()
    {
        // Nos suscribimos al evento global de muerte de enemigos
        EnemyHealthLogicScript.OnAnyEnemyKilled += HandleEnemyKilled;
    }

    void OnDisable()
    {
        EnemyHealthLogicScript.OnAnyEnemyKilled -= HandleEnemyKilled;
    }

    // Este método se invoca cada vez que se mata un enemigo
    private void HandleEnemyKilled()
    {
        // Por ejemplo, si la condición de victoria es matar a 1 enemigo, mostramos la pantalla de ganar.
        // Si es por contar varios enemigos, aquí podrías incrementar un contador y comprobar la condición.
        ShowGameWinScreen();
    }

    public void ShowMainScreen()
    {
        if (mainScreen) mainScreen.SetActive(true);
        if (gameOverScreen) gameOverScreen.SetActive(false);
        if (gameWinScreen) gameWinScreen.SetActive(false);
    }

    public void ShowGameOverScreen()
    {
        if (mainScreen) mainScreen.SetActive(false);
        if (gameOverScreen) gameOverScreen.SetActive(true);
        if (gameWinScreen) gameWinScreen.SetActive(false);
    }

    public void ShowGameWinScreen()
    {
        if (mainScreen) mainScreen.SetActive(false);
        if (gameOverScreen) gameOverScreen.SetActive(false);
        if (gameWinScreen) gameWinScreen.SetActive(true);
    }
}
