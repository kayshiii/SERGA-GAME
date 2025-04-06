using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    [SerializeField] GameObject playerChar;
    [SerializeField] GameObject speechTrigger;
    
    void Start()
    {
        speechTrigger.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Pumasok");
        if (other.gameObject == playerChar)
        {
            speechTrigger.SetActive(true);
            Debug.Log("Pumasok");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerChar)
        {
            speechTrigger.SetActive(false);
            Debug.Log("Pumasok");
        }
    }
}