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
    public Button confirmButton; 
    public GameObject questionSelectionPanel; 
    public Button[] questionButtons; 

    [Header("Pause UI")]
    public Button pauseButton; 

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

    [SerializeField] private NPCBehavior activeNPC; // To be set dynamically (during gameplay)

    public List<NPCDialogueSet> npcDialogueSets;
    private List<QuestionData> currentQuizQuestions;
    private List<string> currentDialogues;
    private int currentQuestionIndex = 0;
    private int selectedOptionIndex = -1;
    private int currentIndex = 0;
    private string currentNPCTag;
    public int correctOrWrong;


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
        questionSelectionPanel.SetActive(false);

        // Initialize question buttons
        for (int i = 0; i < questionButtons.Length; i++)
        {
            int index = i;
            questionButtons[i].onClick.AddListener(() => SelectQuestion(index));
        }

        // Initialize NPC dialogue sets
        npcDialogueSets = new List<NPCDialogueSet>
        {
            // ------------------------------------------ SCENE 1: SCHOOL GARDEN (TUTORIAL) ------------------------------------------
            new NPCDialogueSet
            {
                npcTag = "Tutorial",
                dialogues = new List<string>
                {
                    "Andddd scene!",
                    "Parang hindi ako nadala sa sinabi mo… Pindutin ang [R] upang bumalik sa simula ng usapan natin."
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "Ngayon lang tayo nagkakilala tapos [alis] mo na ako sa buhay mo? Wahhh...",
                        options = new string[] { "<b>aalisin</b>", "<b>alisin</b>", "<b>umalis</b>" },
                        correctAnswerIndex = 0
                    },
                    new QuestionData
                    {
                        question = "Tara! [Alis] na tayo ngayon!",
                        options = new string[] { "<b>Aalisin</b>", "<b>Alisin</b>", "<b>Umalis</b>" },
                        correctAnswerIndex = 2
                    },
                    new QuestionData
                    {
                        question = "[Alis] muna natin 'to sa isip natin... May bukas pa naman!",
                        options = new string[] { "<b>Aalisin</b>", "<b>Alisin</b>", "<b>Umalis</b>" },
                        correctAnswerIndex = 1
                    }
                }
            },
            // ------------------------------------------ SCENE 1: SCHOOL GARDEN (PROPER) ------------------------------------------
            new NPCDialogueSet
            {
                npcTag = "Gardener",
                dialogues = new List<string>
                {
                    "*nagiisip ng malalim*",
                    "Hala, nakalimutan kong <b>diligan</b> ang mga gumamela!",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "Mamaya ko na <b>[dilig]</b> ang mga gumamela.",
                        options = new string[] { "<b>diniligan</b>", "<b>dinidiligan</b>", "<b>didiligan</b>" },
                        correctAnswerIndex = 2
                    },
                    new QuestionData
                    {
                        question = "<b>[Dilig]</b> ko ngayon ang mga gumamela.",
                        options = new string[] { "<b>Diniligan</b>", "<b>Dinidiligan</b>", "<b>Didiligan</b>" },
                        correctAnswerIndex = 1
                    },
                    new QuestionData
                    {
                        question = "<b>[Dilig]</b> ko na ang mga gumamela kaninang umaga.",
                        options = new string[] { "<b>Diniligan</b>", "<b>Dinidiligan</b>", "<b>Didiligan</b>" },
                        correctAnswerIndex = 0
                    }
                }
            },
            new NPCDialogueSet
            {
                npcTag = "Sam",
                dialogues = new List<string>
                {
                    "hays..",
                    "Sayang, di ko nakuhaan ng picture… Tapos nang <b>dilaan</b> ng aso yung pusa…"
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "Kailan kaya ulit <b>[dila]</b> ng aso yung pusa…?",
                        options = new string[] { "<b>dinilaan</b>", "<b>dinidilaan</b>", "<b>didilaan</b>" },
                        correctAnswerIndex = 2
                    },
                    new QuestionData
                    {
                        question = "<b>[dila]</b> ng aso yung pusa kani-kanina lang… Di ko nakuhaan ng picture…",
                        options = new string[] { "<b>Dinilaan</b>", "<b>Dinidilaan</b>", "<b>Didilaan</b>" },
                        correctAnswerIndex = 0
                    },
                    new QuestionData
                    {
                        question = "Ang cute! <b>[dila]</b> ng aso yung pusa ngayon! Kuhaan ko nga ng picture!",
                        options = new string[] { "<b>Dinilaan</b>", "<b>Dinidilaan</b>", "<b>Didilaan</b>" },
                        correctAnswerIndex = 1
                    }
                }
            },
            new NPCDialogueSet
            {
                npcTag = "Gamot",
                dialogues = new List<string>
                {
                    "<b>Nakainom</b> ba ako ng gamot ko bago umalis ng bahay kanina?",
                    "Sumasakit kasi ulo ko...",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "Kung nadala ko lang sana yung gamot ko, edi sana <b>[inom]</b> ko na siya ngayon.",
                        options = new string[] { "<b>uminom</b>", "<b>iniinom</b>", "<b>iinom</b>" },
                        correctAnswerIndex = 1,
                    },
                    new QuestionData
                    {
                        question = "<b>[Inom]</b> na lang ako ng gamot paguwi ko. Sana hindi lumala itong sakit ng ulo ko…",
                        options = new string[] { "<b>Uminom", "<b>Iniinom</b>", "<b>Iinom</b>" },
                        correctAnswerIndex = 2
                    },
                    new QuestionData
                    {
                        question = "Buti na lang <b>[inom]</b> ako ng gamot, sa ganitong init baka sumakit ulit ulo ko.",
                        options = new string[] { "<b>uminom</b>", "<b>iniinom</b>", "<b>iinom</b>" },
                        correctAnswerIndex = 0
                    }
                }
            },
            // ------------------------------------------ SCENE 2: SCHOOL HALLWAY (PROPER) ------------------------------------------
            new NPCDialogueSet
            {
                npcTag = "Jasmine",
                dialogues = new List<string>
                {
                    "Hays!",
                    "Kanina ka pa <b>naglalaro</b> dyan! Ako naman!",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "Kanina mo pa <b>[laro]</b> 'yan! Ako naman!",
                        options = new string[] { "<b>nilalaro</b>", "<b>makikipaglaro</b>", "laruin</b>" },
                        correctAnswerIndex = 0,
                    },
                    new QuestionData
                    {
                        question = "<b>[Laro]</b> na lang ako sa iba! Ang damot-damot mo!",
                        options = new string[] { "<b>Nilalaro</b>", "<b>Makikipaglaro</b>", "<b>Laruin</b>" },
                        correctAnswerIndex = 1
                    },
                    new QuestionData
                    {
                        question = "Sabay na lang natin <b>[laro]</b> 'yan! Mas masaya pa!",
                        options = new string[] { "<b>nilalaro</b>", "<b>makikipaglaro</b>", "<b>laruin</b>" },
                        correctAnswerIndex = 2
                    }
                }
            },
            new NPCDialogueSet
            {
                npcTag = "Kyle",
                dialogues = new List<string>
                {
                    "...",
                    "Matagal ko nang <b>pinipigil</b> ang damdamin ko para sayo... Iligtas mo ako, Jenny!",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "<b>[Pigil]</b> mo ang pagka-cute mo, Jenny! 'Wag ka tumingin sa akin!",
                        options = new string[] { "<b>Makapipigil</b>", "<b>Pigilan</b>", "<b>Mapigil</b>" },
                        correctAnswerIndex = 1,
                    },
                    new QuestionData
                    {
                        question = "Walang <b>[pigil]</b> sa akin! Ako lang ang para sayo, Jenny!",
                        options = new string[] { "<b>makapipigil</b>", "<b>pigilan</b>", "<b>mapigil</b>" },
                        correctAnswerIndex = 0
                    },
                    new QuestionData
                    {
                        question = "Hindi ko na <b>[pigil]</b> ang damdamin ko para sayo... Mahal kita, Jenny!",
                        options = new string[] { "<b>makapipigil</b>", "<b>pigilan</b>", "<b>mapigil</b>" },
                        correctAnswerIndex = 2
                    }
                }
            },
            new NPCDialogueSet
            {
                npcTag = "Maria",
                dialogues = new List<string>
                {
                    "hmm...",
                    "Bakit kailangan ko pang <b>bumili</b> ng regalo ni Kuya Don? Marami naman siyang pera.",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "<b>[bili]</b> naman na ni Kuya Don lahat ng gusto niya. Hindi na niya kailangan ng regalo.",
                        options = new string[] { "<b>Bumili</b>", "<b>Binili</b>", "<b>Binilhan</b>" },
                        correctAnswerIndex = 1,
                    },
                    new QuestionData
                    {
                        question = "<b>[bili]</b> ko na ng regalo si Kuya Don. Tinatago ko lang sa inyo dahil sorpresa ito...",
                        options = new string[] { "<b>Bumili</b>", "<b>Binili</b>", "<b>Binilhan</b>" },
                        correctAnswerIndex = 2
                    },
                    new QuestionData
                    {
                        question = "<b>[bili]</b> naman na sila Mama ng regalo para kay Kuya Don. Hindi pa ba kasya yun?",
                        options = new string[] { "<b>Bumili</b>", "<b>Binili</b>", "<b>Binilhan</b>" },
                        correctAnswerIndex = 0
                    }
                }
            },
            // ------------------------------------------ SCENE 3: NEIGHBORHOOD (PROPER) ------------------------------------------
            new NPCDialogueSet
            {
                npcTag = "Vendor",
                dialogues = new List<string>
                {
                    "Isa pong taho, kuya…",
                    "Naubos na lahat ng taho, bata. <b>Bumalik</b> ka na lang bukas.",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "[Ubos] lang ng taho, pasenya na.",
                        options = new string[] { "<b>Inuubos</b>", "<b>Kauubos</b>", "<b>Maubos</b>" },
                        correctAnswerIndex = 1,
                    },
                    new QuestionData
                    {
                        question = "Malapit nang [ubos] ang taho! Buti na lang umabot ka pa!",
                        options = new string[] { "<b>inuubos</b>", "<b>mauubos</b>", "<b>maubos</b>" },
                        correctAnswerIndex = 2
                    },
                    new QuestionData
                    {
                        question = "Hindi mo naman [ubos] eh…",
                        options = new string[] { "<b>inuubos</b>", "<b>kauubos</b>", "<b>maubos</b>" },
                        correctAnswerIndex = 0
                    }
                }
            },
            new NPCDialogueSet
            {
                npcTag = "Elle",
                dialogues = new List<string>
                {
                    "Nasaan na yung earphones na pinahiram ko sayo kahapon? Bago pa lang yun…",
                    "Saan ko ba <b>iniwan</b> ang earphones ni Kris…?",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "Ah, [iwan] ko sa sinuot kong pantalon kahapon! Teka, balikan ko sa bahay!",
                        options = new string[] { "<b>iniiwan</b>", "<b>iniwan</b>", "<b>naiwan</b>" },
                        correctAnswerIndex = 2,
                    },
                    new QuestionData
                    {
                        question = "Di ko alam… Kahit saan-saan ko kasi [iwan] mga gamit ko, pasensya na!",
                        options = new string[] { "<b>iniiwan</b>", "<b>iniwan</b>", "<b>naiwan</b>" },
                        correctAnswerIndex = 0
                    },
                    new QuestionData
                    {
                        question = "[Iwan] ka na ng earphones mo… Akin na siya, hehe",
                        options = new string[] { "<b>Iniiwan</b>", "<b>Iniwan</b>", "<b>Naiwan</b>" },
                        correctAnswerIndex = 1
                    }
                }
            },
            new NPCDialogueSet
            {
                npcTag = "Aling Anne",
                dialogues = new List<string>
                {
                    "Mamaya na lang tayo mag-usap, ‘nak. <b>Nilalaba</b> ko ngayon ang damit ni Elle.",
                },
                questions = new List<QuestionData>
                {
                    new QuestionData
                    {
                        question = "<b>[Laba]</b> ko lang ng damit ni Elle. Sa wakas, may oras na ako para makapahinga…",
                        options = new string[] { "<b>Kalalaba</b>", "<b>Naglalaba</b>", "<b>Maglalaba</b>" },
                        correctAnswerIndex = 0,
                    },
                    new QuestionData
                    {
                        question = "Hay, medyo nalulungkot ako tuwing <b>[laba]</b> ako ng damit ni Elle… Dati, ang liliit pa lang ng sinusuot niya…",
                        options = new string[] { "<b>kalalaba</b>", "<b>naglalaba</b>", "<b>maglalaba</b>" },
                        correctAnswerIndex = 1
                    },
                    new QuestionData
                    {
                        question = "[Laba] na nga ako ng damit ni Elle habang may lakas pa ako...",
                        options = new string[] { "<b>Kalalaba</b>", "<b>Naglalaba</b>", "<b>Maglalaba</b>" },
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

    public void SetNPCDialogueSet(string npcTag, NPCBehavior npc)
    {
        NPCDialogueSet dialogueSet = npcDialogueSets.Find(set => set.npcTag == npcTag);
        if (dialogueSet != null)
        {
            currentNPCTag = npcTag;
            currentDialogues = dialogueSet.dialogues;
            currentQuizQuestions = dialogueSet.questions;
            currentIndex = 0;
            currentQuestionIndex = 0;
            activeNPC = npc;
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
        if (currentQuizQuestions != null && currentQuizQuestions.Count > 0)
        {
            questionSelectionPanel.SetActive(true);
            optionsPanel.SetActive(false);
            confirmButton.gameObject.SetActive(false);
            feedbackPanel.SetActive(false);

            // Update question button texts
            for (int i = 0; i < questionButtons.Length; i++)
            {
                if (i < currentQuizQuestions.Count)
                {
                    questionButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    questionButtons[i].gameObject.SetActive(false);
                }
            }
        }
    }

    private void SelectQuestion(int questionIndex)
    {
        if (questionIndex < currentQuizQuestions.Count)
        {
            currentQuestionIndex = questionIndex;
            questionSelectionPanel.SetActive(true);
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
    }

    private void OnOptionSelected(int index)
    {
        selectedOptionIndex = index;
        confirmButton.gameObject.SetActive(true);
        questionSelectionPanel.SetActive(false);
        
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

    public void OnQuizAnswerSelected(int index)
    {
        bool isCorrect = index == currentQuizQuestions[currentQuestionIndex].correctAnswerIndex;

        if (currentNPCTag == "Gamot") // If NPC only has correct/wrong animations
        {
            if (isCorrect)
            {
                correctOrWrong = 0;
                activeNPC.PlayReaction(correctOrWrong); // Play "correct" animation
            }
            else if(!isCorrect)
            {
                correctOrWrong = 1;
                activeNPC.PlayReaction(correctOrWrong); // Play "wrong" animation
            }    
        }
        else if (currentNPCTag == "Jasmine") // If NPC only has correct/wrong animations
        {
            if (isCorrect)
            {
                correctOrWrong = 0;
                activeNPC.PlayReaction(correctOrWrong); // Play "correct" animation
            }
            else if(!isCorrect)
            {
                correctOrWrong = 1;
                activeNPC.PlayReaction(correctOrWrong); // Play "wrong" animation
            }    
        }
        else if (currentNPCTag == "Kyle") // If NPC only has correct/wrong animations
        {
            if (isCorrect)
            {
                correctOrWrong = 0;
                activeNPC.PlayReaction(correctOrWrong); // Play "correct" animation
            }
            else if(!isCorrect)
            {
                correctOrWrong = 1;
                activeNPC.PlayReaction(correctOrWrong); // Play "wrong" animation
            }    
        }
        else if (currentNPCTag == "Maria") // If NPC only has correct/wrong animations
        {
            if (isCorrect)
            {
                correctOrWrong = 0;
                activeNPC.PlayReaction(correctOrWrong); // Play "correct" animation
            }
            else if(!isCorrect)
            {
                correctOrWrong = 1;
                activeNPC.PlayReaction(correctOrWrong); // Play "wrong" animation
            }    
        }
        else if (currentNPCTag == "Vendor") // If NPC only has correct/wrong animations
        {
            if (isCorrect)
            {
                correctOrWrong = 0;
                activeNPC.PlayReaction(correctOrWrong); // Play "correct" animation
            }
            else if(!isCorrect)
            {
                correctOrWrong = 1;
                activeNPC.PlayReaction(correctOrWrong); // Play "wrong" animation
            }    
        }
        else if (currentNPCTag == "Elle") // If NPC only has correct/wrong animations
        {
            if (isCorrect)
            {
                correctOrWrong = 0;
                activeNPC.PlayReaction(correctOrWrong); // Play "correct" animation
            }
            else if(!isCorrect)
            {
                correctOrWrong = 1;
                activeNPC.PlayReaction(correctOrWrong); // Play "wrong" animation
            }    
        }
        else if (currentNPCTag == "Aling Anne") // If NPC only has correct/wrong animations
        {
            if (isCorrect)
            {
                correctOrWrong = 0;
                activeNPC.PlayReaction(correctOrWrong); // Play "correct" animation
            }
            else if(!isCorrect)
            {
                correctOrWrong = 1;
                activeNPC.PlayReaction(correctOrWrong); // Play "wrong" animation
            }    
        }
        else // If NPC has unique animations per answer
        {
            activeNPC.PlayReaction(index); // Play NPC's reaction depending on player's answer
        }

        optionsPanel.SetActive(false);
        feedbackPanel.SetActive(true);
        
        // Set feedback text based on NPC tag and correctness
        if (isCorrect)
        {
            feedbackText.text = "Nice one!!!";
        }
        else
        {
            switch(currentNPCTag)
            {
                case "Gardener":
                    feedbackText.text = "Maarte sa pagdilig ang gumamela. Dapat kanina pa ito nadiligan…";
                    break;
                case "Sam":
                    feedbackText.text = "Dapat kuhaan ng picture ni Sam habang nangyayari ito…";
                    break;
                case "Gamot":
                    feedbackText.text = "Kailangan niyang makainom ng gamot!";
                    break;
                case "Jasmine":
                    feedbackText.text = "Hmph!";
                    break;
                case "Kyle":
                    feedbackText.text = "Ha? Anong pinagsasabi mo?";
                    break;
                case "Maria":
                    feedbackText.text = "Ang bait sa atin ni kuya tapos ganyan ka!";
                    break;
                case "Vendor":
                    feedbackText.text = "Wahhh!!!";
                    break;
                case "Elle":
                    feedbackText.text = "Kung hindi mo ibinalik sa akin, isusumbong kita sa tatay ko!";
                    break;
                case "Aling Anne":
                    feedbackText.text = "Hala, nabasa na yung earphones ni Kris… Paano na ito?";
                    break;
                default:
                    feedbackText.text = "Parang mali…";
                    break;
            }
        }

        if (isCorrect)
        {
            // Remove the answered question
            currentQuizQuestions.RemoveAt(currentQuestionIndex);
            
            // If all questions are answered
            if (currentQuizQuestions.Count == 0)
            {
                feedbackText.text = "Nuks!! Nakumplento mo mga tanong ko! ";
                Invoke(nameof(ResetQuiz), 2f);
                switch(currentNPCTag)
                {
                    case "Gardener":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                    case "Sam":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                    case "Gamot":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                    case "Jasmine":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                    case "Kyle":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                    case "Maria":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                    case "Vendor":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                    case "Elle":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                    case "Aling Anne":
                        GameProgressionManager.instance.IncreaseProgress();
                        break;
                }
            }
            else
            {
                Invoke(nameof(ShowQuiz), 2f);
            }
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