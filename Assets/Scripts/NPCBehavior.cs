using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] GameObject playerChar;
    [SerializeField] GameObject speechTrigger;
    [SerializeField] Animator npcAnimator;

    [SerializeField] string[] animationTriggers = new string[3];

    void Start()
    {
        speechTrigger.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerChar)
        {
            speechTrigger.SetActive(true);
            Debug.Log("Entered collider.");

            string npcTag = GetNPCTag();
            UIManager.instance.SetNPCDialogueSet(npcTag, this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerChar)
        {
            speechTrigger.SetActive(false);
            Debug.Log("Exited collider.");
        }
    }
    public void PlayReaction(int choiceIndex)
    {
        if (animationTriggers.Length > choiceIndex)
        {
            npcAnimator.SetTrigger(animationTriggers[choiceIndex]);
        }
    }

    public string GetNPCTag()
    {
        return gameObject.tag;
    }
}