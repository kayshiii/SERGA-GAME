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
    public Button returnButton; // Add this in Inspector

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
    private int correctAnswers = 0;

    [System.Serializable]
    public class QuestionData
    {
        public string question;
        public string[] options;
        public int correctAnswerIndex;
        public string explanation;
    }

    private List<QuestionData> questions = new List<QuestionData>();

    private int selectedOptionIndex = -1;

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
        returnButton.onClick.AddListener(ReturnToOptions);

        UpdateDialogue();
        interactiveBubble.SetActive(false);
        simpleBubble.SetActive(false);
        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);

        // Define quiz questions
        questions.Add(new QuestionData
        {
            question = "Which suffix can be added to 'happy' to form a noun?",
            options = new string[] { "-ness", "-ly", "-ful" },
            correctAnswerIndex = 0,
            explanation = "Adding '-ness' to 'happy' makes 'happiness', which is a noun."
        });

        questions.Add(new QuestionData
        {
            question = "Which suffix can be added to 'quick' to form an adverb?",
            options = new string[] { "-ly", "-ness", "-ful" },
            correctAnswerIndex = 0,
            explanation = "Adding '-ly' to 'quick' makes 'quickly', which is an adverb."
        });

        questions.Add(new QuestionData
        {
            question = "Which suffix can be added to 'wonder' to form an adjective?",
            options = new string[] { "-ful", "-ly", "-ness" },
            correctAnswerIndex = 0,
            explanation = "Adding '-ful' to 'wonder' makes 'wonderful', which is an adjective."
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
            questionText.text = questions[currentQuestionIndex].question;
            confirmButton.gameObject.SetActive(false);
            returnButton.gameObject.SetActive(false);
            selectedOptionIndex = -1;

            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuestionIndex].options[i];
                optionButtons[i].onClick.RemoveAllListeners();

                int index = i;
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
        }
        else
        {
            // Quiz completed
            ShowQuizResults();
        }
    }

    private void OnOptionSelected(int index)
    {
        selectedOptionIndex = index;
        confirmButton.gameObject.SetActive(true);
        returnButton.gameObject.SetActive(true);

        // Disable all option buttons until confirmation
        foreach (Button button in optionButtons)
        {
            button.interactable = false;
        }
    }

    private void ReturnToOptions()
    {
        confirmButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);
        selectedOptionIndex = -1;

        // Re-enable all option buttons
        foreach (Button button in optionButtons)
        {
            button.interactable = true;
        }
    }

    private void ConfirmAnswer()
    {
        if (selectedOptionIndex == -1) return;

        bool isCorrect = selectedOptionIndex == questions[currentQuestionIndex].correctAnswerIndex;

        optionsPanel.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);
        feedbackPanel.SetActive(true);
        
        if (isCorrect)
        {
            correctAnswers++;
            feedbackText.text = "Correct! " + questions[currentQuestionIndex].explanation;
        }
        else
        {
            feedbackText.text = "Wrong answer. " + questions[currentQuestionIndex].explanation;
        }

        currentQuestionIndex++;
        Invoke(nameof(ResetQuiz), 2f);
    }

    private void ShowQuizResults()
    {
        feedbackPanel.SetActive(true);
        feedbackText.text = $"Quiz completed! You got {correctAnswers} out of {questions.Count} correct!";
        
        // Reset quiz state
        currentQuestionIndex = 0;
        correctAnswers = 0;
        
        // Hide quiz UI after showing results
        Invoke(nameof(HideQuizUI), 3f);
    }

    private void HideQuizUI()
    {
        feedbackPanel.SetActive(false);
        optionsPanel.SetActive(false);
        interactiveBubble.SetActive(false);
        simpleBubble.SetActive(false);
    }

    private void ResetQuiz()
    {
        feedbackPanel.SetActive(false);
        // Re-enable all option buttons
        foreach (Button button in optionButtons)
        {
            button.interactable = true;
        }
        ShowQuiz();
    }
}