using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;
    public Transform panTarget; // Target to pan to
    private bool isPanning = false;
    private Vector3 originalPosition;
    private float panDuration = 2f;
    private float panTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        if (isPanning)
        {
            panTimer += Time.deltaTime;
            float progress = panTimer / panDuration;

            if (progress < 1f)
            {
                // Pan to target with upward movement
                Vector3 targetPos = new Vector3(panTarget.position.x, panTarget.position.y + yOffset + 5f, -30f); // Added +2f to y position
                transform.position = Vector3.Lerp(originalPosition, targetPos, progress);
            }
            else
            {
                // Return to player
                Vector3 playerPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
                transform.position = Vector3.Lerp(transform.position, playerPos, FollowSpeed * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, playerPos) < 0.1f)
                {
                    isPanning = false;
                    // Notify UIManager that pan is complete
                    UIManager.instance.OnCameraPanComplete();
                }
            }
        }
        else
        {
            // Normal follow behavior
            Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
        }
    }

    public void StartPan(Transform panToTarget, float duration = 5f)
    {
        if (!isPanning)
        {
            isPanning = true;
            panTarget = panToTarget;
            panDuration = duration;
            panTimer = 0f;
            originalPosition = transform.position;
        }
    }
}