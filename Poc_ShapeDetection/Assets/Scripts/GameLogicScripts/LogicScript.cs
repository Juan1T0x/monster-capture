using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth = 100;
    public TMP_Text healthText;

    void Start()
    {
        setHealthText(maxHealth);
    }

    [ContextMenu("Take Damage")]
    public void TakeDamageInspector()
    {
        TakeDamage(10);
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        setHealthText(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
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
        healthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    void Die()
    {
        // TODO: Add Game Over logic
        Debug.Log("Player died.");
    }
}
