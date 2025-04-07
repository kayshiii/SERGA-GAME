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
    public Button confirmButton; // Add this in Inspector

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
    private int currentQuestionIndex = 0;
    private int selectedAnswerIndex = -1;

    [System.Serializable]
    public class QuestionData
    {
        public string question;
        public string[] options;
        public int correctAnswerIndex;
    }

    private List<QuestionData> questions = new List<QuestionData>();

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        instance = this;
    }

    private void Start()
    {
        leftArrow.onClick.AddListener(PreviousDialogue);
        rightArrow.onClick.AddListener(NextDialogue);
        confirmButton.onClick.AddListener(ConfirmAnswer);

        UpdateDialogue();
        interactiveBubble.SetActive(false);
        simpleBubble.SetActive(false);
        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(false);
        confirmButton.gameObject.SetActive(false);

        // Define quiz questions
        questions.Add(new QuestionData
        {
            question = "Which of the following is a proper suffix?",
            options = new string[] { "-ness", "pre-", "under" },
            correctAnswerIndex = 0
        });

        questions.Add(new QuestionData
        {
            question = "What suffix can be added to 'happy' to make it mean 'the state of being happy'?",
            options = new string[] { "-ly", "-ness", "-ful" },
            correctAnswerIndex = 1
        });

        questions.Add(new QuestionData
        {
            question = "Which suffix can be added to 'quick' to make it mean 'in a quick manner'?",
            options = new string[] { "-ness", "-ly", "-ful" },
            correctAnswerIndex = 1
        });
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
        if (currentQuestionIndex < questions.Count)
        {
            optionsPanel.SetActive(true);
            confirmButton.gameObject.SetActive(false);
            questionText.text = questions[currentQuestionIndex].question;
            selectedAnswerIndex = -1;

            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuestionIndex].options[i];
                optionButtons[i].onClick.RemoveAllListeners();

                int index = i;
                optionButtons[i].onClick.AddListener(() => SelectAnswer(index));
            }
        }
        else
        {
            // Quiz completed
            optionsPanel.SetActive(false);
            confirmButton.gameObject.SetActive(false);
            feedbackPanel.SetActive(true);
            feedbackText.text = "Congratulations! You've completed the quiz! 🎉";
            currentQuestionIndex = 0; // Reset for next time
        }
    }

    private void SelectAnswer(int index)
    {
        selectedAnswerIndex = index;
        confirmButton.gameObject.SetActive(true);
    }

    private void ConfirmAnswer()
    {
        confirmButton.gameObject.SetActive(false);
        OnQuizAnswerSelected(selectedAnswerIndex);
    }

    private void OnQuizAnswerSelected(int index)
    {
        bool isCorrect = index == questions[currentQuestionIndex].correctAnswerIndex;

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