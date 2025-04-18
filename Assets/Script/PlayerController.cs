using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 3f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Animator animator;
    private Vector3 velocity;
    public bool isGrounded;
    public bool hasJumped;
    public bool isFalling;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) move += transform.forward;
        if (Input.GetKey(KeyCode.S)) move -= transform.forward;
        if (Input.GetKey(KeyCode.A)) move -= transform.right;
        if (Input.GetKey(KeyCode.D)) move += transform.right;

        move.Normalize();
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            hasJumped = true;
        }

        velocity += Physics.gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        bool isRunning = move.magnitude > 0;
        isFalling = !isGrounded;

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isFalling", isFalling);
    }
}
