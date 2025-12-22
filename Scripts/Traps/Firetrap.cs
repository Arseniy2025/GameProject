using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Огненная ловушка с задержкой активации
public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage; // Урон от ловушки

    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay; // Задержка перед активацией
    [SerializeField] private float activeTime; // Время активности ловушки
    private Animator anim; // Компонент анимации
    private SpriteRenderer spriteRend; // Компонент отображения спрайта

    private bool triggered; // Ловушка активирована (запущен процесс активации)
    private bool active; // Ловушка активна и наносит урон

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Получение компонентов
        spriteRend = GetComponent<SpriteRenderer>();
    }

    // Обработка входа триггера
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!triggered)
                StartCoroutine(ActivateFiretrap()); // Запуск активации если еще не запущена

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage); // Нанесение урона если ловушка активна
        }
    }

    // Корутина активации ловушки
    private IEnumerator ActivateFiretrap()
    {
        // Подготовка к активации - подсветка красным
        triggered = true; // Установка флага активации
        spriteRend.color = Color.red; // Визуальное предупреждение

        // Ожидание задержки
        yield return new WaitForSeconds(activationDelay);

        // Активация ловушки
        spriteRend.color = Color.white; // Возврат нормального цвета
        active = true; // Установка флага активности
        anim.SetBool("activated", true); // Запуск анимации активации

        // Активная фаза
        yield return new WaitForSeconds(activeTime);

        // Деактивация ловушки
        active = false; // Снятие флага активности
        triggered = false; // Снятие флага активации
        anim.SetBool("activated", false); // Остановка анимации
    }
}