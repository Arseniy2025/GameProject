// Дополнительные функции, которые можно добавить:

// 1. Направление выстрела
[SerializeField] private Vector2 shootDirection = Vector2.right;

// 2. Случайный разброс
[SerializeField] private float spreadAngle = 5f;

// 3. Анимация выстрела
private Animator animator;


private void Start()
{
    // Инициализация компонентов
    animator = GetComponent<Animator>();
}

private void Attack()
{
    cooldownTimer = 0;
    
    // Воспроизведение анимации
    if (animator != null)
        animator.SetTrigger("Shoot");
    
    
    // Получение стрелы
    int arrowIndex = FindArrow();
    if (arrowIndex == -1) return; // Если стрел нет
    
    // Установка позиции и направления
    GameObject arrow = arrows[arrowIndex];
    arrow.transform.position = firePoint.position;
    
    // Добавление случайного разброса
    float randomAngle = Random.Range(-spreadAngle, spreadAngle);
    Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);
    arrow.transform.rotation = rotation;
    
    // Активация
    arrow.GetComponent<EnemyProjectle>().ActivateProjectile();
}

// Улучшенный метод поиска
private int FindArrow()
{
    for (int i = 0; i < arrows.Length; i++)
    {
        if (!arrows[i].activeInHierarchy)
            return i;
    }
    return -1; // Явное указание, что стрел нет
}
