using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PhoneRinging : MonoBehaviour
{
    [SerializeField] private string taskId;

    [SerializeField] private XRGrabInteractable grabInteractable;
    private AudioSource audioSource;

    private bool taskActive;
    
    private void Awake()
    {
        FindFirstObjectByType<TasksTracker>().OnTaskStarted += OnTaskStarted;
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (taskActive)
        {
            if (grabInteractable.isSelected)
            {
                TasksTracker.Instance.CompleteTask(taskId);
                taskActive = false;
                audioSource.Stop();
            }
        }
    }

    private void OnDestroy()
    {
        TasksTracker.Instance.OnTaskStarted -= OnTaskStarted;
    }

    private void OnTaskStarted(string id)
    {
        if (taskId == id)
        {
            audioSource.Play();
            taskActive = true;
        }
        else
        {
            audioSource.Stop();
            taskActive = false;
        }
    }

    
}
