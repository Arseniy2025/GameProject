using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Проектиль игрока
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed; // Скорость полета
    private float direction; // Направление полета
    private bool hit; // Флаг попадания
    private float lifetime; // Время жизни снаряда

    private Animator anim; // Компонент анимации
    private BoxCollider2D boxCollider; // Компонент коллайдера

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (hit) return; // Если попал, прекращаем движение

        // Движение снаряда
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime; // Обновление времени жизни
        if (lifetime > 5) gameObject.SetActive(false); // Самоуничтожение через 5 секунд
    }

    // Обработка столкновения
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true; // Установка флага попадания
        boxCollider.enabled = false; // Отключение коллайдера
        anim.SetTrigger("explode"); // Анимация взрыва

        // Нанесение урона врагу если столкнулись с ним
        if (collision.tag == "Enemy")
            collision.GetComponent<Health>()?.TakeDamage(1); // Оператор ?. - безопасный вызов
    }

    // Инициализация снаряда
    public void SetDirection(float _direction)
    {
        lifetime = 0; // Сброс времени жизни
        direction = _direction; // Установка направления
        gameObject.SetActive(true); // Активация объекта
        hit = false; // Сброс флага попадания
        boxCollider.enabled = true; // Включение коллайдера

        // Разворот спрайта в зависимости от направления
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // Деактивация снаряда (вызывается из анимации)
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}