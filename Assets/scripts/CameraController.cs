using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Основные настройки")]
    public Camera mainCamera; // Первая (игровая) камера
    public Camera menuCamera; // Вторая (меню) камера

    [Header("Позиции и повороты")]
    public Transform startPosition; // Стартовая позиция меню камеры
    public Transform position1; // Позиция для метода 1
    public Transform position2; // Позиция для метода 2
    public Transform position3; // Позиция для метода 3
    public Transform finalPosition; // Финальная позиция для метода 5

    [Header("Канвасы меню")]
    public Canvas menuCanvas1; // Меню для метода 2
    public Canvas menuCanvas2; // Меню для метода 3

    [Header("Настройки движения")]
    public float movementSpeed = 2.0f;
    public float rotationSpeed = 1.0f;

    private bool isMoving = false;

    void Start()
    {
        // Настройка камер при старте
        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (menuCamera != null)
        {
            menuCamera.gameObject.SetActive(true);
            // Устанавливаем стартовую позицию
            if (startPosition != null)
            {
                menuCamera.transform.position = startPosition.position;
                menuCamera.transform.rotation = startPosition.rotation;
            }
        }

        // Отключаем меню при старте
        if (menuCanvas1 != null)
            menuCanvas1.gameObject.SetActive(false);

        if (menuCanvas2 != null)
            menuCanvas2.gameObject.SetActive(false);
    }

    // Метод 1: Плавное движение в определенное положение и поворот
    public void MoveToPosition1()
    {
        if (!isMoving && position1 != null)
            StartCoroutine(MoveCameraTo(position1.position, position1.rotation, null));
    }

    // Метод 2: Движение с запуском меню 1
    public void MoveToPosition2WithMenu1()
    {
        if (!isMoving && position2 != null)
            StartCoroutine(MoveCameraTo(position2.position, position2.rotation, menuCanvas1));
    }

    // Метод 3: Движение с запуском меню 2
    public void MoveToPosition3WithMenu2()
    {
        if (!isMoving && position3 != null)
            StartCoroutine(MoveCameraTo(position3.position, position3.rotation, menuCanvas2));
    }

    // Метод 4: Возврат на стартовую позицию
    public void ReturnToStartPosition()
    {
        if (!isMoving && startPosition != null)
            StartCoroutine(MoveCameraTo(startPosition.position, startPosition.rotation, null));
    }

    // Метод 5: Движение к финальной точке и переключение камер
    public void MoveToFinalAndSwitch()
    {
        if (!isMoving && finalPosition != null)
            StartCoroutine(MoveToFinalPosition());
    }

    // Корутина для плавного движения камеры
    private IEnumerator MoveCameraTo(Vector3 targetPosition, Quaternion targetRotation, Canvas menuToActivate)
    {
        if (isMoving || menuCamera == null) yield break;

        isMoving = true;

        // Отключаем все меню перед началом движения
        DisableAllMenus();

        float progress = 0f;
        Vector3 startPosition = menuCamera.transform.position;
        Quaternion startRotation = menuCamera.transform.rotation;

        while (progress < 1f)
        {
            progress += Time.deltaTime * movementSpeed;

            // Плавное перемещение и вращение
            menuCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            menuCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);

            yield return null;
        }

        // Убеждаемся, что достигли конечной позиции
        menuCamera.transform.position = targetPosition;
        menuCamera.transform.rotation = targetRotation;

        // Активируем меню если указано
        if (menuToActivate != null)
            menuToActivate.gameObject.SetActive(true);

        isMoving = false;
    }

    // Корутина для финального движения и переключения камер
    private IEnumerator MoveToFinalPosition()
    {
        if (isMoving || menuCamera == null || finalPosition == null) yield break;

        isMoving = true;

        // Отключаем все меню
        DisableAllMenus();

        float progress = 0f;
        Vector3 startPosition = menuCamera.transform.position;
        Quaternion startRotation = menuCamera.transform.rotation;

        while (progress < 1f)
        {
            progress += Time.deltaTime * movementSpeed;

            menuCamera.transform.position = Vector3.Lerp(startPosition, finalPosition.position, progress);
            menuCamera.transform.rotation = Quaternion.Lerp(startRotation, finalPosition.rotation, progress);

            yield return null;
        }

        // Переключаем камеры
        SwitchToMainCamera();

        isMoving = false;
    }

    // Метод для переключения на основную камеру
    private void SwitchToMainCamera()
    {
        if (menuCamera != null)
            menuCamera.gameObject.SetActive(false);

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);
    }

    // Метод для отключения всех меню
    private void DisableAllMenus()
    {
        if (menuCanvas1 != null)
            menuCanvas1.gameObject.SetActive(false);

        if (menuCanvas2 != null)
            menuCanvas2.gameObject.SetActive(false);
    }

    // Проверка на движение камеры
    public bool IsCameraMoving()
    {
        return isMoving;
    }
}