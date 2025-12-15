using UnityEngine;

public class ImageAltering : MonoBehaviour
{
    [Header("Assets")]
    public Texture[] photoStages;

    [Header("Settings")]
    public float minTime = 10f;
    public float maxTime = 30f;

    private Renderer rend;
    private int currentStage = 0;
    private float triggerTime;
    
    // This flag resets to FALSE every time the photo changes
    private bool currentStageWitnessed = false; 

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (photoStages.Length > 0) rend.material.mainTexture = photoStages[0];
    }

    void Update()
    {
        // Stop if we reached the final empty stage
        if (currentStage >= photoStages.Length - 1) return;

        // PHASE 1: Waiting for the player to notice the current photo
        if (!currentStageWitnessed)
        {
            if (rend.isVisible)
            {
                currentStageWitnessed = true;
                SetNextRandomTime();
                Debug.Log($"Player saw Stage {currentStage}. Timer started.");
            }
            return; // Stop here. Don't check for decay yet.
        }

        // PHASE 2: Player has seen it, now we wait for the timer
        if (Time.time >= triggerTime)
        {
            // PHASE 3: Timer is done, wait for player to look away
            if (!rend.isVisible)
            {
                ApplyNextStage();
            }
        }
    }

    void ApplyNextStage()
    {
        currentStage++;
        
        // 1. Change the texture
        rend.material.mainTexture = photoStages[currentStage];
        
        // 2. CRITICAL: Reset the witness flag
        // The script now stops and waits for the player to see this NEW image
        currentStageWitnessed = false;
        
        Debug.Log("Photo changed! Waiting for player to see the new version.");
    }

    void SetNextRandomTime()
    {
        triggerTime = Time.time + Random.Range(minTime, maxTime);
    }
}