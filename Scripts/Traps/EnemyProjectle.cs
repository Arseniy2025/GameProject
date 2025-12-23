using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Снаряд, выпускаемый врагами (наследует от EnemyDamage)
public class EnemyProjectle : EnemyDamage
{
    [SerializeField] private float speed; // Скорость полета
    [SerializeField] private float resetTime; // Время жизни снаряда
    private float lifetime; // Текущее время жизни

    // Активация снаряда (вызывается извне)
    public void ActivateProjectile()
    {
        lifetime = 0; // Сброс времени жизни
        gameObject.SetActive(true); // Активация объекта
    }

    private void Update()
    {
        // Движение снаряда вперед
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        // Увеличение времени жизни
        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false); // Деактивация по истечении времени
    }

    // Обработка столкновения
    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Вызов родительского метода для нанесения урона
        gameObject.SetActive(false); // Деактивация при любом столкновении
    }
}
