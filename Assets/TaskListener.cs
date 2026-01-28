using UnityEngine;

public class TaskListener : MonoBehaviour
{
    [SerializeField] private string targetTaskId = "pick_phone";
    [SerializeField] private ProximityAudioTrigger proximityTrigger;

    private void OnEnable()
    {
        if (TasksTracker.Instance != null)
        {
            TasksTracker.Instance.OnTaskStarted += HandleTaskStarted;
        }
        else
        {
            Debug.LogWarning("TaskListener: TasksTracker.Instance is NULL in OnEnable. Will retry in Start.");
        }
    }

    private void Start()
    {
        if (TasksTracker.Instance != null)
        {
            TasksTracker.Instance.OnTaskStarted += HandleTaskStarted;
        }
    }

    private void OnDisable()
    {
        TasksTracker.Instance.OnTaskStarted -= HandleTaskStarted;
    }

    private void HandleTaskStarted(string taskId)
    {
        if (taskId == targetTaskId)
        {
            Debug.Log("PickPhone task started => enabling proximity trigger");
            proximityTrigger.EnableTrigger();
        }
    }
}
