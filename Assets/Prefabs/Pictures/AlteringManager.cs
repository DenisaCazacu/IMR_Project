using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlteringManager : MonoBehaviour
{
    // List of all the photos in your house
    public List<ImageAltering> allPhotos;

    void Start()
    {
        StartCoroutine(DirectorLoop());
    }

    IEnumerator DirectorLoop()
    {
        // Keep looping until the game ends or all photos are done
        while (true)
        {
            // 1. Get a list of photos that aren't finished yet
            List<ImageAltering> activePhotos = new List<ImageAltering>();
            foreach (var photo in allPhotos)
            {
                if (!photo.IsFullyDecayed)
                {
                    activePhotos.Add(photo);
                }
            }

            // If no photos are left to decay, stop the script
            if (activePhotos.Count == 0)
            {
                Debug.Log("All photos are fully decayed.");
                yield break;
            }

            // 2. Shuffle the list (Randomize the order for this round)
            ShuffleList(activePhotos);

            // 3. Iterate through the shuffled list one by one
            foreach (var photo in activePhotos)
            {
                Debug.Log("Director: It is now " + photo.name + "'s turn.");
                
                // Wait for this specific photo to finish its witness->timer->swap cycle
                yield return StartCoroutine(photo.ProcessNextStage());
                
                // Optional: Small pause between photos so they don't happen back-to-back instantly
                yield return new WaitForSeconds(2f);
            }

            Debug.Log("Director: Round complete. Starting next decay level...");
        }
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}