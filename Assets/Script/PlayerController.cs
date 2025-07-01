using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private Animator animator;

    public Vector3 gravityDirection = Vector3.down; // Can be updated from outside (e.g., from GravityManager)
    private bool isGrounded;

    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // Ground check using sphere cast
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDir -= transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDir -= transform.right;
        if (Input.GetKey(KeyCode.D)) moveDir += transform.right;

        moveDir.Normalize();

        Vector3 moveVelocity = moveDir * moveSpeed;
        Vector3 velocityOnPlane = Vector3.ProjectOnPlane(rb.linearVelocity, -gravityDirection);
        Vector3 velocity = moveVelocity + Vector3.Project(rb.linearVelocity, gravityDirection); // Preserve vertical speed
        rb.linearVelocity = velocity;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity += Vector3.up * jumpForce;
        }

        animator.SetBool("isRunning", moveDir.magnitude > 0);
        animator.SetBool("isFalling", !isGrounded);
    }
}
