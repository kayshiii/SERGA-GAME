using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance; // Singleton pattern for easy access

    public GameObject interactPrompt; // "Press E to talk" UI

    private void Awake()
    {
        instance = this;
    }

    public void ShowInteractPrompt(bool state)
    {
        interactPrompt.SetActive(state);
    }
}
