using UnityEngine;

public class CapsuleMovement : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        // Get input from A and D keys (left and right)
        float moveInput = Input.GetAxis("Horizontal");

        // Move the capsule left or right
        transform.Translate(Vector3.right * moveInput * speed * Time.deltaTime);
    }
}