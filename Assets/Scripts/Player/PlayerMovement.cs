using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    public float moveSpeed;
    public Transform orientation;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    public float groundDrag;
    public bool grounded;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        if (playerHealth != null)
            playerHealth.OnPlayerDeath += OnPlayerDeath;
    }


    private void Update()
    {
        if (playerHealth != null && playerHealth.isDead) return;

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
        MyInput();

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;

        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (playerHealth != null && playerHealth.isDead) return;
        MovePlayer();
    }

    private void MyInput() // Captura la entrada del jugador
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer() // Mueve al jugador
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }
    }
    private void UpdateAnimation() // Actualiza las animaciones del jugador
    {
        if (animator == null) return;

        float speed = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z).magnitude;
        animator.SetFloat("Speed", speed);
    }

    private void OnPlayerDeath() // Maneja la muerte del jugador
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (animator != null)
        {
            animator.SetFloat("Speed", 0f);   // fuerza Idle
            animator.SetTrigger("Death");     // si tienes animación de muerte
        }
    }
}