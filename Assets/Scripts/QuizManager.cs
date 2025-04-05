using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject suffixPanel; // Panel holding the 3 buttons
    public TextMeshProUGUI dialogueText;

    private string correctAnswer;

    // Public method to trigger a suffix question
    public void ShowSuffixQuestion(string sentenceWithWrongSuffix, string correctSuffixWord, string[] options)
    {
        dialogueText.text = sentenceWithWrongSuffix;
        correctAnswer = correctSuffixWord;

        suffixPanel.SetActive(true);

        for (int i = 0; i < suffixPanel.transform.childCount; i++)
        {
            Button button = suffixPanel.transform.GetChild(i).GetComponent<Button>();
            TextMeshProUGUI btnText = button.GetComponentInChildren<TextMeshProUGUI>();

            if (i < options.Length)
            {
                string option = options[i];
                btnText.text = option;

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => OnOptionSelected(option));
            }
        }
    }

    private void OnOptionSelected(string selectedOption)
    {
        if (selectedOption == correctAnswer)
        {
            dialogueText.text = "Correct! Well done!";
        }
        else
        {
            dialogueText.text = "Oops! That’s not the right suffix.";
        }

        suffixPanel.SetActive(false);
    }
}
