using UnityEngine;
using System.Collections;

public class CameraControllerSimple : MonoBehaviour
{
    [Header("Основные настройки")]
    public Camera mainCamera;
    public Camera menuCamera;

    [Header("Стартовая и финальная позиции (Transform)")]
    public Transform startPosition;
    public Transform finalPosition;



    public Vector3 Startposition1 = Vector3.zero;

    [Tooltip("Поворот X, Y, Z в градусах")]
    public Vector3 Startrotation1 = Vector3.zero;



    [Header("Позиции для методов 1-3")]
    [Tooltip("Позиция X, Y, Z")]
    public Vector3 position1 = Vector3.zero;

    [Tooltip("Поворот X, Y, Z в градусах")]
    public Vector3 rotation1 = Vector3.zero;

    [Space(10)]
    public Vector3 position2 = Vector3.zero;
    public Vector3 rotation2 = Vector3.zero;

    [Space(10)]
    public Vector3 position3 = Vector3.zero;
    public Vector3 rotation3 = Vector3.zero;

    [Header("Канвасы меню")]

    public Canvas menuCanvasStart;
    public Canvas menuCanvas1;
    public Canvas menuCanvas2;
    public Canvas menuCanvas3;


    public Canvas playerCanvas;

    [Header("Настройки движения")]
    public float movementSpeed = 2.0f;

    private bool isMoving = false;

    void Start()
    {
        InitializeCameras();
    }

    void InitializeCameras()
    {
        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (menuCamera != null)
        {
            menuCamera.gameObject.SetActive(true);
            if (startPosition != null)
            {
                menuCamera.transform.position = startPosition.position;
                menuCamera.transform.rotation = startPosition.rotation;
            }
        }

        DisableAllMenus();
    }

    // Методы для движения
    public void MoveToPosition1()
    {


        menuCanvasStart.gameObject.SetActive(false);
        if (!isMoving)
            StartCoroutine(MoveCameraTo(position1, Quaternion.Euler(rotation1), menuCanvas1));
    }

    public void MoveToPosition2WithMenu1()
    {
        menuCanvasStart.gameObject.SetActive(false);
        if (!isMoving)
            StartCoroutine(MoveCameraTo(position2, Quaternion.Euler(rotation2), menuCanvas3));
    }

    public void MoveToPosition3WithMenu2()
    {
        menuCanvasStart.gameObject.SetActive(false);
        if (!isMoving)
            StartCoroutine(MoveCameraTo(position3, Quaternion.Euler(rotation3), menuCanvas2));
    }

    public void ReturnToStartPosition()
    {
        menuCanvas1.gameObject.SetActive(false);

        menuCanvas2.gameObject.SetActive(false);

        menuCanvas3.gameObject.SetActive(false);

        if (!isMoving && startPosition != null)
            StartCoroutine(MoveCameraTo(Startposition1, Quaternion.Euler( Startrotation1), menuCanvasStart));




    }



    public void MoveToFinalAndSwitch()
    {

        if (!isMoving && finalPosition != null)
            StartCoroutine(MoveToFinalPosition());
    }

    private IEnumerator MoveCameraTo(Vector3 targetPosition, Quaternion targetRotation, Canvas menuToActivate)
    {
        if (isMoving || menuCamera == null) yield break;

        isMoving = true;
        DisableAllMenus();

        Vector3 startPos = menuCamera.transform.position;
        Quaternion startRot = menuCamera.transform.rotation;
        float progress = 0f;

        // Фаза 1: Сначала вращение
        while (progress < 1f)
        {
            progress += Time.deltaTime * movementSpeed;
            // Используем Lerp для вращения, позиция остается неизменной
            menuCamera.transform.rotation = Quaternion.Lerp(startRot, targetRotation, progress);
            yield return null;
        }

        // Сбрасываем прогресс для фазы движения
        progress = 0f;
        Quaternion finalRotation = menuCamera.transform.rotation; // Убедимся, что вращение завершено

        // Фаза 2: Затем перемещение
        while (progress < 1f)
        {
            progress += Time.deltaTime * movementSpeed;
            // Используем Lerp для позиции, вращение остается целевым
            menuCamera.transform.position = Vector3.Lerp(startPos, targetPosition, progress);
            // Убедимся, что вращение остается целевым на протяжении движения
            menuCamera.transform.rotation = finalRotation;
            yield return null;
        }

        // Финализация позиции и вращения
        menuCamera.transform.position = targetPosition;
        menuCamera.transform.rotation = targetRotation;

        if (menuToActivate != null)
            menuToActivate.gameObject.SetActive(true);

        isMoving = false;
    }

    private IEnumerator MoveToFinalPosition()
    {
        if (isMoving || menuCamera == null || finalPosition == null) yield break;

        isMoving = true;
        DisableAllMenus();

        Vector3 startPos = menuCamera.transform.position;
        Quaternion startRot = menuCamera.transform.rotation;
        float progress = 0f;

        while (progress < 1f)
        {
            progress += Time.deltaTime * movementSpeed;
            menuCamera.transform.position = Vector3.Lerp(startPos, finalPosition.position, progress);
            menuCamera.transform.rotation = Quaternion.Lerp(startRot, finalPosition.rotation, progress);
            yield return null;
        }

        SwitchToMainCamera();
        isMoving = false;


        menuCanvas1.gameObject.SetActive(false);

        menuCanvas2.gameObject.SetActive(false);

        menuCanvas3.gameObject.SetActive(false);
        menuCanvasStart.gameObject.SetActive(false);



        playerCanvas.gameObject.SetActive(true);
        GameManager.Instance.StartGame();
    }

    private void SwitchToMainCamera()
    {
        if (menuCamera != null) menuCamera.gameObject.SetActive(false);
        if (mainCamera != null) mainCamera.gameObject.SetActive(true);
    }

    private void DisableAllMenus()
    {
        if (menuCanvas1 != null) menuCanvas1.gameObject.SetActive(false);
        if (menuCanvas2 != null) menuCanvas2.gameObject.SetActive(false);
    }

    public bool IsCameraMoving() => isMoving;
}