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
        "Hi there! I'm your guide today.",
        "Let's talk about suffixes.",
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

    private List<QuestionData> quizQuestions;
    private int currentQuestionIndex = 0;

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

        // Initialize quiz questions
        quizQuestions = new List<QuestionData>
        {
            new QuestionData
            {
                question = "Which of the following is a proper suffix?",
                options = new string[] { "-ness", "pre-", "under" },
                correctAnswerIndex = 0
            },
            new QuestionData
            {
                question = "What does the suffix '-ful' mean?",
                options = new string[] { "Full of", "Without", "Before" },
                correctAnswerIndex = 0
            },
            new QuestionData
            {
                question = "Which word uses the suffix '-ly' correctly?",
                options = new string[] { "Quickly", "Unly", "Prely" },
                correctAnswerIndex = 0
            }
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
        if (currentQuestionIndex < quizQuestions.Count)
        {
            optionsPanel.SetActive(true);
            questionText.text = quizQuestions[currentQuestionIndex].question;

            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = quizQuestions[currentQuestionIndex].options[i];
                optionButtons[i].onClick.RemoveAllListeners();

                int index = i;
                optionButtons[i].onClick.AddListener(() => OnQuizAnswerSelected(index));
            }
        }
        else
        {
            // Quiz completed
            optionsPanel.SetActive(false);
            feedbackPanel.SetActive(true);
            feedbackText.text = "Congratulations! You've completed all questions! 🎉";
            currentQuestionIndex = 0; // Reset for next time
        }
    }

    private void OnQuizAnswerSelected(int index)
    {
        bool isCorrect = index == quizQuestions[currentQuestionIndex].correctAnswerIndex;

        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(true);
        feedbackText.text = isCorrect ? "Correct! 🎉" : "Wrong answer. Try again.";

        if (isCorrect)
        {
            currentQuestionIndex++;
            Invoke(nameof(ShowQuiz), 2f);
        }
        else
        {
            Invoke(nameof(ResetQuiz), 2f);
        }
    }

    private void ResetQuiz()
    {
        feedbackPanel.SetActive(false);
        ShowQuiz();
    }
}