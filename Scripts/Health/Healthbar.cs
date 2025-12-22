using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Отображение здоровья игрока на UI
public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health playerHealth; // Ссылка на здоровье игрока
    [SerializeField] private Image totalhealthBar; // Полоса максимального здоровья
    [SerializeField] private Image currenthealthBar; // Полоса текущего здоровья

    private void Start()
    {
        // Инициализация полосы максимального здоровья (предполагается 10 макс. здоровья)
        totalhealthBar.fillAmount = playerHealth.currentHealth / 10;
    }

    private void Update()
    {
        // Обновление полосы текущего здоровья
        currenthealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}