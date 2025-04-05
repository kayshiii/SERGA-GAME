using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Dialogue UI")]
    public GameObject interactPrompt;
    public GameObject interactiveBubble;
    public TextMeshProUGUI dialogueText;
    public Button leftArrow;
    public Button rightArrow;

    [Header("Quiz UI")]
    public GameObject optionsPanel;
    public Button[] optionButtons; // Set this to 3 buttons in Inspector
    public TextMeshProUGUI questionText;
    public GameObject feedbackPanel;
    public TextMeshProUGUI feedbackText;

    [Header("Simple Thought Bubble")]
    public GameObject simpleBubble;
    public TextMeshProUGUI simpleBubbleText;

    private List<string> dialogues = new List<string>
    {
        "Hi there! I’m your guide today.",
        "Let’s talk about suffixes.",
        "A suffix is a group of letters placed at the end of a word to change its meaning.",
        "For example, adding '-ness' to 'happy' makes 'happiness'.",
        "Ready to test your knowledge?"
    };

    private int currentIndex = 0;

    [System.Serializable]
    public class QuestionData
    {
        public string question;
        public string[] options;
        public int correctAnswerIndex;
    }

    private QuestionData suffixQuestion;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        instance = this;
    }

    private void Start()
    {
        leftArrow.onClick.AddListener(PreviousDialogue);
        rightArrow.onClick.AddListener(NextDialogue);

        UpdateDialogue();
        interactiveBubble.SetActive(false);
        simpleBubble.SetActive(false);
        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(false);

        // Define quiz content
        suffixQuestion = new QuestionData
        {
            question = "Which of the following is a proper suffix?",
            options = new string[] { "-ness", "pre-", "under" },
            correctAnswerIndex = 0
        };
    }

    public void ShowInteractPrompt(bool state)
    {
        interactPrompt.SetActive(state);
    }

    public void ShowBubbles(bool state, string simpleText)
    {
        interactiveBubble.SetActive(state);
        simpleBubble.SetActive(state);
        simpleBubbleText.text = simpleText;
        currentIndex = 0;
        UpdateDialogue();
    }

    private void UpdateDialogue()
    {
        if (currentIndex < dialogues.Count)
        {
            dialogueText.text = dialogues[currentIndex];
            ShowArrows(true);
        }
        else
        {
            dialogueText.text = "";
            ShowArrows(false);
            ShowQuiz();
        }
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
        if (currentIndex < dialogues.Count)
        {
            currentIndex++;
            UpdateDialogue();
        }
    }

    private void ShowArrows(bool show)
    {
        leftArrow.gameObject.SetActive(show);
        rightArrow.gameObject.SetActive(show);
    }

    private void ShowQuiz()
    {
        optionsPanel.SetActive(true);
        questionText.text = suffixQuestion.question;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = suffixQuestion.options[i];
            optionButtons[i].onClick.RemoveAllListeners();

            int index = i;
            optionButtons[i].onClick.AddListener(() => OnQuizAnswerSelected(index));
        }
    }

    private void OnQuizAnswerSelected(int index)
    {
        bool isCorrect = index == suffixQuestion.correctAnswerIndex;

        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(true);
        feedbackText.text = isCorrect ? "Correct! 🎉" : "Wrong answer. Try again.";

        if (!isCorrect)
        {
            Invoke(nameof(ResetQuiz), 2f);
        }
        else
        {
            Debug.Log("Quiz completed successfully!");
            // Optionally: unlock next step or trigger event here
        }
    }

    private void ResetQuiz()
    {
        feedbackPanel.SetActive(false);
        ShowQuiz();
    }
}