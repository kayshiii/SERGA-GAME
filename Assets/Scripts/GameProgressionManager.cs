using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{
    [SerializeField] GameObject playerChar;
    [SerializeField] GameObject spawnPoint1;
    [SerializeField] GameObject spawnPoint2;
    [SerializeField] GameObject spawnPoint3;

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

        if (completedNPCs == 3) // After 3 NPCs completed
        {
            TriggerLevel2();
        }
        else if (completedNPCs == 6) // After 6 NPCs completed
        {
            TriggerLevel3();
        }
    }

    private void TriggerLevel2()
    {
        Debug.Log("3 NPCs completed! Teleporting to Level 2...");
        TeleportPlayer(spawnPoint2);
    }

    private void TriggerLevel3()
    {
        Debug.Log("6 NPCs completed! Teleporting to Level 3...");
        TeleportPlayer(spawnPoint3);
    }

    private void TeleportPlayer(GameObject spawnPoint)
    {
        if (playerChar != null && spawnPoint != null)
        {
            playerChar.transform.position = spawnPoint.transform.position;
            playerChar.transform.rotation = spawnPoint.transform.rotation;
        }
        else
        {
            Debug.LogWarning("Player or spawn point not assigned.");
        }
    }
}
