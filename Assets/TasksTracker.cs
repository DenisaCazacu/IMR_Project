using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TasksTracker : MonoBehaviour
{
    [Serializable]
    class GameTask
    {
        public string id;
        public string description;

        public GameTask(string id, string description)
        {
            this.id = id;
            this.description = description;
        }
    }
    
    [SerializeField] TMP_Text taskText;

    [SerializeField]private List<GameTask> allTasks;

    private int currentTaskIndex = 0;

    public event Action<string> OnTaskStarted;
    
    public static TasksTracker Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        
    }

    private void Start()
    {
        UpdateTask(allTasks[0]);
    }

    private void UpdateTask(GameTask newTask)
    {
        taskText.text = newTask.description;
        OnTaskStarted?.Invoke(newTask.id);
    }

    public void CompleteTask(string taskId)
    {
        if(taskId == allTasks[currentTaskIndex].id)
        {
            currentTaskIndex++;
            if(currentTaskIndex < allTasks.Count)
            {
                UpdateTask(allTasks[currentTaskIndex]);
            }
            else
            {
                taskText.text = "All tasks completed!";
            }
        }
    }
    
}
