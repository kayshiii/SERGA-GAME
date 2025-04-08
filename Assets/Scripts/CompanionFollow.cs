using UnityEngine;

public class CompanionFollow : MonoBehaviour
{
    public Transform target;            // Reference to the player
    public float followSpeed = 5f;      // Speed of following
    public float xOffset = 1.5f;        // Horizontal offset distance
    public float bobAmplitude = 0.25f;  // Bobbing height
    public float bobFrequency = 2f;     // Bobbing speed

    private float yOffset;              // Initial vertical offset from player
    private int lastDirection = 1;      // 1 for right, -1 for left

    void Start()
    {
        yOffset = transform.position.y - target.position.y;

        // Optional: auto-detect initial facing direction
        lastDirection = target.localScale.x >= 0 ? 1 : -1;
    }

    void Update()
    {
        // Get horizontal input (assuming you use Input Manager's "Horizontal" axis)
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Update facing direction only when player moves
        if (horizontalInput > 0)
        {
            lastDirection = 1;
        }
        else if (horizontalInput < 0)
        {
            lastDirection = -1;
        }

        // Calculate target position for companion
        float targetX = target.position.x + (xOffset * lastDirection);
        float smoothedX = Mathf.Lerp(transform.position.x, targetX, followSpeed * Time.deltaTime);

        float bobY = target.position.y + yOffset + Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;

        transform.position = new Vector3(smoothedX, bobY, transform.position.z);
    }
}
