using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Комната с врагами
public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies; // Массив врагов в комнате
    private Vector3[] initialPosition; // Начальные позиции врагов

    private void Awake()
    {
        // Сохранение начальных позиций врагов для респавна
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position; // Запоминаем позицию
        }
    }

    // Активация/деактивация комнаты
    public void ActivateRoom(bool _status)
    {
        // Управление активностью врагов и их позициями
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status); // Включение/выключение врага
                enemies[i].transform.position = initialPosition[i]; // Сброс позиции
            }
        }
    }
}
