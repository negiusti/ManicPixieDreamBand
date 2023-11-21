using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StarSpawnerScript : MonoBehaviour
{
    //private int[] stringz = { 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 3, 2, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1 };
    //private int[] stringz = { 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 4, 4, 4, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 4, 4, 4, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 4, 4, 4, 3, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1 };
    private bool hasStarted = false;
    public GameObject pinkStar;
    public GameObject blackStar;
    public GameObject purpleStar;
    public GameObject redStar;
    public StartSyncerScript starter;
    public Vector3 pinkSpawnPosition;
    public Vector3 blackSpawnPosition;
    public Vector3 purpleSpawnPosition;
    public Vector3 redSpawnPosition;
    //private string relativePath = "hamster_notes";
    private string timesRelativePath = "hamster_easy";
    private string notesRelativePath = "hamster_easy_notes";
    private StreamReader timesReader;
    private StreamReader notesReader;
    private string currTimeLine;
    private string currNoteLine;
    //private int i;
    private float delay;
    private int note;
    private AudioSource hamster;
    private Queue<GameObject> spawnedStars;
    private int hitNotes;
    private int missedNotes;
    private float runwayDelay;

    // Start is called before the first frame update
    void Start()
    {
        hamster = starter.GetComponent<AudioSource>();
        //i = 0;
        hitNotes = 0;
        missedNotes = 0;
        spawnedStars = new Queue<GameObject>();
        try
        {
            // Combine the relative path with the current working directory to get the full file path
            string timesFullPath = Path.Combine(Directory.GetCurrentDirectory(), timesRelativePath);
            string notesFullPath = Path.Combine(Directory.GetCurrentDirectory(), notesRelativePath);
            Debug.Log("PATH: " + timesFullPath);
            // Check if the file exists
            if (File.Exists(timesFullPath))
            {
                timesReader = new StreamReader(timesFullPath);
                currTimeLine = timesReader.ReadLine();
                try
                {
                    delay = float.Parse(currTimeLine);
                }
                catch (FormatException)
                {
                    Debug.LogError("Invalid float format.");
                }
            }
            else
            {
                Debug.Log("File not found: " + timesFullPath);
            }
            if (File.Exists(notesFullPath))
            {
                notesReader = new StreamReader(notesFullPath);
                currNoteLine = notesReader.ReadLine();
                try
                {
                    note = int.Parse(currNoteLine);
                }
                catch (FormatException)
                {
                    Debug.LogError("Invalid int format.");
                }
            }
            else
            {
                Debug.Log("File not found: " + notesFullPath);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("An error occurred: " + ex.Message);
        }

        pinkSpawnPosition = pinkStar.transform.position;
        pinkSpawnPosition.y = 7f;
        blackSpawnPosition = blackStar.transform.position;
        blackSpawnPosition.y = 7f;
        purpleSpawnPosition = purpleStar.transform.position;
        purpleSpawnPosition.y = 7f;
        redSpawnPosition = redStar.transform.position;
        redSpawnPosition.y = 7f;
    }

    private IEnumerator DelayedActions()
    {
        while ((currTimeLine = timesReader.ReadLine()) != null)
        {
            while (hamster.time < delay - runwayDelay)
            {
                // Wait until the desired delay time has passed
                yield return null;
            }

            // Perform the action or event here
            SpawnStar();
            try
            {
                delay = float.Parse(currTimeLine);
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
            if (hamster.isPlaying)
            {
                runwayDelay = starter.GetRunwayDelay();
                Debug.Log("runway delay is " + runwayDelay);
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
                else if (spawnedStars.Peek().transform.position.y < -7f)
                {
                    GameObject starToDestroy = spawnedStars.Dequeue();
                    runwayDelay = starToDestroy.GetComponent<StarMoverScript>().GetRunwayDelay();
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
        Debug.Log("runway delay is " + runwayDelay);
        if ((currNoteLine = notesReader.ReadLine()) != null)
        {
            note = int.Parse(currNoteLine);
        }

        // spawn note
        //int r = stringz[i++];
        //if (i == stringz.Length)
        //    i = 0;
        if (note == 1)
        {
            GameObject p = Instantiate(pinkStar, pinkSpawnPosition, Quaternion.identity);
            p.transform.parent = gameObject.transform;
            spawnedStars.Enqueue(p);
        }
        else if (note == 2)
        {
            GameObject b = Instantiate(blackStar, blackSpawnPosition, Quaternion.identity);
            b.transform.parent = gameObject.transform;
            spawnedStars.Enqueue(b);
        }
        else if (note == 3)
        {
            GameObject p = Instantiate(purpleStar, purpleSpawnPosition, Quaternion.identity);
            p.transform.parent = gameObject.transform;
            spawnedStars.Enqueue(p);
        }
        else if (note == 4)
        {
            GameObject x = Instantiate(redStar, redSpawnPosition, Quaternion.identity);
            x.transform.parent = gameObject.transform;
            spawnedStars.Enqueue(x);
        }
    }   

}
