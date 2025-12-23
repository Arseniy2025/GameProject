using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Менеджер пользовательского интерфейса, управляющий экранами паузы и завершения игры
public class UIManager : MonoBehaviour
{
    [Header("Game Over")]
    [SerializeField] private GameObject gameOverScreen; // Экран завершения игры

    [Header("Pause")]
    [SerializeField] private GameObject pauseScreen; // Экран паузы

    private void Awake()
    {
        // Инициализация: скрываем оба экрана при запуске
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }
    
    private void Update()
    {
        // Проверка нажатия клавиши Escape для паузы
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Если экран паузы уже активен - снимаем паузу, иначе ставим на паузу
            PauseGame(!pauseScreen.activeInHierarchy);
        }
    }

    #region Game Over
    // Активация экрана завершения игры
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    // Перезапуск текущего уровня
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Возврат в главное меню
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Выход из игры / выход из режима игры в редакторе
    public void Quit()
    {
        Application.Quit(); // Закрытие игры (работает только в сборке)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Выход из режима игры (выполняется только в редакторе)
#endif
    }
    #endregion

    #region Pause
    // Управление паузой в игре
    public void PauseGame(bool status)
    {
        // Если status == true - ставим на паузу | если status == false - снимаем с паузы
        pauseScreen.SetActive(status);

        // Когда игра на паузе, устанавливаем timescale в 0 (время останавливается)
        // когда пауза снята, возвращаем timescale в 1 (время идет нормально)
        if (status)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    #endregion
}
