// IInteractable.cs
using UnityEngine;

public interface IInteractable
{
    string InteractionPrompt { get; } // Текст подсказки (опционально)
    bool IsInteractable { get; } // Можно ли взаимодействовать сейчас

    void Interact(); // Метод, который вызывается при взаимодействии
}