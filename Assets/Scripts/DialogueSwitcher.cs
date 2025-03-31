using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueSwitcher : MonoBehaviour
{
    public Text dialogueText; // UI text to display dialogue
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

    void Start()
    {
        leftArrow.onClick.AddListener(PreviousDialogue);
        rightArrow.onClick.AddListener(NextDialogue);
        UpdateDialogue();
    }

    void UpdateDialogue()
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
