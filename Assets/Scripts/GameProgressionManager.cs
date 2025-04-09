using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{
    public static GameProgressionManager instance;

    private int completedNPCs = 0;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(this);
        instance = this;
    }

    public void IncreaseProgress()
    {
        completedNPCs++;
        Debug.Log("Progress increased. Current progress count: " + completedNPCs);

        if (completedNPCs == 3) // Adjust threshold as needed
        {
            TriggerNextLevel();
        }
    }

    private void TriggerNextLevel()
    {
        Debug.Log("All NPCs completed! Teleporting to the next level...");
        // Teleport logic or scene change goes here
    }
}
