using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Основное меню игры
public class MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform arrow; // Стрелка выбора
    [SerializeField] private RectTransform[] buttons; // Массив кнопок меню
    private int currentPosition; // Текущая выбранная позиция

    private void Awake()
    {
        ChangePosition(0); // Инициализация начальной позиции
    }

    private void Update()
    {
        // Обработка ввода для навигации
        if (Input.GetKeyDown(KeyCode.UpArrow))
            ChangePosition(-1); // Перемещение вверх
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            ChangePosition(1); // Перемещение вниз

        // Обработка подтверждения выбора
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetButtonDown("Submit"))
            Interact(); // Взаимодействие с выбранной опцией
    }

    // Метод изменения текущей позиции
    public void ChangePosition(int _change)
    {
        currentPosition += _change; // Изменение позиции

        // Циклическая навигация (последняя → первая, первая → последняя)
        if (currentPosition < 0)
            currentPosition = buttons.Length - 1;
        else if (currentPosition > buttons.Length - 1)
            currentPosition = 0;

        AssignPosition(); // Обновление позиции стрелки
    }

    // Установка позиции стрелки
    private void AssignPosition()
    {
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }

    // Взаимодействие с выбранной опцией
    private void Interact()
    {
        if (currentPosition == 0)
        {
            // Запуск игры - загрузка сохраненного уровня или уровня 1 по умолчанию
            SceneManager.LoadScene(PlayerPrefs.GetInt("level", 1));
        }
        else if (currentPosition == 1)
        {
            Application.Quit(); // Выход из игры
        }
    }
}