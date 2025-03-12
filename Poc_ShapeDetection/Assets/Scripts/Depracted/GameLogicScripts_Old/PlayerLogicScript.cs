using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isAlive = true;

    public TMP_Text healthText;

    private UIManager uiManager;

    void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();

        if (uiManager == null)
            Debug.LogError("UIManager no encontrado en la escena.");

        uiManager.ShowMainScreen();
    }

    void Start()
    {
        setHealthText(maxHealth);
        isAlive = true;
    }

    [ContextMenu("Take Damage")]
    public void TakeDamageInspector()
    {
        TakeDamage(10);
    }

    public void TakeDamage(int damage)
    {
        if (isAlive)
        {
            currentHealth -= damage;
            setHealthText(currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    [ContextMenu("Reset Health")]
    public void resetHealth()
    {
        currentHealth = maxHealth;
        setHealthText(currentHealth);
    }

    public void setHealthText(int health)
    {
        currentHealth = health;
        healthText.text = health + "/" + maxHealth;
    }

    [ContextMenu("Die")]
    public void Die()
    {
        Debug.Log("Player died.");
        isAlive = false;
        uiManager.ShowGameOverScreen();
    }
}
