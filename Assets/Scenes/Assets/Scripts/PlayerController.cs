using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float rotationSpeed = 10f;

    [Header("Salto")]
    public float jumpForce = 7f;

    [Header("Referencias")]
    public Transform cameraTransform;

    [Header("Ground Check")]
    public Transform groundCheck;

    public float groundDistance = 0.3f;

    public LayerMask groundLayer;

    private Rigidbody rb;

    private PlayerInputActions inputActions;

    private Vector2 moveInput;

    private bool isGrounded;

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // CONFIGURACIÓN RIGIDBODY
        rb.interpolation =
            RigidbodyInterpolation.Interpolate;

        rb.collisionDetectionMode =
            CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        // INPUT MOVIMIENTO
        moveInput =
            inputActions.Player.Move.ReadValue<Vector2>();

        // INPUT SALTO
        if (inputActions.Player.Jump.triggered)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        Move();
    }

    void Move()
    {
        // DIRECCIÓN DE LA CÁMARA
        Vector3 cameraForward =
            cameraTransform.forward;

        Vector3 cameraRight =
            cameraTransform.right;

        // ELIMINAR INCLINACIÓN
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // NORMALIZAR
        cameraForward.Normalize();
        cameraRight.Normalize();

        // MOVIMIENTO RELATIVO A CÁMARA
        Vector3 movementDirection =
            cameraForward * moveInput.y +
            cameraRight * moveInput.x;

        // NORMALIZAR
        if (movementDirection.magnitude > 1f)
        {
            movementDirection.Normalize();
        }

        // VELOCIDAD
        float currentSpeed = walkSpeed;

        if (inputActions.Player.Sprint.IsPressed())
        {
            currentSpeed = sprintSpeed;
        }

        // MOVIMIENTO
        Vector3 movement =
            movementDirection *
            currentSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(
            rb.position + movement
        );

        // ROTACIÓN
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(
                    movementDirection
                );

            Quaternion smoothRotation =
                Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                );

            rb.MoveRotation(
                smoothRotation
            );
        }
    }

    void Jump()
    {
        if (!isGrounded) return;

        rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundLayer
        );
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundDistance
        );
    }
}