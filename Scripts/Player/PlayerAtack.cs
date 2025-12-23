using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Система атаки игрока
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown; // Задержка между атаками
    [SerializeField] private Transform firePoint; // Точка выстрела
    [SerializeField] private GameObject[] fireballs; // Массив огненных шаров (пул объектов)

    private Animator anim; // Компонент анимации
    private PlayerMove playerMovement; // Компонент движения игрока
    private float cooldownTimer = Mathf.Infinity; // Таймер перезарядки

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMove>();
    }

    private void Update()
    {
        // Проверка возможности атаки (клик левой кнопкой мыши)
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack(); // Выполнение атаки

        cooldownTimer += Time.deltaTime; // Обновление таймера
    }

    // Выполнение атаки
    private void Attack()
    {
        anim.SetTrigger("attack"); // Анимация атаки
        cooldownTimer = 0; // Сброс таймера

        int fireballIndex = FindFireball(); // Поиск доступного огненного шара
        if (fireballIndex != -1) // Если найден доступный снаряд
        {
            fireballs[fireballIndex].transform.position = firePoint.position; // Установка позиции
            fireballs[fireballIndex].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x)); // Направление
        }
    }

    // Поиск неактивного огненного шара в пуле
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i; // Возврат индекса неактивного снаряда
        }
        return -1; // Все снаряды активны
    }
}
