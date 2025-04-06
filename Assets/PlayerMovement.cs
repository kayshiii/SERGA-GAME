using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); // optional if using physics
    }

    void Update()
    {
        // Get input
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        // Set animator Speed parameter based on movement
        animator.SetFloat("Speed", moveInput.magnitude);
    }

    void FixedUpdate()
    {
        // Actually move the player
        transform.position += moveInput * moveSpeed * Time.fixedDeltaTime;
    }
}
