using UnityEngine;
using UnityEngine.SceneManagement;

public class BGSound : MonoBehaviour
{
    public static BGSound Instance;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    [Header("Настройки музыки")]
    public SceneMusic[] sceneMusics;

    [Header("Настройки громкости")]
    [Range(0f, 1f)]
    public float initialVolume = 0.5f;
    public float volumeChangeStep = 0.1f;

    private AudioSource audioSource;
    private string currentSceneName;

    void Awake()
    {
        // Реализация Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Инициализация AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.volume = initialVolume;
        audioSource.loop = false; // Отключаем авто-повтор, будем контролировать вручную

        // Подписываемся на событие смены сцены
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // Запускаем музыку для начальной сцены
        PlaySceneMusic(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        // Проверяем, закончилась ли музыка и перезапускаем если нужно
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlaySceneMusic(scene.name);
    }

    void PlaySceneMusic(string sceneName)
    {
        // Если сцена не изменилась, ничего не делаем
        if (currentSceneName == sceneName) return;

        currentSceneName = sceneName;

        // Ищем музыку для текущей сцены
        AudioClip newClip = FindMusicForScene(sceneName);

        if (newClip != null)
        {
            audioSource.clip = newClip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Не найдена музыка для сцены: {sceneName}");
        }
    }

    AudioClip FindMusicForScene(string sceneName)
    {
        foreach (var sceneMusic in sceneMusics)
        {
            if (sceneMusic.sceneName == sceneName)
            {
                return sceneMusic.musicClip;
            }
        }
        return null;
    }

    // Метод для увеличения громкости
    public void IncreaseVolume()
    {
        audioSource.volume = Mathf.Clamp01(audioSource.volume + volumeChangeStep);
        Debug.Log($"Громкость увеличена до: {audioSource.volume}");
    }

    // Метод для уменьшения громкости
    public void DecreaseVolume()
    {
        audioSource.volume = Mathf.Clamp01(audioSource.volume - volumeChangeStep);
        Debug.Log($"Громкость уменьшена до: {audioSource.volume}");
    }

    void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}