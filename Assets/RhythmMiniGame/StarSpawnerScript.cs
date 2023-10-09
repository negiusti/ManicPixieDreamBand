using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StarSpawnerScript : MonoBehaviour
{
    private int[] stringz = { 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 3, 2, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1 };
    private bool hasStarted = false;
    public GameObject pinkStar;
    public GameObject blackStar;
    public GameObject purpleStar;
    public GameObject redStar;
    public GameObject starter;
    public Vector3 pinkSpawnPosition;
    public Vector3 blackSpawnPosition;
    public Vector3 purpleSpawnPosition;
    public Vector3 redSpawnPosition;
    private string relativePath = "hamster_notes";
    private StreamReader reader;
    private string line;
    private float minX = -7.47f;
    private int i;
    private float delay;
    private AudioSource hamster;
    private Queue<GameObject> spawnedStars;
    private int hitNotes;
    private int missedNotes;

    // Start is called before the first frame update
    void Start()
    {
        hamster = starter.GetComponent<AudioSource>();
        i = 0;
        hitNotes = 0;
        missedNotes = 0;
        spawnedStars = new Queue<GameObject>();
        try
        {
            // Combine the relative path with the current working directory to get the full file path
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
            Debug.Log("PATH: " + fullPath);
            // Check if the file exists
            if (File.Exists(fullPath))
            {
                reader = new StreamReader(fullPath);
                line = reader.ReadLine();
                try
                {
                    delay = float.Parse(line);
                }
                catch (FormatException)
                {
                    Debug.LogError("Invalid float format.");
                }
            }
            else
            {
                Debug.Log("File not found: " + fullPath);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("An error occurred: " + ex.Message);
        }
        pinkSpawnPosition = new Vector3(pinkSpawnPosition.x, pinkStar.transform.position.y, pinkSpawnPosition.z);
        blackSpawnPosition = new Vector3(blackSpawnPosition.x, blackStar.transform.position.y, blackSpawnPosition.z);
        purpleSpawnPosition = new Vector3(purpleSpawnPosition.x, purpleStar.transform.position.y, purpleSpawnPosition.z);
        redSpawnPosition = new Vector3(redSpawnPosition.x, redStar.transform.position.y, redSpawnPosition.z);
    }

    private IEnumerator DelayedActions()
    {
        while ((line = reader.ReadLine()) != null)
        {
            while (hamster.time < delay)
            {
                // Wait until the desired delay time has passed
                yield return null;
            }

            // Perform the action or event here
            SpawnStar();
            try
            {
                delay = float.Parse(line);
            }
            catch (FormatException)
            {
                Debug.LogError("Invalid float format.");
            }
        }
    }

    void Update()
    {
        if (!hasStarted)
        {
            if (starter.transform.position.x < minX)
            {
                StartCoroutine(DelayedActions());
                hasStarted = true;
            }
        } else
        {
            if (spawnedStars.Count > 0)
            {
                // This means the note was "hit"
                if (spawnedStars.Peek() == null)
                {
                    spawnedStars.Dequeue();
                    hitNotes++;
                }
                // This means the note was "missed"
                else if (spawnedStars.Peek().transform.position.x < -12)
                {
                    GameObject starToDestroy = spawnedStars.Dequeue();
                    Destroy(starToDestroy);
                    missedNotes++;
                }
                    
            }
        }
    }

    public float GetScore()
    {
        return hitNotes / (hitNotes + missedNotes);
    }

    private void SpawnStar()
    {
        // spawn note
        int r = stringz[i++];
        if (i == stringz.Length)
            i = 0;
        if (r == 1)
            spawnedStars.Enqueue(Instantiate(pinkStar, pinkSpawnPosition, Quaternion.Euler(new Vector3(0f, 0f, -90f))));
        else if (r == 2)
            spawnedStars.Enqueue(Instantiate(blackStar, blackSpawnPosition, Quaternion.Euler(new Vector3(0f, 0f, -90f))));
        else if (r == 3)
            spawnedStars.Enqueue(Instantiate(purpleStar, purpleSpawnPosition, Quaternion.Euler(new Vector3(0f, 0f, -90f))));
        else if (r == 4)
            spawnedStars.Enqueue(Instantiate(redStar, redSpawnPosition, Quaternion.Euler(new Vector3(0f, 0f, -90f))));
    }   

}
