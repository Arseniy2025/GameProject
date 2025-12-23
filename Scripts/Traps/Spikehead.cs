using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Шипастая голова, атакующая при обнаружении игрока (наследует от EnemyDamage)
public class Spikehead : EnemyDamage
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed; // Скорость атаки
    [SerializeField] private float range; // Дальность обнаружения игрока
    [SerializeField] private float checkDelay; // Задержка между проверками
    [SerializeField] private LayerMask playerLayer; // Слой игрока для обнаружения
    private Vector3[] directions = new Vector3[4]; // Массив направлений для проверки
    private Vector3 destination; // Целевая позиция для движения
    private float checkTimer; // Таймер проверки
    private bool attacking; // Флаг атаки

    // Сброс состояния при активации
    private void OnEnable()
    {
        Stop(); // Остановка движения
    }

    private void Update()
    {
        // Движение к цели если в режиме атаки
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            // Проверка обнаружения игрока по таймеру
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer(); // Проверка наличия игрока
        }
    }

    // Проверка обнаружения игрока
    private void CheckForPlayer()
    {
        CalculateDirections(); // Расчет направлений

        // Проверка в 4 направлениях
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red); // Визуализация лучей
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            // Если обнаружен игрок и еще не атакуем
            if (hit.collider != null && !attacking)
            {
                attacking = true; // Начало атаки
                destination = directions[i]; // Направление атаки
                checkTimer = 0; // Сброс таймера
            }
        }
    }

    // Расчет направлений для проверки
    private void CalculateDirections()
    {
        directions[0] = transform.right * range; // Направо
        directions[1] = -transform.right * range; // Налево
        directions[2] = transform.up * range; // Вверх
        directions[3] = -transform.up * range; // Вниз
    }

    // Остановка атаки
    private void Stop()
    {
        destination = transform.position; // Установка цели как текущая позиция (остановка)
        attacking = false; // Снятие флага атаки
    }

    // Обработка столкновения
    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Нанесение урона
        Stop(); // Остановка при любом столкновении
    }
}
