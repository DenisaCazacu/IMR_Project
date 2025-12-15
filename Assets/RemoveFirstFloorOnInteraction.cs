using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.SceneManagement;

public class RemoveFirstFloorOnInteraction : MonoBehaviour
{
    [SerializeField] private string taskId;

    [SerializeField] private XRGrabInteractable grabInteractable;
    [SerializeField] private GameObject objectForTask;
    [SerializeField] private Transform objectFinalPosition;
    [SerializeField] private GameObject floorBlocker;
    [SerializeField] private Transform finalDestination;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private AudioSource audioSource;

    private bool taskActive = false;
    private bool fogActive = false;
    private bool sceneLoaded = false;

    private void Awake()
    {
        FindFirstObjectByType<TasksTracker>().OnTaskStarted += OnTaskStarted;
    }

    private void Update()
    {
        if (taskActive)
        {
            if (grabInteractable.isSelected)
            {
                //EFFECT STARTED
                //TasksTracker.Instance.CompleteTask(taskId);
                RemoveFloor();
                taskActive = false;
                fogActive = true;
            }
        }
        if (fogActive && finalDestination != null && playerTransform != null)
        {
            float camToDest = Vector3.Distance(playerTransform.position, finalDestination.position);
            // Ensure the fog end is always less than the distance to finalDestination
            RenderSettings.fogEndDistance = Mathf.Max(2.1f, camToDest - 0.1f);
            // Check if within 1 unit to load scene
            if (!sceneLoaded && camToDest <= 1f)
            {
                sceneLoaded = true;
                SceneManager.LoadScene("FinalScene");
            }
        }
    }

    private void RemoveFloor()
    {
        floorBlocker.SetActive(true);
        // Enable fog and set start distance
        RenderSettings.fog = true;
        RenderSettings.fogStartDistance = 2f;
        // Set initial fog end distance (will be updated in Update)
        if (finalDestination != null && playerTransform != null)
        {
            float camToDest = Vector3.Distance(playerTransform.position, finalDestination.position);
            RenderSettings.fogEndDistance = Mathf.Max(2.1f, camToDest - 0.8f);
        }
        // Play audio if available
        AudioSource src = audioSource != null ? audioSource : GetComponent<AudioSource>();
        if (src != null)
        {
            src.Play();
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
            taskActive = true;
            objectForTask.transform.position = objectFinalPosition.position;
        }
        else
        {
            taskActive = false;
        }
    }
}
