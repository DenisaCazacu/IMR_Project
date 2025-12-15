using System.Collections;
using UnityEngine;

public class ImageAltering : MonoBehaviour
{
    public Texture[] photoStages;
    public float minWaitTime = 5f; // Time to wait AFTER being seen
    public float maxWaitTime = 15f;

    private Renderer rend;
    private int currentStage = 0;

    public bool IsFullyDecayed => currentStage >= photoStages.Length - 1;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        // Ensure we start clean
        if (photoStages.Length > 0) rend.material.mainTexture = photoStages[0];
    }

    // The Manager calls this function to start the spooky process for THIS specific photo
    public IEnumerator ProcessNextStage()
    {
        if (IsFullyDecayed) yield break;

        // 1. WAIT FOR WITNESS (Player must see current stage first)
        // We loop every frame until the object is visible
        while (!rend.isVisible)
        {
            yield return null; 
        }
        Debug.Log(gameObject.name + ": Player has seen me. Timer started.");

        // 2. WAIT FOR TIMER (Random delay)
        float waitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(waitTime);

        // 3. WAIT FOR HIDE (Player must look away to trigger the scare)
        while (rend.isVisible)
        {
            yield return null;
        }

        // 4. SWAP TEXTURE
        currentStage++;
        rend.material.mainTexture = photoStages[currentStage];
        Debug.Log(gameObject.name + ": Swapped to Stage " + currentStage);
    }
}