// CursorManager.cs
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        LockCursor();
        InvokeRepeating("ForceLock", 0.5f, 0.5f); // Периодически проверяем
    }

    void ForceLock()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Debug.LogWarning("Cursor was unlocked, forcing lock");
            LockCursor();
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}