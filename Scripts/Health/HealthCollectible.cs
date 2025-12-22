using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Собираемый предмет для восстановления здоровья
public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue; // Количество восстанавливаемого здоровья
    [SerializeField] private AudioClip pickupSound; // Звук подбора

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Восстановление здоровья игрока
            collision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false); // Деактивация предмета
            // Примечание: звук не воспроизводится в этом коде
        }
    }
}