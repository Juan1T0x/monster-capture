using System;
using UnityEngine;

public class EnemyHealthLogicScript : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Evento de instancia (opcional si se desea para casos individuales)
    public event Action OnEnemyKilled;

    // Evento global para notificar que cualquier enemigo ha sido eliminado
    public static event Action OnAnyEnemyKilled;

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
        // Aquí podrías actualizar algún indicador visual si lo deseas
    }

    void Die()
    {
        Debug.Log("Enemy killed.");
        // Notificar a los suscriptores locales (si los hubiera)
        OnEnemyKilled?.Invoke();
        // Notificar a todos los interesados globalmente
        OnAnyEnemyKilled?.Invoke();

        Destroy(gameObject);
    }
}
