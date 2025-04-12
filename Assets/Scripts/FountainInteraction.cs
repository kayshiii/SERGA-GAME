using UnityEngine;

public class FountainInteraction : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private bool hasInteracted = false;

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !hasInteracted)
        {
            hasInteracted = true;
            UIManager.instance.ContinueFountainDialogue();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            UIManager.instance.ShowInteractPrompt(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            UIManager.instance.ShowInteractPrompt(false);
        }
    }
} 