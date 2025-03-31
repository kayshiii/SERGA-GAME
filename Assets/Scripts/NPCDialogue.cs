using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    [TextArea(3, 5)] public string npcDialogue;

    private bool isPlayerInRange = false;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            UIManager.instance.ShowBubbles(true, npcDialogue);
            UIManager.instance.ShowInteractPrompt(false);
        }

        if (dialogueUI.activeSelf)
        {
            PositionDialogueUI();
        }
    }
    private void PositionDialogueUI()
    {
        Vector3 worldPosition = transform.position + new Vector3(0, 8f, 0); // Interactive bubble at 8f
        Vector3 simpleBubblePosition = transform.position + new Vector3(0, 9f, 0); // Simple bubble at 9f

        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        Vector3 simpleScreenPosition = mainCamera.WorldToScreenPoint(simpleBubblePosition);

        UIManager.instance.interactiveBubble.transform.position = screenPosition;
        UIManager.instance.simpleBubble.transform.position = simpleScreenPosition;
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
            UIManager.instance.ShowBubbles(false, "");
        }
    }
}
