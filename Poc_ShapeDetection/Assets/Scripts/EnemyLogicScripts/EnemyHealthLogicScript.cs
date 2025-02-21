using UnityEngine;

public class EnemyHealthLogicScript : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

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
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [ContextMenu("Reset Health")]
    public void resetHealth()
    {
        currentHealth = maxHealth;
    }


    public void setHealthText(int health)
    {
        currentHealth = health;
    }


    void Die()
    {
        // TODO: Destroy the enemy object
        Debug.Log("Enemy killed.");
        // Destroy the enemy object
        Destroy(gameObject);
    }
}
