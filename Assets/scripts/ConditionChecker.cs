using UnityEngine;

public class ConditionChecker : MonoBehaviour
{
  
    // Точка на уровне
    public Transform targetPoint;

    // Расстояние для проверки
    public float checkDistance = 5f;

    // Ссылка на игрока (можно назначить в инспекторе)
    public Transform player;

    // Флаг, чтобы метод вызывался только один раз
    private bool methodExecuted = false;

    void Update()
    {
        // Проверяем, назначены ли все необходимые компоненты
        if (player == null || targetPoint == null)
        {
            Debug.LogWarning("Player или TargetPoint не назначены!");
            return;
        }

        // Если метод уже был вызван, выходим из функции
        if (!methodExecuted)
            return;

        // Проверяем все условия
        if (CheckAllConditions())
        {
            // Вызываем метод, если все условия выполнены
            ExecuteSpecialMethod();
        }
    }

    bool CheckAllConditions()
    {
        // Проверяем все булевые значения
        bool allConditionsTrue = GameManager.Instance.PlaerTakeAirObj && GameManager.Instance.PlaerTakEarthObj && GameManager.Instance.PlaerTakeFireObj && GameManager.Instance.PlaerTakeWaterObj;

        if (!allConditionsTrue)
            return false;

        // Проверяем расстояние до точки
        float distanceToPoint = Vector3.Distance(player.position, targetPoint.position);

        return distanceToPoint <= checkDistance;
    }

    void ExecuteSpecialMethod()
    {
        // Тут ваш метод, который нужно вызвать
        Debug.Log("Все условия выполнены! Вызываю специальный метод...");

        // Пример: меняем цвет объекта
        if (TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material.color = Color.green;
        }

        // Устанавливаем флаг, чтобы метод больше не вызывался
        methodExecuted = true;

        // Или здесь вызовите ваш собственный метод
        // YourCustomMethod();
    }

    // Вспомогательный метод для визуализации в редакторе
    void OnDrawGizmosSelected()
    {
        if (targetPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(targetPoint.position, checkDistance);
        }
    }

   
}