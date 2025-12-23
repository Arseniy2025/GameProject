using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Базовый класс для урона от врагов
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] protected float damage; // Величина урона

    // Виртуальный метод обработки столкновения
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            collision.GetComponent<Health>().TakeDamage(damage); // Нанесение урона
    }
}
