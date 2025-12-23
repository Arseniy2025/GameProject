using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Универсальная стрелка для выбора в UI (используется для меню настроек, уровней и т.д.)
public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons; // Кнопки для навигации
    private RectTransform arrow; // Ссылка на RectTransform стрелки
    private int currentPosition; // Текущая выбранная позиция

    private void Awake()
    {
        arrow = GetComponent<RectTransform>(); // Получение ссылки на компонент
    }

    private void OnEnable()
    {
        currentPosition = 0; // Сброс позиции при активации
        ChangePosition(0); // Установка начальной позиции
    }

    private void Update()
    {
        // Навигация с использованием W/S и стрелок
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            ChangePosition(-1); // Вверх
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            ChangePosition(1); // Вниз

        // Взаимодействие с выбранной опцией (E или Enter)
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.E))
            Interact(); // Активация выбранной кнопки
    }

    // Изменение позиции выбора
    private void ChangePosition(int _change)
    {
        currentPosition += _change;

        // Проверка изменения (не нужно, если change == 0)
        if (_change != 0)

            // Циклическая навигация по массиву кнопок
            if (currentPosition < 0)
                currentPosition = buttons.Length - 1;
            else if (currentPosition > buttons.Length - 1)
                currentPosition = 0;

        AssignPosition(); // Обновление визуальной позиции
    }

    // Установка визуальной позиции стрелки
    private void AssignPosition()
    {
        // Перемещение стрелки по вертикали к позиции выбранной кнопки
        arrow.position = new Vector3(arrow.position.x, buttons[currentPosition].position.y);
    }

    // Взаимодействие с текущей опцией
    private void Interact()
    {
        // Активация события onClick выбранной кнопки
        buttons[currentPosition].GetComponent<Button>().onClick.Invoke();
    }
}
