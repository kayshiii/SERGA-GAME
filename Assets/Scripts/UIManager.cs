using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject interactPrompt; 

    private void Awake()
    {
        instance = this;
    }

    public void ShowInteractPrompt(bool state)
    {
        interactPrompt.SetActive(state);
    }
}
