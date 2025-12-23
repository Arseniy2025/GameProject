using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed; // Скорость плавного перемещения камеры между комнатами
    private float currentPosX; // Целевая X-координата для перемещения камеры
    private Vector3 velocity = Vector3.zero; // Вспомогательная переменная для SmoothDamp

    // Параметры для режима следования за игроком
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;

    private void Update()
    {
        // Плавное перемещение камеры к целевой X-координате (центру комнаты)
        // SmoothDamp создает эффект плавного ускорения и замедления
        // Параметры:
        // 1. Текущая позиция камеры
        // 2. Целевая позиция (только X меняется, Y и Z остаются прежними)
        // 3. Ссылка на переменную velocity - хранит текущую скорость для плавности
        // 4. Время достижения цели (чем меньше speed, тем быстрее камера двигается)
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);
    }

    // Метод для переключения камеры на новую комнату
    // Вызывается из скрипта Door при переходе игрока через дверь
    public void MoveToNewRoom(Transform _newRoom)
    {
        // Устанавливаем целевую X-координату как центр новой комнаты
        // Камера начнет плавно перемещаться к этой позиции в Update()
        currentPosX = _newRoom.position.x;
    }
}
