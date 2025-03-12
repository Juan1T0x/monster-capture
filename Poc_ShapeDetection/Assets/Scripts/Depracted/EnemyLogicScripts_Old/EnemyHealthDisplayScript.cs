using UnityEngine;
using UnityEngine.UI; // o using TMPro; si usas TextMeshPro
using TMPro;

public class EnemyHealthDisplay : MonoBehaviour
{
    // Si usas UI Text
    public TMP_Text healthText;

    // Si usas TextMeshPro, en su lugar:
    // public TMPro.TextMeshProUGUI healthText;

    private EnemyHealthLogicScript enemyHealth;

    void Awake()
    {
        // Suponemos que el prefab Enemy tiene el script de salud en el mismo objeto raíz
        enemyHealth = GetComponentInParent<EnemyHealthLogicScript>();

        if (healthText == null)
        {
            Debug.LogError("No se asignó el componente de texto en " + gameObject.name);
        }
    }

    void Update()
    {
        // Actualiza el texto con la vida actual y la vida máxima
        healthText.text = enemyHealth.currentHealth + " / " + enemyHealth.maxHealth;
    }
}
