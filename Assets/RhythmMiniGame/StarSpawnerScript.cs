using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class StarSpawnerScript : MonoBehaviour
{
    //private int[] stringz = { 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 3, 2, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1, 1, 1, 3, 3, 3, 2, 2, 1, 1, 2, 2, 1, 1, 1 };
    //private int[] stringz = { 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 4, 4, 4, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 4, 4, 4, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 4, 4, 4, 3, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1, 3, 2, 1, 2, 1, 1 };
    public float lagCorrection;
    private bool hasStarted = false;
    public GameObject pinkStar;
    public GameObject blackStar;
    public GameObject purpleStar;
    public GameObject redStar;
    public StarMoverScript starter;
    public Vector3 pinkSpawnPosition;
    public Vector3 blackSpawnPosition;
    public Vector3 purpleSpawnPosition;
    public Vector3 redSpawnPosition;
    public float highwaySpeed = 5f;
    private int i;
    private float delay;
    private int note;
    private AudioSource hamster;
    private Queue<GameObject> spawnedStars;
    private int hitNotes;
    private int missedNotes;
    private float runwayDelay;
    private MiniGame miniGame;
    private string[] notes;
    private string[] times;
    private Coroutine spawnStarCoroutine;

    private void OnLoadCompleted1(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string songNotes = obj.Result.text;
            notes = songNotes.Split(new char[] { '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            Debug.LogError("Failed to load file. Error: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    private void OnLoadCompleted2(AsyncOperationHandle<TextAsset> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            string songTimes = obj.Result.text;
            times = songTimes.Split(new char[] { '\n', '\r', '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                delay = float.Parse(times[i]);
            }
            catch (FormatException)
            {
                Debug.LogError("Invalid float format.");
            }
        }
        else
        {
            Debug.LogError("Failed to load file. Error: " + obj.OperationException);
        }
        Addressables.Release(obj);
    }

    public float HighwaySpeed()
    {
        return highwaySpeed;
    }

    private void OnEnable()
    {
        i = 0;
        hasStarted = false;
        hitNotes = 0;
        missedNotes = 0;
        hamster = this.GetComponent<AudioSource>();
        // Specify the addressable path (use the address you set in the Addressables Group)
        string addressablePath1 = "Assets/RhythmGameNotes/BodyHorror/bodyhorror_notes.txt";
        string addressablePath2 = "Assets/RhythmGameNotes/BodyHorror/bodyhorror.txt";

        // Load the text file asynchronously
        AsyncOperationHandle<TextAsset> asyncOperation1 = Addressables.LoadAssetAsync<TextAsset>(addressablePath1);
        AsyncOperationHandle<TextAsset> asyncOperation2 = Addressables.LoadAssetAsync<TextAsset>(addressablePath2);

        // Register a callback for when the load operation completes
        asyncOperation1.Completed += OnLoadCompleted1;
        asyncOperation2.Completed += OnLoadCompleted2;

        miniGame = this.transform.parent.gameObject.GetComponent<MiniGame>();
    }

    private void OnDisable()
    {
        hamster.Stop();
        if (spawnedStars != null)
        {
            GameObject star;
            while (spawnedStars.TryDequeue(out star))
            {
                Destroy(star);
            }
        }
        if (spawnStarCoroutine != null)
            StopCoroutine(spawnStarCoroutine);
    }


    // Start is called before the first frame update
    void Start()
    {
        spawnedStars = new Queue<GameObject>();
        pinkSpawnPosition = pinkStar.transform.position;
        pinkSpawnPosition.y = starter.transform.position.y;
        blackSpawnPosition = blackStar.transform.position;
        blackSpawnPosition.y = starter.transform.position.y;
        purpleSpawnPosition = purpleStar.transform.position;
        purpleSpawnPosition.y = starter.transform.position.y;
        redSpawnPosition = redStar.transform.position;
        redSpawnPosition.y = starter.transform.position.y;
    }

    private IEnumerator DelayedActions()
    {
        lagCorrection = ES3.Load("LagCalibration", 0f);
        Debug.Log("LAG COORECTION" + lagCorrection);
        while (i < times.Length)
        {
            while (hamster.time < delay - runwayDelay - lagCorrection) // TODO: ADD LAG CORRECTION HERE!!!!!
            {
                // Wait until the desired delay time has passed
                yield return null;
            }

            // Perform the action or event here
            SpawnStar();
            if (++i >= times.Length)
                break;
            try
            {                
                delay = float.Parse(times[i]);
            }
            catch (FormatException)
            {
                Debug.LogError("Invalid float format.");
            }
        }
        while(hamster.time < hamster.clip.length - 0.5f)
            yield return null;

        //miniGame.SetActive(false);
        miniGame.CloseMiniGame();
        
    }


    void Update()
    {
        if (!hasStarted)
        {
            if (starter.hasPassed())
            {
                hamster.Play();
                runwayDelay = starter.GetRunwayDelay();
                Debug.Log("runway delay is " + runwayDelay);
                spawnStarCoroutine = StartCoroutine(DelayedActions());
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
                else if (spawnedStars.Peek().transform.position.y < -20f)
                {
                    GameObject starToDestroy = spawnedStars.Dequeue();
                    runwayDelay = starToDestroy.GetComponent<StarMoverScript>().GetRunwayDelay();
                    Destroy(starToDestroy);
                    missedNotes++;
                }
                    
            }
        }
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    if (spawnStarCoroutine != null)
        //        StopCoroutine(spawnStarCoroutine);
        //    miniGame.CloseMiniGame();
        //}
    }

    public float GetScore()
    {
        return hitNotes / (hitNotes + missedNotes);
    }

    private void SpawnStar()
    {
        Debug.Log("runway delay is " + runwayDelay);
        if (i < notes.Length)
        {
            note = int.Parse(notes[i]);
        }

        if (note == 1)
        {
            GameObject p = Instantiate(pinkStar, pinkSpawnPosition, Quaternion.identity);
            p.SetActive(true);
            p.transform.parent = gameObject.transform;
            p.transform.localScale = pinkStar.transform.localScale;
            spawnedStars.Enqueue(p);
        }
        else if (note == 2)
        {
            GameObject b = Instantiate(blackStar, blackSpawnPosition, Quaternion.identity);
            b.SetActive(true);
            b.transform.parent = gameObject.transform;
            b.transform.localScale = pinkStar.transform.localScale;
            spawnedStars.Enqueue(b);
        }
        else if (note == 3)
        {
            GameObject p = Instantiate(purpleStar, purpleSpawnPosition, Quaternion.identity);
            p.SetActive(true);
            p.transform.parent = gameObject.transform;
            p.transform.localScale = pinkStar.transform.localScale;
            spawnedStars.Enqueue(p);
        }
        else if (note == 4)
        {
            GameObject x = Instantiate(redStar, redSpawnPosition, Quaternion.identity);
            x.SetActive(true);
            x.transform.parent = gameObject.transform;
            x.transform.localScale = pinkStar.transform.localScale;
            spawnedStars.Enqueue(x);
        }
    }   

}
