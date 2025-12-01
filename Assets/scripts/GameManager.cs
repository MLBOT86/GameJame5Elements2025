using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Синглтон
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("GameManager");
                    _instance = go.AddComponent<GameManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    // Переменные для текста и HP
    [SerializeField] private GameObject displayText;
    public int playerHP = 3;
    [SerializeField] private int maxHP = 5;
    [SerializeField] private GameObject heartContainer;
    [SerializeField] private GameObject heartPrefab;

    private bool textEffectPlayed = false;

    // Про
    //
    // верка на дубликаты при создании

    public bool GameStarted=false;


    [Header("Menu References")]
    public GameObject pauseMenuUI; // Ссылка на UI панель меню паузы

    private bool isPaused = false;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);

        // Инициализация
      
    }

    void Update()
    {
        // Проверка нажатия ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }




    private void InitializeGame()
    {
        // Создаем контейнер для сердечек если он не назначен
        if (heartContainer == null)
        {
            GameObject canvas = FindObjectOfType<Canvas>().gameObject;
            if (canvas != null)
            {
                heartContainer = new GameObject("HeartContainer");
                heartContainer.transform.SetParent(canvas.transform);
                RectTransform rt = heartContainer.AddComponent<RectTransform>();
                rt.anchorMin = new Vector2(1, 1);
                rt.anchorMax = new Vector2(1, 1);
                rt.pivot = new Vector2(1, 1);
                rt.anchoredPosition = new Vector2(-50, -50);
            }
        }

        // Запускаем эффект текста если еще не был запущен
        if (!textEffectPlayed &&(SceneManager.GetActiveScene().name == "SampleScene"))
        {
            StartCoroutine(TextEffectCoroutine());
        }
        else
        {
            // Если эффект уже был, убедимся что текст выключен
            if (displayText != null)
                displayText.SetActive(false);
        }

        // Обновляем отображение HP
        UpdateHeartsDisplay();
    }


    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "SampleScene")
        {

            InitializeGame();
            GameStarted = true;

            LOckedCursor();
        }
    }
    public void StartGame()
    {

        InitializeGame();
        GameStarted = true;
        SoundManager.Instance.StartSoundSays();
        LOckedCursor();
    }



    public void LOckedCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockedCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    // Корутина для эффекта текста
    private IEnumerator TextEffectCoroutine()
    {
        if (displayText == null) yield break;

        // Ждем 3 секунды
        yield return new WaitForSeconds(3f);

        // Включаем текст
        displayText.SetActive(true);

        // Ждем 4 секунды
        yield return new WaitForSeconds(4f);

        // Выключаем текст
        displayText.SetActive(false);

        // Помечаем что эффект уже сработал
        textEffectPlayed = true;
    }

    // Метод для уменьшения HP на 1
    public void TakeDamage()
    {
        if (playerHP > 0)
        {
            playerHP--;
            UpdateHeartsDisplay();
            Debug.Log($"Player took damage! HP: {playerHP}");

            // Проверка на смерть
            if (playerHP <= 0)
            {
                PlayerDied();
            }
        }
    }

    // Метод для лечения (дополнительно)
    public void Heal()
    {
        if (playerHP < maxHP)
        {
            playerHP++;
            UpdateHeartsDisplay();
            Debug.Log($"Player healed! HP: {playerHP}");
        }
    }

    // Метод для отрисовки сердечек
    private void UpdateHeartsDisplay()
    {
        if (heartContainer == null || heartPrefab == null) return;

        // Очищаем старые сердечки
        foreach (Transform child in heartContainer.transform)
        {
            Destroy(child.gameObject);
        }

        // Создаем новые сердечки
        for (int i = 0; i < maxHP; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer.transform);
            RectTransform rt = heart.GetComponent<RectTransform>();

            // Позиционируем сердечки
            rt.anchoredPosition = new Vector2(-i * 60, 0);

            // Если индекс больше текущего HP - делаем сердечко полупрозрачным
            Image heartImage = heart.GetComponent<Image>();
            if (heartImage != null)
            {
                Color heartColor = heartImage.color;
                heartColor.a = (i < playerHP) ? 1f : 0.3f;
                heartImage.color = heartColor;
            }
        }
    }

    // Метод вызывается когда HP достигает 0
    private void PlayerDied()
    {
        Debug.Log("Player died!");
        // Здесь можно добавить логику смерти игрока
        // Например: перезагрузка сцены, показ экрана смерти и т.д.
    }

    // Геттер для HP (может пригодиться)
    public int GetPlayerHP()
    {
        return playerHP;
    }

    // Сеттер для HP (может пригодиться)
    public void SetPlayerHP(int newHP)
    {
        playerHP = Mathf.Clamp(newHP, 0, maxHP);
        UpdateHeartsDisplay();
    }

    // Метод для сброса состояния при перезапуске игры
    public void ResetGame()
    {
        playerHP = 3;
        textEffectPlayed = false;
        UpdateHeartsDisplay();

        if (displayText != null)
            displayText.gameObject.SetActive(false);
    }

    // Вызывается при загрузке новой сцены
    private void OnLevelWasLoaded(int level)
    {
        // Если это первая сцена или нужно переинициализировать
        if (level == 0) // замените 0 на индекс вашей стартовой сцены
        {
            InitializeGame();
        }
    }






    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; // Останавливает игровое время

        // Активируем меню паузы, если оно назначено
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }

        // Дополнительно: отключаем игровые звуки или другие системы
        AudioListener.pause = true;
    }

    /// <summary>
    /// Возобновление игры
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Восстанавливаем нормальное время

        // Скрываем меню паузы
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }

        // Восстанавливаем звук
        AudioListener.pause = false;
    }

    /// <summary>
    /// Закрытие меню паузы (можно вызывать из кнопки UI)
    /// </summary>
    public void ClosePauseMenu()
    {
        ResumeGame();
    }

    /// <summary>
    /// Переход на новую сцену
    /// </summary>
    /// <param name="sceneName">Имя сцены для загрузки</param>
    public void LoadScene(string sceneName)
    {
        // Убедимся, что время восстановлено перед загрузкой сцены
        Time.timeScale = 1f;
        AudioListener.pause = false;

        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Перезапуск текущей сцены
    /// </summary>
    public void RestartScene()
    {
        // Убедимся, что время восстановлено перед перезагрузкой
        Time.timeScale = 1f;
        AudioListener.pause = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Выход из игры...");

        // В редакторе Unity
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // В собранной версии
        Application.Quit();
#endif
    }

    // Дополнительные полезные методы

    /// <summary>
    /// Проверка, находится ли игра на паузе
    /// </summary>
    public bool IsGamePaused()
    {
        return isPaused;
    }

    /// <summary>
    /// Переключение состояния паузы
    /// </summary>
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }


}