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

    [Header("Pause UI")]
    public Button pauseButton; // Add this in Inspector

    [Header("Simple Thought Bubble")]
    public GameObject simpleBubble;
    public TextMeshProUGUI simpleBubbleText;

    [System.Serializable]
    public class QuestionData
    {
        public string question;
        public string[] options;
        public int correctAnswerIndex;
    }

    [System.Serializable]
    public class NPCDialogueSet
    {
        public string npcTag;
        public List<string> dialogues;
        public List<QuestionData> questions;
    }

    public List<NPCDialogueSet> npcDialogueSets;
    private List<QuestionData> currentQuizQuestions;
    private List<string> currentDialogues;
    private int currentQuestionIndex = 0;
    private int selectedOptionIndex = -1;
    private int currentIndex = 0;

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
        pauseButton.onClick.AddListener(() => PauseManager.instance.TogglePause());

        interactiveBubble.SetActive(false);
        simpleBubble.SetActive(false);
        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(false);
        confirmButton.gameObject.SetActive(false);

        // Initialize NPC dialogue sets
        npcDialogueSets = new List<NPCDialogueSet>
        {
            new NPCDialogueSet
            {
                npcTag = "Teacher",
                dialogues = new List<string>
                {
                    "Kamusta! Ako si teacher.",
                    "ANSWER THIS"
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "Ngayon lang tayo nagkakilala tapos [alis] mo na ako sa buhay mo? Wahhh...",
                        options = new string[] { "aalisin", "alisin", "umalis" },
                        correctAnswerIndex = 0
                    },
                    new QuestionData
                    {
                        question = "[alis] muna natin 'to sa isip natin... May bukas pa naman!",
                        options = new string[] { "aalisin", "alisin", "umalis" },
                        correctAnswerIndex = 1
                    },
                    new QuestionData
                    {
                        question = "Tara! [alis] na tayo ngayon!",
                        options = new string[] { "aalisin", "alisin", "umalis" },
                        correctAnswerIndex = 2
                    }
                }
            },
            new NPCDialogueSet
            {
                npcTag = "Gardener",
                dialogues = new List<string>
                {
                    "Hi! Ako si Juan, isang gardener.",
                    "tulong sa sagot hehe",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "Mamaya ko na [dilig] ang mga gumamela.",
                        options = new string[] { "didiligan", "dinidiligan", "diniligan" },
                        correctAnswerIndex = 0
                    },
                    new QuestionData
                    {
                        question = "[dilig] ko ngayon ang mga gumamela.",
                        options = new string[] { "didiligan", "dinidiligan", "diniligan" },
                        correctAnswerIndex = 1
                    },
                    new QuestionData
                    {
                        question = "[dilig] ko na ang mga gumamela kaninang umaga.",
                        options = new string[] { "didiligan", "dinidiligan", "diniligan" },
                        correctAnswerIndex = 2
                    }
                }
            }
        };

        UpdateDialogue();
        interactiveBubble.SetActive(false);
        simpleBubble.SetActive(false);
        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(false);
        confirmButton.gameObject.SetActive(false);
    }

    public void SetNPCDialogueSet(string npcTag)
    {
        NPCDialogueSet dialogueSet = npcDialogueSets.Find(set => set.npcTag == npcTag);
        if (dialogueSet != null)
        {
            currentDialogues = dialogueSet.dialogues;
            currentQuizQuestions = dialogueSet.questions;
            currentIndex = 0;
            currentQuestionIndex = 0;
            UpdateDialogue();
        }
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
        if (currentIndex < currentDialogues.Count)
        {
            dialogueText.text = currentDialogues[currentIndex];
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
        if (currentIndex < currentDialogues.Count)
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
        if (currentQuestionIndex < currentQuizQuestions.Count)
        {
            optionsPanel.SetActive(true);
            confirmButton.gameObject.SetActive(false);
            questionText.text = currentQuizQuestions[currentQuestionIndex].question;
            selectedOptionIndex = -1;

            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuizQuestions[currentQuestionIndex].options[i];
                optionButtons[i].onClick.RemoveAllListeners();

                int index = i;
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
        }
        else
        {
            // Quiz completed
            optionsPanel.SetActive(false);
            confirmButton.gameObject.SetActive(false);
            feedbackPanel.SetActive(true);
            feedbackText.text = "Congratulations! You've completed all questions! 🎉";
            currentQuestionIndex = 0; // Reset for next time
        }
    }

    private void OnOptionSelected(int index)
    {
        selectedOptionIndex = index;
        confirmButton.gameObject.SetActive(true);
        
        // Update question text to show selected option
        string originalQuestion = currentQuizQuestions[currentQuestionIndex].question;
        string selectedOption = currentQuizQuestions[currentQuestionIndex].options[index];
        
        // Find the placeholder word in the question (e.g., [alis] or [dilig])
        int startIndex = originalQuestion.IndexOf('[');
        int endIndex = originalQuestion.IndexOf(']');
        if (startIndex != -1 && endIndex != -1)
        {
            string placeholder = originalQuestion.Substring(startIndex, endIndex - startIndex + 1);
            questionText.text = originalQuestion.Replace(placeholder, selectedOption);
        }
        else
        {
            questionText.text = originalQuestion;
        }
    }

    private void ConfirmAnswer()
    {
        confirmButton.gameObject.SetActive(false);
        OnQuizAnswerSelected(selectedOptionIndex);
    }

    private void OnQuizAnswerSelected(int index)
    {
        bool isCorrect = index == currentQuizQuestions[currentQuestionIndex].correctAnswerIndex;

        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(true);
        feedbackText.text = isCorrect ? "nice one badi! 🎉" : "Parang mali… Nagmamadali na tayo!";

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