using System;
using UnityEngine;

public class CompleteTaskOnDropoff : MonoBehaviour
{
    [SerializeField] private GameObject requiredObject;
    [SerializeField] private string taskId;


    private void Awake()
    {
        FindFirstObjectByType<TasksTracker>().OnTaskStarted += OnTaskStarted;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        TasksTracker.Instance.OnTaskStarted -= OnTaskStarted;
    }

    private void OnTaskStarted(string id)
    {
        if (taskId == id)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == requiredObject)
        {
            TasksTracker tasksTracker = TasksTracker.Instance;
            if(tasksTracker != null)
            {
                tasksTracker.CompleteTask(taskId);
                
                gameObject.SetActive(false);
            }
        }
    }
}
