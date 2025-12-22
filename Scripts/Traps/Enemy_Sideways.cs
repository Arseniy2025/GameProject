using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Враг, движущийся горизонтально (вправо-влево)
public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float movementDistance; // Дистанция движения
    [SerializeField] private float speed; // Скорость движения
    [SerializeField] private float damage; // Урон, наносимый игроку
    private bool movingLeft; // Флаг направления движения
    private float leftEdge; // Левая граница движения
    private float rightEdge; // Правая граница движения

    private void Awake()
    {
        // Расчет границ движения относительно начальной позиции
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        // Логика движения влево
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                // Движение влево
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false; // Достигли левой границы - меняем направление
        }
        // Логика движения вправо
        else
        {
            if (transform.position.x < rightEdge)
            {
                // Движение вправо
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true; // Достигли правой границы - меняем направление
        }
    }

    // Обработка столкновения с игроком
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Нанесение урона здоровью игрока
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}