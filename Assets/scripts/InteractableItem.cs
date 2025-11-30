// InteractableItem.cs
using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    [SerializeField] private string interactionPrompt = "Нажмите F для взаимодействия";
    [SerializeField] private bool isInteractable = true;
    [SerializeField] public GameObject GirlSay;
    public string InteractionPrompt => interactionPrompt;
    public bool IsInteractable => isInteractable;

    public void Interact()
    {
        // Здесь реализуй логику взаимодействия с предметом
        Debug.Log($"Взаимодействовал с {gameObject.name}");

        // Пример: поднять предмет
        // PickUp();

        // Пример: открыть дверь
        // OpenDoor();

        // Пример: активировать механизм
        // ActivateMechanism();

        GirlCanSay();
    }

    // Дополнительные методы для конкретной логики
    private void PickUp()
    {
        // Логика поднятия предмета
        gameObject.SetActive(false);
    }
    private void GirlCanSay()
    {
        GirlSay.SetActive(true);
    }
    private void OpenDoor()
    {
        // Логика открытия двери
        // transform.Rotate(0, 90, 0);
    }
}