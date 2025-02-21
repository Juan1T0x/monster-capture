using UnityEngine;
using TMPro;

public class EnemyHealthDisplay : MonoBehaviour
{
    // Asigna este campo en el inspector (arrastrar el TextMeshProUGUI que está en el Canvas hijo)
    public TextMeshProUGUI healthText;

    private EnemyHealthLogicScript enemyHealth;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealthLogicScript>();
        if (healthText == null)
        {
            Debug.LogError("No se asignó el componente TextMeshProUGUI en " + gameObject.name);
        }
    }

    void Update()
    {
        // Actualiza el texto con el formato "current / max"
        healthText.text = enemyHealth.currentHealth + " / " + enemyHealth.maxHealth;
        // Que el texto siempre esté encima del enemigo
        healthText.transform.position = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);
    }
}
