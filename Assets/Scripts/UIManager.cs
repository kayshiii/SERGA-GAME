using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Interactive Dialogue UI")]
    public GameObject interactPrompt;
    public GameObject interactiveBubble;

    public TextMeshProUGUI dialogueText;
    public Button leftArrow;
    public Button rightArrow;

    private List<string> dialogues = new List<string>
    {
        "Hello, traveler!",
        "The road ahead is dangerous.",
        "Beware of the dark woods...",
        "Good luck on your journey!"
    };

    private int currentIndex = 0;

    [Header("Non-Interactive Thought Bubble")]
    public GameObject simpleBubble;
    public TextMeshProUGUI simpleBubbleText;

    private void Awake()
    {
        instance = this;
    }

    public void ShowInteractPrompt(bool state)
    {
        interactPrompt.SetActive(state);
    }

    private void Start()
    {
        if (leftArrow != null) leftArrow.onClick.AddListener(PreviousDialogue);
        if (rightArrow != null) rightArrow.onClick.AddListener(NextDialogue);
        UpdateDialogue();

        interactiveBubble.SetActive(false);
        simpleBubble.SetActive(false);
    }
    public void ShowBubbles(bool state, string simpleText)
    {
        interactiveBubble.SetActive(state);
        simpleBubble.SetActive(state);
        simpleBubbleText.text = simpleText;
    }

    private void UpdateDialogue()
    {
        dialogueText.text = dialogues[currentIndex];
    }

    public void PreviousDialogue()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateDialogue();
        }
    }

    public void NextDialogue()
    {
        if (currentIndex < dialogues.Count - 1)
        {
            currentIndex++;
            UpdateDialogue();
        }
    }
}
