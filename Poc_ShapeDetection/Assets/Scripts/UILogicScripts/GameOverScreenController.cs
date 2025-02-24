using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreenController : MonoBehaviour
{
    [SerializeField] private Button restartButton;

    void Awake()
    {
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(RestartScene);
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
