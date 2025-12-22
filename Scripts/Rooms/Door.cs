using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Дверь между комнатами, управляющая камерой
public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom; // Предыдущая комната
    [SerializeField] private Transform nextRoom; // Следующая комната
    [SerializeField] private CameraController cam; // Контроллер камеры

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Определение направления движения игрока
            if (collision.transform.position.x < transform.position.x)
                cam.MoveToNewRoom(nextRoom); // Движение вправо - следующая комната
            else
                cam.MoveToNewRoom(previousRoom); // Движение влево - предыдущая комната
        }
    }
}