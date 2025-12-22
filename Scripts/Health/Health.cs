using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Система здоровья для игрока и врагов
public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth = 5; // Начальное здоровье
    public float currentHealth { get; private set; } // Текущее здоровье (публичное свойство)
    private Animator anim; // Компонент анимации
    private bool dead; // Флаг смерти

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration; // Длительность неуязвимости
    [SerializeField] private int numberOfFlashes; // Количество миганий при неуязвимости
    private SpriteRenderer spriteRend; // Компонент отображения спрайта

    [Header("Components")]
    [SerializeField] private Behaviour[] components; // Компоненты для отключения при смерти
    private bool invulnerable; // Флаг неуязвимости

    private UIManager uiManager; // Менеджер интерфейса

    private void Awake()
    {
        currentHealth = startingHealth; // Инициализация здоровья
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        uiManager = FindObjectOfType<UIManager>(); // Поиск менеджера UI
    }

    // Получение урона
    public void TakeDamage(float _damage)
    {
        if (invulnerable || dead) return; // Если неуязвим или мертв - игнорируем урон

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth); // Уменьшение здоровья

        if (currentHealth > 0) // Если еще жив
        {
            anim.SetTrigger("hurt"); // Анимация получения урона
            StartCoroutine(Invunerability()); // Активация неуязвимости
        }
        else // Если умер
        {
            if (!dead)
            {
                Die(); // Вызов смерти
            }
        }
    }

    // Обработка смерти
    private void Die()
    {
        dead = true; // Установка флага смерти

        // Отключение всех компонентов из массива
        if (components != null)
        {
            foreach (Behaviour component in components)
            {
                component.enabled = false;
            }
        }

        // Остановка физического движения
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Сброс скорости
            rb.gravityScale = 0; // Отключение гравитации
        }

        // Анимация смерти
        anim.SetBool("grounded", true); // Установка на земле
        anim.SetTrigger("die"); // Триггер анимации смерти

        // Вызов Game Over через 1 секунду
        Invoke("CallGameOver", 1f);
    }

    // Вызов Game Over в UI
    private void CallGameOver()
    {
        if (uiManager != null)
        {
            uiManager.GameOver();
        }
        else
        {
            Debug.LogWarning("UIManager не найден!");
        }
    }

    // Добавление здоровья
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    // Корутина неуязвимости
    private IEnumerator Invunerability()
    {
        invulnerable = true; // Установка флага неуязвимости

        // Игнорирование столкновений с врагами
        Physics2D.IgnoreLayerCollision(10, 11, true);

        // Мигание спрайта
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f); // Красный полупрозрачный
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white; // Белый нормальный
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        // Восстановление коллизий
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false; // Снятие флага неуязвимости
    }

    private void Deactivate()
    {
        // Опциональный метод для полной деактивации объекта
    }

    // Возрождение объекта
    public void Respawn()
    {
        if (!dead) return; // Если не умер, не нужно респавнить

        dead = false; // Снятие флага смерти
        currentHealth = startingHealth; // Восстановление здоровья

        // Включение всех компонентов
        if (components != null)
        {
            foreach (Behaviour component in components)
            {
                component.enabled = true;
            }
        }

        // Восстановление физики
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 7; // Значение по умолчанию из PlayerMove
        }

        // Сброс анимаций
        anim.ResetTrigger("die");
        anim.ResetTrigger("hurt");
        anim.Play("Idle"); // Воспроизведение анимации покоя

        // Включение неуязвимости на короткое время
        StartCoroutine(Invunerability());
    }
}