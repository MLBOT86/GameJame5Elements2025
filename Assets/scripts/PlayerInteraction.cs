// PlayerInteraction.cs
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayer;

    [Header("UI References")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private GameObject interactionPrompt;

    private Camera playerCamera;
    private IInteractable currentInteractable;

    void Start()
    {
        playerCamera = GetComponent<Camera>();

        // Настройка курсора
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Активируем прицел
        if (crosshair != null)
            crosshair.SetActive(true);

        // Скрываем подсказку взаимодействия
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        CheckForInteractable();
        HandleInteractionInput();
    }

    private void CheckForInteractable()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null && interactable.IsInteractable)
            {
                currentInteractable = interactable;
                ShowInteractionPrompt(interactable.InteractionPrompt);
                return;
            }
        }

        // Если не нашли взаимодействующий объект
        currentInteractable = null;
        HideInteractionPrompt();
    }

    private void HandleInteractionInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    private void ShowInteractionPrompt(string prompt = "")
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(true);

        if (interactionText != null && !string.IsNullOrEmpty(prompt))
            interactionText.text = prompt;
    }

    private void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
}