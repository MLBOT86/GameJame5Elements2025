using System.Linq;
using UnityEngine;

public class InteractableItem : MonoBehaviour, IInteractable, IInteractableExtended
{
    [Header("Interaction Settings")]
    [SerializeField] private string interactionPrompt = "Нажмите F для взаимодействия";
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private bool isInteractable = true;
    [SerializeField] public GameObject GirlSay;

    [Header("Highlight Settings")]
    //[SerializeField] private Material highlightMaterial; // Материал для подсветки
   // [SerializeField] private Color highlightColor = Color.yellow;
    //[SerializeField] private float highlightIntensity = 1.5f;

   // private Material[] originalMaterials;
    //private Renderer objectRenderer;
    private bool isHighlighted = false;
    private Transform playerTransform;
    public GameObject GirlLight;

    public string InteractionPrompt => interactionPrompt;
    public bool IsInteractable => isInteractable;

    void Start()
    {
       // objectRenderer = GetComponent<Renderer>();
       // if (objectRenderer != null)
       // {
          //  originalMaterials = objectRenderer.materials;
       // }

        // Ищем игрока по тегу
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning($"Игрок не найден для объекта {gameObject.name}. Убедитесь, что у игрока установлен тег 'Player'");
        }

        // Создаем материал для подсветки если он не задан
       // if (highlightMaterial == null)
       // {
           // CreateHighlightMaterial();
       // }
    }



    public float GetInteractionDistance()
    {
        return interactionDistance;
    }

    void Update()
    {
        if (!IsInteractable || playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= interactionDistance && !isHighlighted)
        {
            HighlightObject(true);
        }
        else if (distanceToPlayer > interactionDistance && isHighlighted)
        {
            HighlightObject(false);
        }
    }

    public void Interact()
    {
        if (!IsInteractable) return;

        Debug.Log($"Взаимодействовал с {gameObject.name}");

        // Отключаем возможность взаимодействия
        isInteractable = false;

        // Убираем подсветку
        HighlightObject(false);

        // Вызываем метод взаимодействия
        GirlCanSay();

        // Опционально: отключаем скрипт или объект
        // enabled = false;
        // gameObject.SetActive(false);
        GirlLight.SetActive(false);
    }

    private void GirlCanSay()
    {
        SoundManager.Instance.GoCollectItem();

      
    }

    private void HideGirlSay()
    {
        if (GirlSay != null)
        {
            GirlSay.SetActive(false);
        }
    }

    private void HighlightObject(bool highlight)
    {
        if (highlight)
        {
            GirlLight.SetActive(true);
        }
        else
        {
            GirlLight.SetActive(false);
        }



       
    }

   

   

    // Визуализация радиуса взаимодействия в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}