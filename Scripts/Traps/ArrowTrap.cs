using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ловушка-арбалет, которая стреляет стрелами с определенной периодичностью
public class ArrowTrap : MonoBehaviour
{
    // Время перезарядки между выстрелами (в секундах)
    [SerializeField] private float attackCooldown;
    
    // Точка, из которой выпускаются стрелы
    [SerializeField] private Transform firePoint;
    
    // Массив стрел (пул объектов для оптимизации)
    [SerializeField] private GameObject[] arrows;
    
    // Таймер для отслеживания времени с последнего выстрела
    private float cooldownTimer;

    // Метод выполнения атаки (выстрел стрелой)
    private void Attack()
    {
        // Сбрасываем таймер перезарядки
        cooldownTimer = 0;

        // Получаем индекс доступной стрелы из пула
        int arrowIndex = FindArrow();
        
        // Устанавливаем позицию стрелы в точку выстрела
        arrows[arrowIndex].transform.position = firePoint.position;
        
        // Активируем стрелу через компонент EnemyProjectle
        arrows[arrowIndex].GetComponent<EnemyProjectle>().ActivateProjectile();
    }

    // Метод поиска неактивной стрелы в пуле объектов
    private int FindArrow()
    {
        // Проходим по всем стрелам в массиве
        for (int i = 0; i < arrows.Length; i++)
        {
            // Проверяем, не активна ли стрела в иерархии сцены
            if (!arrows[i].activeInHierarchy)
                return i; // Возвращаем индекс первой найденной неактивной стрелы
        }
        
        // Если все стрелы активны, возвращаем индекс 0 (переиспользуем первую стрелу)
        return 0;
    }

    // Метод Update вызывается каждый кадр
    private void Update()
    {
        // Увеличиваем таймер на время, прошедшее с последнего кадра
        cooldownTimer += Time.deltaTime;

        // Проверяем, прошло ли достаточно времени для следующего выстрела
        if (cooldownTimer >= attackCooldown)
            Attack(); // Выполняем атаку
    }
}
