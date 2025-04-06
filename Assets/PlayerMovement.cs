using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Animator animator;
    private Vector3 moveInput;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D
        float moveZ = Input.GetAxisRaw("Vertical");   // W/S

        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        // Trigger appropriate animation
        if (moveInput.magnitude > 0)
        {
            if (moveX > 0)
            {
                animator.SetTrigger("WalkRight");
                animator.SetFloat("Speed", moveInput.magnitude);
            }
            else if (moveX < 0)
            {
                animator.SetTrigger("WalkLeft");
                animator.SetFloat("Speed", moveInput.magnitude);
            }
        }

        // Optional: You can reset triggers to avoid constant transitions
        if (moveInput == Vector3.zero)
        {
            animator.ResetTrigger("WalkLeft");
            animator.ResetTrigger("WalkRight");
            animator.SetFloat("Speed", moveInput.magnitude);
        }
    }

    void FixedUpdate()
    {
        transform.position += moveInput * moveSpeed * Time.fixedDeltaTime;
    }
}
