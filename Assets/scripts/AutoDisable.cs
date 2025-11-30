using UnityEngine;
using System.Collections;

public class AutoDisable : MonoBehaviour
{
    [SerializeField] private float disableDelay = 3f; // время в секундах до выключения

    void OnEnable()
    {
        // Запускаем корутину для выключения
        StartCoroutine(DisableAfterDelay());
    }

    IEnumerator DisableAfterDelay()
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(disableDelay);

        // Выключаем объект
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        // Останавливаем все корутины при выключении объекта
        StopAllCoroutines();
    }
}