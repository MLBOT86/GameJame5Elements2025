using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string sceneNameToLoad; // Имя сцены для загрузки
    [SerializeField] private float detectionDistance = 5f; // Дистанция обнаружения игрока
    [SerializeField] private GameObject player;
    private void Update()
    {
        // Проверяем наличие игрока поблизости
      //  GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Вычисляем расстояние до игрока
            float distance = Vector3.Distance(transform.position, player.transform.position);
            Debug.Log(distance);
            // Если игрок достаточно близко - загружаем сцену
            if (distance <= detectionDistance)
            {
                Debug.Log("IseeYou");

                LoadNewScene();
            }
        }
    }

    private void LoadNewScene()
    {
        if (!string.IsNullOrEmpty(sceneNameToLoad))
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }
        else
        {
            Debug.LogWarning("Scene name is not set!");
        }
    }

    // Визуализация зоны обнаружения в редакторе (опционально)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
}