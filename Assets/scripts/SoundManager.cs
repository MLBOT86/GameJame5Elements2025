using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    // Синглтон instance
    public static SoundManager Instance { get; private set; }

    // AudioSource компоненты для воспроизведения звуков
    private AudioSource audioSource;

    // Звуковые файлы (добавьте их через инспектор)
    public AudioClip firstSound;
    public AudioClip collectItemSound;
    public AudioClip playerLeaveSound;
    public AudioClip endGameSound;

    // Флаг для отслеживания воспроизведения первого звука
    private bool firstSoundPlayed = false;

    // Настройки громкости
    private float volumeStep = 0.1f; // Шаг изменения громкости
    private float maxVolume = 1.0f;  // Максимальная громкость
    private float minVolume = 0.0f;  // Минимальная громкость

    private void Awake()
    {
        // Реализация синглтона
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Получаем или добавляем AudioSource компонент
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Запускаем первый звук через 2 секунды
        StartCoroutine(PlayFirstSoundAfterDelay());
    }

    // Корутина для воспроизведения первого звука с задержкой
    private IEnumerator PlayFirstSoundAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        PlayFirstSound();
    }

    // Метод для воспроизведения первого звука (только один раз)
    private void PlayFirstSound()
    {
        if (!firstSoundPlayed && firstSound != null)
        {
            audioSource.PlayOneShot(firstSound);
            firstSoundPlayed = true;
            Debug.Log("Первый звук воспроизведен");
        }
    }

    // Метод для сбора предмета
    public void GoCollectItem()
    {
        if (collectItemSound != null)
        {
            audioSource.PlayOneShot(collectItemSound);
            Debug.Log("Звук сбора предмета воспроизведен");
        }
    }

    // Метод для выхода игрока
    public void PlayerLeaveGame()
    {
        if (playerLeaveSound != null)
        {
            audioSource.PlayOneShot(playerLeaveSound);
            Debug.Log("Звук выхода игрока воспроизведен");
        }
    }

    // Метод для завершения игры
    public void EndGameSound()
    {
        if (endGameSound != null)
        {
            audioSource.PlayOneShot(endGameSound);
            Debug.Log("Звук завершения игры воспроизведен");
        }
    }

    // Метод для увеличения громкости
    public void IncreaseVolume()
    {
        float newVolume = audioSource.volume + volumeStep;
        audioSource.volume = Mathf.Clamp(newVolume, minVolume, maxVolume);
        Debug.Log($"Громкость увеличена до: {audioSource.volume:F2}");
    }

    // Метод для уменьшения громкости
    public void DecreaseVolume()
    {
        float newVolume = audioSource.volume - volumeStep;
        audioSource.volume = Mathf.Clamp(newVolume, minVolume, maxVolume);
        Debug.Log($"Громкость уменьшена до: {audioSource.volume:F2}");
    }

    // Дополнительные методы для управления звуком
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
        Debug.Log($"Громкость установлена на: {audioSource.volume:F2}");
    }

    public void StopAllSounds()
    {
        audioSource.Stop();
        Debug.Log("Все звуки остановлены");
    }

    // Метод для получения текущей громкости (может пригодиться)
    public float GetCurrentVolume()
    {
        return audioSource.volume;
    }

    // Метод для установки шага изменения громкости
    public void SetVolumeStep(float step)
    {
        volumeStep = Mathf.Clamp(step, 0.01f, 0.5f);
        Debug.Log($"Шаг громкости установлен на: {volumeStep:F2}");
    }
}