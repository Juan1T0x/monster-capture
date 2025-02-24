using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Screens")]
    public GameObject mainScreen;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

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
