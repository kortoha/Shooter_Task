using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private EnemyDamage _enemyDamage;
    [SerializeField] private Image _barImage;

    private void Update()
    {
        UpdateHealthBar(_enemyDamage.health, _enemyDamage.maxHealth, _barImage);
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth, Image barImage)
    {
        float healthLavel = currentHealth / maxHealth;

        barImage.fillAmount = healthLavel;

        if (healthLavel <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
