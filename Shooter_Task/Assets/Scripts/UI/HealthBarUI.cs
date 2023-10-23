using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Player _player; 
    [SerializeField] private Image _barImage;

    private void Update()
    {
        UpdateHealthBar(_player.health, _player.maxHealth, _barImage);
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