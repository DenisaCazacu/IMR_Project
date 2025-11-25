using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectPositionRandomizer : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private Transform[] positions;

    private int currentPosition = 0;
    private Renderer rend;
    
    private int randomizeCheckIntervalSeconds = 5;
    private float lastCheckTime;

    private XRGrabInteractable grabInteract;
    private void Awake()
    {
        rend = targetObject.GetComponentInChildren<Renderer>();
        grabInteract = targetObject.GetComponent<XRGrabInteractable>();
    }
    private void Update()
    {
        if(Time.time - lastCheckTime > randomizeCheckIntervalSeconds)
        {
            RandomizeMoved();
            lastCheckTime = Time.time;
        }
    }

    void RandomizeMoved()
    {
        if (!grabInteract.isSelected && Vector3.Distance(targetObject.position,positions[currentPosition].position) > 0.5f && !FOVStateProvider.Instance.IsObjectInFOV(rend))
        {
            currentPosition = (currentPosition + 1) % positions.Length;
            targetObject.position = positions[currentPosition].position;
            print("Randomize");
        }
    }
}
