using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 7f;
    public float airControl = 0.5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    [Header("Ground Detection")]
    public LayerMask groundMask = 1;
    public float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    [SerializeField] private Camera playerCamera;
    private float xRotation = 0f;
    [SerializeField] private bool isGrounded;
    private Vector3 moveInput;

    // Input values
    private float mouseX, mouseY;
    private float horizontal, vertical;
    private bool jumpPressed;
    private bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //playerCamera = GetComponentInChildren<Camera>();

        // Настройки Rigidbody
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Курсор в центре и скрыт
        // 

    }

    void Update()
    {

        if (GameManager.Instance.GameStarted)
        {

            GetInput();
            HandleMouseLook();
            
        }

    }

    void FixedUpdate()
    {
        CheckGrounded();
        HandleMovement();
        HandleJump();
    }

    void GetInput()
    {
        // Движение
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        // Мышь
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Прыжок и бег
        jumpPressed = Input.GetKey(KeyCode.Space);
        isRunning = Input.GetKey(KeyCode.LeftShift);
    }

    void HandleMouseLook()
    {
        // Поворот по вертикали (вверх/вниз)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        // Применяем поворот камеры
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Поворот по горизонтали (влево/вправо) - поворачиваем весь объект
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        // Вектор движения относительно взгляда
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        // Рассчитываем желаемое направление движения
        Vector3 desiredMove = (forward * vertical + right * horizontal).normalized;

        // Текущая скорость (бег или ходьба)
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        // Применяем движение
        if (isGrounded)
        {
            // На земле - полный контроль
            Vector3 targetVelocity = desiredMove * currentSpeed;
            targetVelocity.y = rb.velocity.y; // Сохраняем вертикальную скорость

            rb.velocity = targetVelocity;
        }
        else
        {
            // В воздухе - ограниченный контроль
            Vector3 airVelocity = desiredMove * currentSpeed * airControl;
            airVelocity.y = rb.velocity.y;

            rb.velocity = Vector3.Lerp(rb.velocity, airVelocity, Time.fixedDeltaTime * 5f);
        }
    }

    void HandleJump()
    {
        if (jumpPressed && isGrounded)
        {
            // Сбрасываем вертикальную скорость перед прыжком для consistency
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;

            // Применяем силу прыжка
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            
        }
    }

    void CheckGrounded()
    {
        // Raycast для проверки земли
        RaycastHit hit;
        Vector3 rayStart = transform.position + Vector3.up * 0.1f;

        isGrounded = Physics.Raycast(rayStart, Vector3.down, out hit,
                                   groundCheckDistance + 0.1f, groundMask);

        // Визуализация луча в редакторе
        Debug.DrawRay(rayStart, Vector3.down * (groundCheckDistance + 0.1f),
                     isGrounded ? Color.green : Color.red);
    }

    // Опционально: для лучшего контроля можно добавить drag
    void ApplyDrag()
    {
        if (isGrounded)
        {
            Vector3 velocity = rb.velocity;
            velocity.x *= 0.9f;
            velocity.z *= 0.9f;
            rb.velocity = velocity;
        }
    }
}