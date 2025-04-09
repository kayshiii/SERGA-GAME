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
            UIManager.instance.SetNPCDialogueSet(gameObject.tag);
            UIManager.instance.ShowBubbles(true, npcDialogue);
            UIManager.instance.ShowInteractPrompt(false);
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
            UIManager.instance.ShowBubbles(false, "");
        }
    }
}
