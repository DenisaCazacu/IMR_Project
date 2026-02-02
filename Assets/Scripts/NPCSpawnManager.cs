using System;
using UnityEngine;

public class NPCSpawnManager : MonoBehaviour
{
    [Header("The NPCs")]
    public GameObject friendNPC;  // Drag your Friend object here
    public GameObject motherNPC;  // Drag your Mother object here

    [Header("Trigger Settings")]
    [Tooltip("The exact Task ID that makes the Friend appear")]
    public string friendSpawnTaskID = "find_umbrella"; 

    [Tooltip("The exact Task ID that swaps Friend for Mother")]
    public string motherSwapTaskID = "find_keys";

    private void Awake()
    {
        if(friendNPC != null) friendNPC.SetActive(false);
        if(motherNPC != null) motherNPC.SetActive(false);
    }

    void Start()
    {
        // 1. Ensure everyone is hidden at the start of the game


        // 2. Subscribe to the Task Tracker
        if (TasksTracker.Instance != null)
        {
            TasksTracker.Instance.OnTaskStarted += CheckForSpawns;
        }
    }

    void OnDestroy()
    {
        // Always unsubscribe when objects are destroyed to prevent errors
        if (TasksTracker.Instance != null)
        {
            TasksTracker.Instance.OnTaskStarted -= CheckForSpawns;
        }
    }

    // This function runs automatically every time a new task starts
    private void CheckForSpawns(string currentTaskID)
    {
        Debug.Log("Task changed to: " + currentTaskID);

        // LOGIC 1: Spawn the Friend
        // e.g., Player gets the task to go downstairs/find umbrella
        if (currentTaskID == friendSpawnTaskID)
        {
            friendNPC.SetActive(true);
            motherNPC.SetActive(false); // Ensure mom is hidden
            Debug.Log("SPAWN LOGIC: Friend has appeared.");
        }

        // LOGIC 2: The Hallucination Swap
        // e.g., Player spoke to friend, now looking for keys. Friend vanishes, Mom appears.
        else if (currentTaskID == motherSwapTaskID)
        {
            friendNPC.SetActive(false); // Friend disappears
            motherNPC.SetActive(true);  // Mother appears in his place
            Debug.Log("SPAWN LOGIC: Swapped Friend for Mother.");
        }
    }
}